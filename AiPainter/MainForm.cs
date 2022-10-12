using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AiPainter.InvokeAiAdapter;
using AiPainter.LamaCleanerAdapter;
using AiPainter.RemBgAdapter;

#pragma warning disable CS8602

namespace AiPainter
{
    public partial class MainForm : Form
    {
        private const int VIEWPORT_WIDTH = 512;
        private const int VIEWPORT_HEIGHT = 512;
        
        private const int PEN_SIZE = 10;
        private const int MOVE_SIZE = 64;
        
        private static readonly Primitive UNDO_DELIMITER = new() { Kind = PrimitiveKind.UndoDelimiter };

        private List<Primitive> primitives = new();
        private readonly List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? lastPrim => primitives.LastOrDefault();

        private static readonly HatchBrush primBrush = new(HatchStyle.Percent75, Color.Red, Color.Transparent);
        private static readonly Pen primPen = PenTools.CreateRoundPen(primBrush);
        
        private static readonly HatchBrush cursorBrush = new(HatchStyle.Percent75, Color.LightCoral, Color.Transparent);
        
        private string? filePath;

        private Point? cursorPt;

        private static readonly ImageStorage storage = new();

        public MainForm()
        {
            InitializeComponent();

            btPen1_Click(null, null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            hPicScroll.Minimum = 0;
            hPicScroll.SmallChange = 1;
            hPicScroll.Scroll += (_, ee) => updateImages(ee.NewValue);

            splitContainer.Panel2.MouseWheel += (_, ee) =>
            {
                hPicScroll.Value = Math.Max(hPicScroll.Minimum, Math.Min(hPicScroll.Maximum, hPicScroll.Value + (ee.Delta > 0 ? -1 : 1)));
                updateImages(null);
            };

            Task.Run(async () =>
            {
                while (Visible)
                {
                    if (!generating)
                    {
                        bool changesDetected;
                        lock (storage)
                        {
                            changesDetected = storage.Update();
                        }
                        if (changesDetected)
                        {
                            Invoke(() => updateImages(null));
                        }
                    }

                    for (var i = 0; i < 10 && Visible; i++) await Task.Delay(100);
                }
            });

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                if (File.Exists(args[1]))
                {
                    filePath = args[1];
                    pictureBox.Image = BitmapTools.Load(filePath);
                }
            }
        }

        private string? rightButtonPressed;
        
        private void updateImages(int? hPicScrollValue)
        {
            var sz = Math.Max(50, splitContainer.Panel2.ClientSize.Height);
            var x = 0;
            var n = 0;
            
            lock (storage)
            {
                hPicScroll.LargeChange = Math.Max(1, splitContainer.Panel2.ClientSize.Width / (sz + 10) - 1);
                hPicScroll.Maximum = Math.Max(0, storage.Count - 2);

                var j = hPicScrollValue ?? hPicScroll.Value;
                while (x < splitContainer.Panel2.ClientSize.Width && j < storage.Count)
                {
                    var pb = (PictureBox?)splitContainer.Panel2.Controls.Find("pic" + n, false).FirstOrDefault();
                    if (pb == null)
                    {
                        pb = new PictureBox();
                        pb.Name = "pic" + n;
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        pb.Cursor = Cursors.UpArrow;
                        pb.Parent = splitContainer.Panel2;

                        pb.MouseDown += (_, e) =>
                        {
                            switch (e.Button)
                            {
                                case MouseButtons.Left:
                                    filePath = (string)pb.Tag;
                                    pictureBox.Image = BitmapTools.Clone((Bitmap)pb.Image);
                                    primitives.Clear();
                                    redoPrimitiveBlocks.Clear();
                                    pictureBox.Refresh();
                                    break;

                                case MouseButtons.Right:
                                    rightButtonPressed = (string)pb.Tag;
                                    pb.Capture = true;

                                    Task.Run(async () =>
                                    {
                                        var start = DateTime.Now;

                                        while (rightButtonPressed != null)
                                        {
                                            await Task.Delay(100);

                                            if (DateTime.Now - start > TimeSpan.FromSeconds(1))
                                            {
                                                File.Delete(rightButtonPressed);
                                                storage.Remove(rightButtonPressed);
                                                Invoke(() => updateImages(null));
                                                break;
                                            }
                                        }
                                    });
                                    break;
                            }
                        };

                        pb.MouseUp += (_, e) =>
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                rightButtonPressed = null;
                                pb.Capture = false;
                            }
                        };
                    }

                    pb.Image = storage.GetAt(j).Bitmap;
                    pb.Location = new Point(x, 0);
                    pb.Size = new Size(sz, sz);
                    pb.Tag = storage.GetAt(j).FilePath;
                    pb.Visible = true;
                    pb.Text = storage.GetAt(j).FilePath;

                    x += sz + 10;
                    j++;
                    n++;
                }
            }

            while (x < splitContainer.Panel2.ClientSize.Width)
            {
                var pb = (PictureBox?)splitContainer.Panel2.Controls.Find("pic" + n, false).FirstOrDefault();
                if (pb != null) pb.Visible = false;
                x += sz + 10;
                n++;
            }

            splitContainer.Panel2.Refresh();
        }

        private void pictureBox_Paint(object? sender, PaintEventArgs e)
        {
            var cen = new Point(pictureBox.ClientSize.Width / 2, pictureBox.ClientSize.Height / 2);
            
            MaskHelper.DrawPrimitives(cen, e.Graphics, primPen, primBrush, primitives);

            if (cursorPt == null) return;

            var penSize = getActivePenSize();
            e.Graphics.FillEllipse
            (
                cursorBrush,
                cen.X + cursorPt.Value.X - penSize / 2,
                cen.Y + cursorPt.Value.Y - penSize / 2,
                penSize,
                penSize
            );
        }

        private void pictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            pictureBox.Capture = true;

            var loc = e.Location;
            loc.X -= pictureBox.ClientSize.Width / 2;
            loc.Y -= pictureBox.ClientSize.Height / 2;
        
            if (lastPrim != UNDO_DELIMITER) primitives.Add(UNDO_DELIMITER);

            primitives.Add(new Primitive
            {
                Kind = PrimitiveKind.Line,
                Pt0 = loc,
                Pt1 = loc,
                PenSize = getActivePenSize(),
            });

            pictureBox.Refresh();
        }

        private void pictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            var loc = e.Location;
            loc.X -= pictureBox.ClientSize.Width / 2;
            loc.Y -= pictureBox.ClientSize.Height / 2;

            if (pictureBox.Capture)
            {
                cursorPt = null;
                
                switch (lastPrim.Kind)
                {
                    case PrimitiveKind.Line:
                        lastPrim.Pt1 = loc;
                        primitives.Add(new Primitive
                        {
                            Kind = PrimitiveKind.Line,
                            Pt0 = loc,
                            Pt1 = loc,
                            PenSize = getActivePenSize(),
                        });
                        break;
                }
            }
            else
            {
                cursorPt = loc;
            }
           
            pictureBox.Refresh();
        }

        private void pictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            pictureBox.Capture = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == 0 && e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (!primitives.Any()) return;
                
                var i = primitives.FindLastIndex(x => x == UNDO_DELIMITER);
                redoPrimitiveBlocks.Add(primitives.GetRange(i, primitives.Count - i).ToArray());
                primitives = primitives.GetRange(0, i);

                pictureBox.Refresh();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Y || e.Modifiers == (Keys.Control|Keys.Shift) && e.KeyCode == Keys.Z)
            {
                if (!redoPrimitiveBlocks.Any()) return;

                var redoBlock = redoPrimitiveBlocks.Last();
                redoPrimitiveBlocks.RemoveAt(redoPrimitiveBlocks.Count - 1);
                primitives.AddRange(redoBlock);

                pictureBox.Refresh();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                btSave_Click(null, null);
            }
        }

        private Bitmap? getActiveImage()
        {
            if (pictureBox.Image == null) return null;

            var bmp = BitmapTools.Clone(pictureBox.Image)!;

            var cenX = VIEWPORT_WIDTH  / 2 - pictureBox.ViewportDeltaX;
            var cenY = VIEWPORT_HEIGHT / 2 - pictureBox.ViewportDeltaY;

            MaskHelper.DrawAlpha(new Point(cenX, cenY), bmp, primitives);

            return bmp;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            var destPath = filePath.EndsWith("-transparent.png") 
                               ? filePath
                               : Path.Combine(Path.GetDirectoryName(filePath)!, Path.GetFileNameWithoutExtension(filePath)) + "-transparent.png";
            
            getActiveImage().Save(destPath, ImageFormat.Png);
        }

        private void btDeAlpha_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            BitmapTools.DeAlpha(pictureBox.Image);
            pictureBox.Refresh();
        }

        private void btLoad_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                pictureBox.Image = BitmapTools.Load(filePath);
                primitives.Clear();
                redoPrimitiveBlocks.Clear();
                pictureBox.Refresh();
            }
        }

        private void btPen1_Click(object sender, EventArgs e)
        {
            btPen1.Checked = true;
            btPen2.Checked = false;
        }

        private void btPen2_Click(object sender, EventArgs e)
        {
            btPen1.Checked = false;
            btPen2.Checked = true;
        }

        private int getActivePenSize()
        {
            return btPen1.Checked ? PEN_SIZE : PEN_SIZE * 4;
        }

        private void pictureBox_MouseEnter(object? sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void pictureBox_MouseLeave(object? sender, EventArgs e)
        {
            cursorPt = null;
            pictureBox.Refresh();
            Cursor.Show();
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            updateImages(null);
        }

        private bool generating;

        private void btInvokeAiGenerate_Click(object sender, EventArgs e)
        {
            if (generating)
            {
                btInvokeAiGenerate.Enabled = false;
                InvokeAi.Cancel();
                return;
            }

            if (tbPrompt.Text.Trim() == "")
            {
                tbPrompt.Focus();
                return;
            }

            generating = true;

            var dx = pictureBox.ViewportDeltaX;
            var dy = pictureBox.ViewportDeltaY;

            var activeImage = cbInvokeAiUseInitImage.Checked ? getActiveImage() : null;
            var croppedImage = activeImage is { Width: <= VIEWPORT_WIDTH, Height: <= VIEWPORT_HEIGHT }
                                         ? activeImage
                                         : BitmapTools.GetCropped
                                         (
                                             activeImage, 
                                             -dx, 
                                             -dy,
                                             VIEWPORT_WIDTH, 
                                             VIEWPORT_HEIGHT
                                         );

            var sdImage = new AiImageInfo
            {
                prompt = tbPrompt.Text.Trim() != "" ? tbPrompt.Text.Trim() : null,
                cfg_scale = numInvokeAiCfgScale.Value,
                gfpgan_strength = numInvokeAiGfpGan.Value,
                iterations = (int)numInvokeAiIterations.Value,
                seed = tbInvokeAiSeed.Text.Trim() == "" ? -1 : long.Parse(tbInvokeAiSeed.Text.Trim()),
                steps = (int)numInvokeAiSteps.Value,
                strength = numInvokeAiImg2img.Value,
                initimg = BitmapTools.GetBase64String(croppedImage),
            };

            var oldGenerateText = btInvokeAiGenerate.Text;
            btInvokeAiGenerate.Text = "CANCEL";
            tbPrompt.Enabled = false;
            
            pbInvokeAiIterations.Maximum = sdImage.iterations;
            pbInvokeAiIterations.Value = 0;

            pbInvokeAiSteps.Value = 0;
            pbInvokeAiSteps.Maximum = sdImage.steps;

            InvokeAi.Generate(sdImage, progress =>
            {
                Invoke(() =>
                {
                    switch (progress.@event)
                    {
                        case "step":
                            pbInvokeAiSteps.Value = progress.step ?? 0;
                            break;                    
                    
                        case "result":
                            pbInvokeAiSteps.Value = 0;
                            pbInvokeAiIterations.Value++;
                            if (pbInvokeAiIterations.Value == pbInvokeAiIterations.Maximum)
                            {
                                btInvokeAiGenerate.Text = oldGenerateText;
                                btInvokeAiGenerate.Enabled = true;
                                tbPrompt.Enabled = true;

                                var fName = progress.url.Split('/', '\\').Last();
                                var fPath = Path.Combine(Program.Config.InvokeAiOutputFolderPath, fName);

                                _ = Task.Run(async () =>
                                {
                                    while (!File.Exists(fPath)) await Task.Delay(500);
                                    await Task.Delay(1000);
                                    var bmp = BitmapTools.Load(fPath)!;
                                    if (activeImage != null && activeImage != croppedImage)
                                    {
                                        using var g = Graphics.FromImage(activeImage);
                                        g.DrawImageUnscaled(bmp, -dx, -dy);
                                        activeImage.Save(fPath, ImageFormat.Png);
                                    }
                                    generating = false;
                                });
                            }
                            break;

                        case "canceled":
                            pbInvokeAiSteps.Value = 0;
                            btInvokeAiGenerate.Text = oldGenerateText;
                            btInvokeAiGenerate.Enabled = true;
                            tbPrompt.Enabled = true;
                            generating = false;
                            break;
                    }
                });
            });
        }

        private void btClearActiveImage_Click(object sender, EventArgs e)
        {
            filePath = null;
            pictureBox.Image = null;
        }

        private void btInvokeAiReset_Click(object sender, EventArgs e)
        {
            var sdImage = new AiImageInfo();
            numInvokeAiImg2img.Value = sdImage.strength;
            numInvokeAiCfgScale.Value = sdImage.cfg_scale;
            numInvokeAiGfpGan.Value = sdImage.gfpgan_strength;
            numInvokeAiSteps.Value = sdImage.steps;
        }

        private void btLeft_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            if (pictureBox.ViewportDeltaX < 0) pictureBox.ViewportDeltaX += MOVE_SIZE;
            else
            {
                var bmp = new Bitmap(pictureBox.Image.Width + MOVE_SIZE, pictureBox.Image.Height, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Transparent, 0, 0, MOVE_SIZE, bmp.Height);
                    g.DrawImageUnscaled(pictureBox.Image, MOVE_SIZE, 0);
                }

                pictureBox.Image = bmp;
            }

            pictureBox.Refresh();
        }

        private void btRight_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            pictureBox.ViewportDeltaX -= MOVE_SIZE;

            if (pictureBox.ViewportDeltaX + pictureBox.Image.Width < VIEWPORT_WIDTH)
            {
                var sz = VIEWPORT_WIDTH - (pictureBox.ViewportDeltaX + pictureBox.Image.Width);

                var bmp = new Bitmap(pictureBox.Image.Width + sz, pictureBox.Image.Height, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Transparent, bmp.Width - sz, 0, sz, bmp.Height);
                    g.DrawImageUnscaled(pictureBox.Image, 0, 0);
                }

                pictureBox.Image = bmp;
                
            }
            
            pictureBox.Refresh();
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            if (pictureBox.ViewportDeltaY < 0) pictureBox.ViewportDeltaY += MOVE_SIZE;
            else
            {
                var bmp = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height + MOVE_SIZE, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Transparent, 0, 0, bmp.Width, MOVE_SIZE);
                    g.DrawImageUnscaled(pictureBox.Image, 0, MOVE_SIZE);
                }

                pictureBox.Image = bmp;
            }

            pictureBox.Refresh();
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            pictureBox.ViewportDeltaY -= MOVE_SIZE;

            if (pictureBox.ViewportDeltaY + pictureBox.Image.Height < VIEWPORT_HEIGHT)
            {
                var sz = VIEWPORT_HEIGHT - (pictureBox.ViewportDeltaY + pictureBox.Image.Height);

                var bmp = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height + sz, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(Brushes.Transparent, 0, bmp.Height - sz, bmp.Width, sz);
                    g.DrawImageUnscaled(pictureBox.Image, 0, 0);
                }

                pictureBox.Image = bmp;
                
            }
            
            pictureBox.Refresh();
        }

        private void btResetMask_Click(object sender, EventArgs e)
        {
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            pictureBox.Refresh();
        }

        private void btApplyAlphaMask_Click(object sender, EventArgs e)
        {
            pictureBox.Image = getActiveImage();
            primitives.Clear();
            redoPrimitiveBlocks.Clear();
            Refresh();
        }

        private void btLamaCleanerInpaint_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            btLamaCleanerInpaint.Enabled = false;
            Task.Run(() =>
            {
                var result = LamaCleaner.RunAsync(getActiveImage()).Result;
                Invoke(() =>
                {
                    btLamaCleanerInpaint.Enabled = true;
                    pictureBox.Image = result;
                    primitives.Clear();
                    redoPrimitiveBlocks.Clear();
                    Refresh();
                });
            });
        }

        private void btRemBgRemoveBackground_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            btRemBgRemoveBackground.Enabled = false;
            Task.Run(() =>
            {
                var result = RemBg.RunAsync(getActiveImage()).Result;
                Invoke(() =>
                {
                    btRemBgRemoveBackground.Enabled = true;
                    pictureBox.Image = result;
                    primitives.Clear();
                    redoPrimitiveBlocks.Clear();
                    Refresh();
                });
            });
        }
    }
}