using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using AiPainter.Helpers;

#pragma warning disable CS8602

namespace AiPainter
{
    public partial class MainForm : Form
    {
        private const int IMAGE_EXTEND_SIZE = 64;
        
        private string? filePath;

        private static readonly StoredImageList storedImageList = new();
        
        private string? rightButtonPressed;

        private bool invokeaiIsPortOpen;
        private bool lamaCleanerIsPortOpen;
        private bool remBgIsPortOpen;
        
        public MainForm()
        {
            InitializeComponent();

            checkPortsWorker.RunWorkerAsync();
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
                    bool changesDetected;
                    lock (storedImageList)
                    {
                        changesDetected = storedImageList.Update();
                    }
                    if (changesDetected)
                    {
                        Invoke(() =>
                        {
                            updateImages(null);
                        });
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
                                                lock (storedImageList)
                                                {
                                                    storedImageList.Remove(rightButtonPressed);
                                                }
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
                    toolTip.SetToolTip(pb, Path.GetFileName(pb.Text) + " (" + Path.GetDirectoryName(pb.Text) + ")\n\nHold right mouse button to remove file from disk.");

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
            var image = pictureBox.Image;
            if (image == null) return;

            format ??= BitmapTools.HasAlpha(image) || Path.GetExtension(filePath).ToLowerInvariant() == ".png"
                           ? ImageFormat.Png
                           : ImageFormat.Jpeg;

            var baseFileName = Path.GetFileNameWithoutExtension(filePath)!;
            var match = Regex.Match(baseFileName, @"(.+)-aip_(\d+)$");
            var n = match.Success ? int.Parse(match.Groups[2].Value) + 1 : 1;
            if (match.Success) baseFileName = match.Groups[1].Value;
            baseFileName += "-aip_";
            
            var baseDir = Path.GetDirectoryName(filePath)!;
  
            while (Directory.GetFiles(baseDir, baseFileName + n.ToString("D3") + ".*").Any()) n++;
            
            image.Save(Path.Join(baseDir, baseFileName) + n.ToString("D3") + (Equals(format, ImageFormat.Png) ? ".png" : ".jpg"), format);
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
                using var image = BitmapTools.Load(filePath);
                pictureBox.Image = BitmapTools.GetShrinked
                (
                    image,
                    Program.Config.ShrinkImageOnOpenMaxWidth,
                    Program.Config.ShrinkImageOnOpenMaxHeight
                );
                pictureBox.ResetMask();
                pictureBox.ResetView();
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            updateImages(null);
        }

        private void btClearActiveImage_Click(object sender, EventArgs e)
        {
            filePath = null;
            pictureBox.Image = null;
            pictureBox.Refresh();
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
            pictureBox.ShiftMask(IMAGE_EXTEND_SIZE, 0);
            pictureBox.AddBoxToMask(0, 0, IMAGE_EXTEND_SIZE, pictureBox.Image.Height);

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
            pictureBox.AddBoxToMask(bmp.Width - IMAGE_EXTEND_SIZE, 0 , IMAGE_EXTEND_SIZE, pictureBox.Image.Height);
            pictureBox.ActiveBox.X += IMAGE_EXTEND_SIZE;
                
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
            pictureBox.ShiftMask(0, IMAGE_EXTEND_SIZE);
            pictureBox.AddBoxToMask(0, 0, pictureBox.Image.Width, IMAGE_EXTEND_SIZE);

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
            pictureBox.AddBoxToMask(0, bmp.Height - IMAGE_EXTEND_SIZE, pictureBox.Image.Width, IMAGE_EXTEND_SIZE);
            pictureBox.ActiveBox.Y += IMAGE_EXTEND_SIZE;
                
            pictureBox.Refresh();
        }

        private void btResetMask_Click(object sender, EventArgs e)
        {
            pictureBox.ResetMask();
            pictureBox.Refresh();
        }

        private void controlsStateUpdater_Tick(object sender, EventArgs e)
        {
            panInvokeAi.UpdateState(pictureBox, invokeaiIsPortOpen);
            panLamaCleaner.UpdateState(pictureBox, lamaCleanerIsPortOpen);
            panRemBg.UpdateState(pictureBox, remBgIsPortOpen);

            pictureBox.Enabled = !panLamaCleaner.InProcess && !panRemBg.InProcess;

            var activeBox = pictureBox.ActiveBox;

            Text = (string.IsNullOrEmpty(filePath) ? "AiPainter" : Path.GetFileName(filePath))
                 + (pictureBox.Image == null ? "" : " (" + pictureBox.Image.Width + " x " + pictureBox.Image.Height + ")")
                 + $" [Active box: X,Y = {activeBox.X},{activeBox.Y}; WxH = {activeBox.Width}x{activeBox.Height}]"
                 + (string.IsNullOrEmpty(filePath) ? "" : " | " + Path.GetDirectoryName(filePath));

            btClearActiveImage.Enabled = pictureBox.Image != null;
            btCopyToClipboard.Enabled = pictureBox.Image != null;
            btSavePng.Enabled = pictureBox.Image != null;
            btSaveJpeg.Enabled = pictureBox.Image != null;
            btResetMask.Enabled = pictureBox.HasMask;
            btDeAlpha.Enabled = pictureBox.Image != null && BitmapTools.HasAlpha(pictureBox.Image);
            btRestorePrevMask.Enabled = pictureBox.HasPrevMask;
            btResizeAndMoveActiveBoxToFitImage.Enabled = pictureBox.Image != null;

            btLeft.Enabled = pictureBox.Image != null;
            btUp.Enabled = pictureBox.Image != null;
            btDown.Enabled = pictureBox.Image != null;
            btRight.Enabled = pictureBox.Image != null;
        }

        private void checkPortsWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                invokeaiIsPortOpen = true;//ProcessHelper.IsPortOpen(Program.Config.InvokeAiUrl);
                lamaCleanerIsPortOpen = true;//ProcessHelper.IsPortOpen(Program.Config.LamaCleanerUrl);
                remBgIsPortOpen = true;//ProcessHelper.IsPortOpen(Program.Config.RemBgUrl);
                Thread.Sleep(200);
            }
        }

        private void btRestorePrevMask_Click(object sender, EventArgs e)
        {
            pictureBox.RestorePreviousMask();
        }

        private void btAbout_Click(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
        }

        private void btCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox.Image!);
        }

        private void btResizeAndMoveActiveBoxToFitImage_Click(object sender, EventArgs e)
        {
            var sz = Math.Max(pictureBox.Image.Width, pictureBox.Image.Height);
            pictureBox.ActiveBox = new Rectangle
            (
                (pictureBox.Image.Width - sz) >> 1,
                (pictureBox.Image.Height - sz) >> 1,
                sz,
                sz
            );
            pictureBox.Refresh();
        }
    }
}