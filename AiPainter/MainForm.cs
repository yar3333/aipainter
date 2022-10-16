using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using AiPainter.Adapters;
using AiPainter.Adapters.StuffForInvokeAi;
using AiPainter.Helpers;

#pragma warning disable CS8602

namespace AiPainter
{
    public partial class MainForm : Form
    {
        private const int IMAGE_EXTEND_SIZE = 64;
        
        private string? _filePath;

        private string? filePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                Text = "AiPainter" + (string.IsNullOrEmpty(value) ? "" : " - " + value);
            }
        }

        private static readonly StoredImageList storedImageList = new();
        
        private string? rightButtonPressed;

        public MainForm()
        {
            InitializeComponent();
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
                        lock (storedImageList)
                        {
                            changesDetected = storedImageList.Update();
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
        
        private void updateImages(int? hPicScrollValue)
        {
            var sz = Math.Max(50, splitContainer.Panel2.ClientSize.Height);
            var x = 0;
            var n = 0;
            
            lock (storedImageList)
            {
                hPicScroll.LargeChange = Math.Max(1, splitContainer.Panel2.ClientSize.Width / (sz + 10) - 1);
                hPicScroll.Maximum = Math.Max(0, storedImageList.Count - 2);

                var j = hPicScrollValue ?? hPicScroll.Value;
                while (x < splitContainer.Panel2.ClientSize.Width && j < storedImageList.Count)
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
                                    pictureBox.ResetMask();
                                    pictureBox.ResetView();
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
                                                storedImageList.Remove(rightButtonPressed);
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

                    pb.Image = storedImageList.GetAt(j).Bitmap;
                    pb.Location = new Point(x, 0);
                    pb.Size = new Size(sz, sz);
                    pb.Tag = storedImageList.GetAt(j).FilePath;
                    pb.Visible = true;
                    pb.Text = storedImageList.GetAt(j).FilePath;

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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == 0 && e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                pictureBox.Undo();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Y || e.Modifiers == (Keys.Control|Keys.Shift) && e.KeyCode == Keys.Z)
            {
                pictureBox.Redo();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                save(null);
            }
        }

        private void save(ImageFormat? format)
        {
            var image = pictureBox.GetImageWithMaskToTransparent();
            if (image == null) return;

            format ??= BitmapTools.HasAlpha(image) || Path.GetExtension(filePath).ToLowerInvariant() == ".png"
                           ? ImageFormat.Png
                           : ImageFormat.Jpeg;

            var baseFileName = Path.GetFileNameWithoutExtension(filePath)!;
            var match = Regex.Match(baseFileName, @"(.+)-aip_(\d+)$");
            var n = match.Success ? int.Parse(match.Groups[1].Value) + 1 : 1;
            if (match.Success) baseFileName = match.Groups[0].Value;
            baseFileName += "-aip_";
            
            var baseDir = Path.GetDirectoryName(filePath)!;
  
            while (Directory.GetFiles(baseDir, baseFileName + n.ToString("D3") + ".*").Any()) n++;
            
            image.Save(Path.Join(baseDir, baseFileName) + (Equals(format, ImageFormat.Png) ? ".png" : ".jpg"), format);
        }

        private void btSavePng_Click(object sender, EventArgs e)
        {
            save(ImageFormat.Png);
        }

        private void btSaveJpeg_Click(object sender, EventArgs e)
        {
            save(ImageFormat.Jpeg);
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
                pictureBox.ResetMask();
                pictureBox.ResetView();
            }
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

            var dx = pictureBox.ViewDeltaX;
            var dy = pictureBox.ViewDeltaY;

            var wasCropped = false;

            var activeImage = cbInvokeAiUseInitImage.Checked ? pictureBox.GetImageWithMaskToTransparent() : null;
            var croppedImage = activeImage != null
                                   ? pictureBox.GetImageWithMaskToTransparentCroppedToViewport(out wasCropped)
                                   : null;

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
                                    if (wasCropped)
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

            var bmp = new Bitmap(pictureBox.Image.Width + IMAGE_EXTEND_SIZE, pictureBox.Image.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Transparent, 0, 0, IMAGE_EXTEND_SIZE, bmp.Height);
                g.DrawImageUnscaled(pictureBox.Image, IMAGE_EXTEND_SIZE, 0);
            }

            pictureBox.Image = bmp;

            pictureBox.Refresh();
        }

        private void btRight_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            var bmp = new Bitmap(pictureBox.Image.Width + IMAGE_EXTEND_SIZE, pictureBox.Image.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Transparent, bmp.Width - IMAGE_EXTEND_SIZE, 0, IMAGE_EXTEND_SIZE, bmp.Height);
                g.DrawImageUnscaled(pictureBox.Image, 0, 0);
            }

            pictureBox.Image = bmp;
                
            pictureBox.Refresh();
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height + IMAGE_EXTEND_SIZE, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Transparent, 0, 0, bmp.Width, IMAGE_EXTEND_SIZE);
                g.DrawImageUnscaled(pictureBox.Image, 0, IMAGE_EXTEND_SIZE);
            }

            pictureBox.Image = bmp;

            pictureBox.Refresh();
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            
            var bmp = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height + IMAGE_EXTEND_SIZE, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Transparent, 0, bmp.Height - IMAGE_EXTEND_SIZE, bmp.Width, IMAGE_EXTEND_SIZE);
                g.DrawImageUnscaled(pictureBox.Image, 0, 0);
            }

            pictureBox.Image = bmp;
                
            pictureBox.Refresh();
        }

        private void btResetMask_Click(object sender, EventArgs e)
        {
            pictureBox.ResetMask();
        }

        private void btApplyAlphaMask_Click(object sender, EventArgs e)
        {
            pictureBox.Image = pictureBox.GetImageWithMaskToTransparent();
            pictureBox.ResetMask();
        }

        private void btLamaCleanerInpaint_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            btLamaCleanerInpaint.Enabled = false;
            Task.Run(() =>
            {
                var result = LamaCleaner.RunAsync(pictureBox.GetImageWithMaskToTransparent()).Result;
                Invoke(() =>
                {
                    btLamaCleanerInpaint.Enabled = true;
                    pictureBox.Image = result;
                    pictureBox.ResetMask();
                });
            });
        }

        private void btRemBgRemoveBackground_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            btRemBgRemoveBackground.Enabled = false;
            Task.Run(() =>
            {
                var result = RemBg.RunAsync(pictureBox.GetImageWithMaskToTransparent()).Result;
                Invoke(() =>
                {
                    btRemBgRemoveBackground.Enabled = true;
                    pictureBox.Image = result;
                    pictureBox.ResetMask();
                });
            });
        }

        private void controlsStateUpdater_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }
    }
}