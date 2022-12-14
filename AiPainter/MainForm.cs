using System.Drawing.Imaging;
using System.Text.Json;
using AiPainter.Controls;
using AiPainter.Helpers;

#pragma warning disable CS8602

namespace AiPainter
{
    public partial class MainForm : Form
    {
        private const int IMAGE_EXTEND_SIZE = 64;

        private string? filePath;
        public string? FilePath 
        { 
            get => filePath;
            set
            {
                filePath = value;

                var folder = filePath != null ? (Path.GetDirectoryName(filePath) ?? Program.Config.OutputFolder) : Program.Config.OutputFolder;
                if (storedImageList.Folder != folder)
                {
                    storedImageList = new(folder);
                }
            }
        }

        private static StoredImageList storedImageList = new(Program.Config.OutputFolder);

        public MainForm()
        {
            InitializeComponent();

            panStableDiffusion.OnGenerate = () =>
            {
                panGenerationList.AddGeneration(panStableDiffusion, pictureBox, this);
            };

            updateImageListWorker.RunWorkerAsync();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            hPicScroll.Minimum = 0;
            hPicScroll.SmallChange = 1;
            hPicScroll.Scroll += (_, ee) => updateImages(ee.NewValue);

            splitContainer.Panel2.MouseWheel += (_, ee) =>
            {
                hPicScroll.Value = Math.Max(hPicScroll.Minimum, Math.Min(hPicScroll.Maximum - hPicScroll.LargeChange + 1, hPicScroll.Value + (ee.Delta > 0 ? -1 : 1)));
                updateImages(null);
            };

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                if (File.Exists(args[1]))
                {
                    FilePath = args[1];
                    pictureBox.Image = BitmapTools.Load(FilePath);
                }
            }
        }

        private void updateImageListWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!IsDisposed)
            {
                var changesDetected = false;
                try
                {
                    lock (storedImageList)
                    {
                        changesDetected = storedImageList.Update();
                    }
                }
                catch (Exception ee)
                {
                    Program.Log.WriteLine(ee.ToString());
                    DelayTools.WaitForExit(1000);
                }
                
                if (changesDetected)
                {
                    try
                    {
                        Invoke(() =>
                        {
                            lock (storedImageList)
                            {
                                if (hPicScroll.Value == Math.Max(0, hPicScroll.Maximum - hPicScroll.LargeChange + 1))
                                {
                                    hPicScroll.Maximum = Math.Max(0, storedImageList.Count - 1);
                                    hPicScroll.Value = Math.Max(0, hPicScroll.Maximum - hPicScroll.LargeChange + 1);
                                }
                                updateImages(null);
                            }
                        });
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }

                DelayTools.WaitForExit(1000);
            }
        }
        
        private void updateImages(int? hPicScrollValue)
        {
            var panel = splitContainer.Panel2;

            var sz = Math.Max(50, panel.ClientSize.Height);
            var x = 0;
            var n = 0;
            
            lock (storedImageList)
            {
                hPicScroll.LargeChange = (panel.ClientSize.Width + 5) / (sz + 5);
                hPicScroll.Maximum = Math.Max(0, storedImageList.Count - 1);
                hPicScroll.Value = Math.Max(0, Math.Min(hPicScroll.Value, hPicScroll.Maximum - hPicScroll.LargeChange + 1));
                if (hPicScrollValue != null) hPicScrollValue = Math.Max(0, Math.Min(hPicScrollValue.Value, hPicScroll.Maximum - hPicScroll.LargeChange + 1));

                var i = hPicScrollValue ?? hPicScroll.Value;
                while (x < panel.ClientSize.Width && i < storedImageList.Count)
                {
                    var pb = (SmartImagePreview?)panel.Controls.Find("pic" + n, false).SingleOrDefault() ?? createSmartImagePreview(n);

                    pb.Image = storedImageList.GetAt(i).Bitmap!;
                    pb.Location = new Point(x, 0);
                    pb.Size = new Size(sz, sz);
                    pb.FilePath = storedImageList.GetAt(i).FilePath;
                    pb.Visible = true;
                    toolTip.SetToolTip(pb.PictureBox, Path.GetFileName(pb.FilePath) + " (" + Path.GetDirectoryName(pb.FilePath) + ")\n\nClick to load image. Double click to load also original parameters from *.json file.");

                    x += sz + 5;
                    i++;
                    n++;
                }
            }

            while (x < panel.ClientSize.Width)
            {
                var pb = (SmartImagePreview?)panel.Controls.Find("pic" + n, false).FirstOrDefault();
                if (pb != null) pb.Visible = false;
                x += sz + 5;
                n++;
            }

            panel.Refresh();
        }

        private SmartImagePreview createSmartImagePreview(int n)
        {
            var pb = new SmartImagePreview();
            pb.Name = "pic" + n;
            pb.Parent = splitContainer.Panel2;

            pb.OnImageClick = () =>
            {
                FilePath = pb.FilePath;
                pictureBox.Image = BitmapTools.Load(pb.FilePath);
                pictureBox.ResetMask();
                pictureBox.HistoryClear();
            };

            pb.OnImageDoubleClick = () =>
            {
                var parametersJsonFilePath = Path.Join(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) + ".json";
                if (File.Exists(parametersJsonFilePath))
                {
                    var sdGenerationParameters = JsonSerializer.Deserialize<SdGenerationParameters>(File.ReadAllText(parametersJsonFilePath));

                    panStableDiffusion.numSteps.Value = sdGenerationParameters.steps;
                    panStableDiffusion.tbPrompt.Text = sdGenerationParameters.prompt;
                    panStableDiffusion.tbNegative.Text = sdGenerationParameters.negative;
                    panStableDiffusion.numCfgScale.Value = sdGenerationParameters.cfgScale;
                    panStableDiffusion.tbSeed.Text = sdGenerationParameters.seed.ToString();
                    panStableDiffusion.Modifiers = sdGenerationParameters.modifiers;
                }
            };

            pb.OnImageRemove = () =>
            {
                lock (storedImageList)
                {
                    File.Delete(pb.FilePath);
                    storedImageList.Remove(pb.FilePath);

                    var jsonFilePath = Path.Join(Path.GetDirectoryName(pb.FilePath), Path.GetFileNameWithoutExtension(pb.FilePath)) + ".json";
                    if (File.Exists(jsonFilePath)) File.Delete(jsonFilePath);

                    updateImages(null);
                }
            };
            
            return pb;
        }

        protected override bool ProcessKeyPreview(ref Message msg)
        {
            pictureBox?.ProcessKeys(msg);
            return base.ProcessKeyPreview(ref msg);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var focusInText = panStableDiffusion.IsTextboxInFocus;

            if (!focusInText && keyData == (Keys.Control | Keys.Z))
            {
                pictureBox.HistoryUndo();
                return true;
            }

            if (!focusInText && keyData == (Keys.Control | Keys.Y) || keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                pictureBox.HistoryRedo();
                return true;
            }

            if (keyData == (Keys.Control | Keys.S))
            {
                save();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void save()
        {
            if (pictureBox.Image == null) return;

            if (Path.GetExtension(FilePath).ToLowerInvariant() != ".png" && BitmapTools.HasAlpha(pictureBox.Image))
            {
                var r = MessageBox.Show
                (
                    this,
                    "Image has transparent areas. Do you want to save it as *.png?",
                    "Warning",
                    MessageBoxButtons.YesNoCancel
                );
                if (r == DialogResult.Cancel) return;
                if (r == DialogResult.Yes)
                {
                    FilePath = Path.Join(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) + ".png";
                    save();
                    return;
                }
            }

            var format = Path.GetExtension(FilePath).ToLowerInvariant() == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
            pictureBox.Image.Save(FilePath!, format);
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
                FilePath = openFileDialog.FileName;
                pictureBox.Image = BitmapTools.Load(FilePath);
                pictureBox.ResetMask();
                pictureBox.ZoomAndMoveGlobalViewToFitImage();
                pictureBox.HistoryClear();
                pictureBox.Refresh();
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            pictureBox.Refresh();
            updateImages(null);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            pictureBox.Refresh();
        }

        private void btClearActiveImage_Click(object sender, EventArgs e)
        {
            FilePath = null;
            pictureBox.Image = null;
            pictureBox.HistoryClear();
            pictureBox.Refresh();
        }

        private void btLeft_Click(object sender, EventArgs e)
        {
            pictureBox.HistoryAddCurrentState();

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
            pictureBox.HistoryAddCurrentState();
            
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
            pictureBox.HistoryAddCurrentState();
            
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
            pictureBox.HistoryAddCurrentState();
            
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
            pictureBox.HistoryAddCurrentState(); // TODO: ???
            
            pictureBox.ResetMask();
            pictureBox.Refresh();
        }

        private void controlsStateUpdater_Tick(object sender, EventArgs e)
        {
            panStableDiffusion.UpdateState(pictureBox);
            panLamaCleaner.UpdateState(pictureBox);
            panRemBg.UpdateState(pictureBox);

            pictureBox.Enabled = !panLamaCleaner.InProcess && !panRemBg.InProcess;

            var activeBox = pictureBox.ActiveBox;

            Text = (string.IsNullOrEmpty(FilePath) ? "AiPainter" : Path.GetFileName(FilePath))
                 + (pictureBox.Image == null ? "" : " (" + pictureBox.Image.Width + " x " + pictureBox.Image.Height + ")")
                 + $" [Active box: X,Y = {activeBox.X},{activeBox.Y}; WxH = {activeBox.Width}x{activeBox.Height}]"
                 + (string.IsNullOrEmpty(FilePath) ? "" : " | " + Path.GetDirectoryName(FilePath));

            btClearActiveImage.Enabled = pictureBox.Image != null && pictureBox.Enabled;
            btCopyToClipboard.Enabled = pictureBox.Image != null && pictureBox.Enabled;
            btResetMask.Enabled = pictureBox.HasMask && pictureBox.Enabled;
            btDeAlpha.Enabled = pictureBox.Image != null && BitmapTools.HasAlpha(pictureBox.Image) && pictureBox.Enabled;
            btRestorePrevMask.Enabled = pictureBox.HasPrevMask && pictureBox.Enabled;
            btResizeAndMoveActiveBoxToFitImage.Enabled = pictureBox.Image != null && pictureBox.Enabled;

            btSave.Enabled = !string.IsNullOrEmpty(FilePath) && pictureBox.Image != null && pictureBox.Enabled;
            btSaveAs.Enabled = pictureBox.Image != null && pictureBox.Enabled;

            btLeft.Enabled = pictureBox.Image != null && pictureBox.Enabled;
            btUp.Enabled = pictureBox.Image != null && pictureBox.Enabled;
            btDown.Enabled = pictureBox.Image != null && pictureBox.Enabled;
            btRight.Enabled = pictureBox.Image != null && pictureBox.Enabled;

            sbResize.Enabled = pictureBox.Image != null && pictureBox.Enabled;
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
            pictureBox.HistoryAddCurrentState();

            pictureBox.ResizeAndMoveActiveBoxToFitImage();
            pictureBox.ZoomAndMoveGlobalViewToFitImage();
            pictureBox.Refresh();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void btSaveAs_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            
            saveFileDialog.Filter = 
                  "PNG file (*.png)|*.png"
                + "|JPG file (*.jpg)|*.jpg;*.jpeg"
                + "|All files (*.*)|*.*";
            
            saveFileDialog.FileName = Path.GetFileName(FilePath);
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = saveFileDialog.FileName;
                save();
            }
        }

        private void splitContainer_Panel2_Resize(object sender, EventArgs e)
        {
            updateImages(null);
        }

        private void resizeTo2048ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeImage(2048);
        }

        private void resizeTo1024ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeImage(1024);
        }

        private void resizeTo512ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeImage(512);
        }

        private void resizeImage(int size)
        {
            pictureBox.HistoryAddCurrentState();

            var image = pictureBox.Image!;
            var k = Math.Min((double)size / image.Width, (double)size / image.Height);
            pictureBox.Image = BitmapTools.GetResized(image, (int)Math.Round(image.Width * k), (int)Math.Round(image.Height * k));
        }
    }
}