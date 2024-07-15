using System.Drawing.Imaging;
using AiPainter.Adapters.LamaCleaner;
using AiPainter.Helpers;

#pragma warning disable CS8602

namespace AiPainter
{
    public partial class MainForm : Form
    {
        private const int IMAGE_EXTEND_SIZE = 64;

        public string? FilePath;

        public string ImagesFolder
        {
            get => panImages.ImagesFolder;
            set => panImages.ImagesFolder = value;
        }

        private readonly LamaCleanerManager lamaCleaner = new();

        public MainForm()
        {
            InitializeComponent();

            panImages.mainForm = this;
            panStableDiffusion.mainForm = this;

            updateImageListWorker.RunWorkerAsync();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                if (File.Exists(args[1]))
                {
                    var filePath = Path.GetFullPath(args[1]);
                    OpenImageFile(filePath);
                    ImagesFolder = Path.GetDirectoryName(filePath)!;
                }
            }

            sbResize.DropDownItems.Clear();
            foreach (var size in Program.Config.ImageSizes.Select(x => x.Split("x").First()).Distinct().Select(x => int.Parse(x)).OrderBy(x => x))
            {
                sbResize.DropDownItems.Add("Resize image to " + size, null, (_, _) =>
                {
                    resizeImage(size);
                });
            }
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
                OpenImageFile(openFileDialog.FileName);
                ImagesFolder = Path.GetDirectoryName(FilePath)!;
            }
        }

        public void OpenImageFile(string filePath, bool autoZoom = true)
        {
            FilePath = filePath;
            pictureBox.Image = BitmapTools.Load(FilePath);
            pictureBox.ResetMask();
            if (autoZoom) pictureBox.ZoomAndMoveGlobalViewToFitImage();
            pictureBox.HistoryClear();
            pictureBox.Refresh();
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            pictureBox.Refresh();
            panImages.updateImages(null);
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
            pictureBox.AddBoxToMask(bmp.Width - IMAGE_EXTEND_SIZE, 0, IMAGE_EXTEND_SIZE, pictureBox.Image.Height);
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
            pictureBox.HistoryAddCurrentState();
            pictureBox.ResetMask();
            pictureBox.Refresh();
        }

        private void controlsStateUpdater_Tick(object sender, EventArgs e)
        {
            panStableDiffusion.UpdateState();
            lamaCleaner.UpdateState(pictureBox, btRemoveObjectFromImage);

            pictureBox.Enabled = !lamaCleaner.InProcess;

            var activeBox = pictureBox.ActiveBox;

            Text = (string.IsNullOrEmpty(FilePath) ? "AiPainter" : Path.GetFileName(FilePath))
                 + (pictureBox.Image == null ? "" : " (" + pictureBox.Image.Width + " x " + pictureBox.Image.Height + ")")
                 + $" [Active box: X,Y = {activeBox.X},{activeBox.Y}; WxH = {activeBox.Width}x{activeBox.Height}]"
                 + " | " + Path.GetFullPath(ImagesFolder);

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

            btUpscale.Enabled = pictureBox.Image != null;
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
            panImages.updateImages(null);
        }

        private void resizeImage(int size)
        {
            pictureBox.HistoryAddCurrentState();

            var image = pictureBox.Image!;
            var k = Math.Min((double)size / image.Width, (double)size / image.Height);
            pictureBox.Image = BitmapTools.GetResized(image, (int)Math.Round(image.Width * k), (int)Math.Round(image.Height * k));
        }

        private void btRemoveObjectFromImage_Click(object sender, EventArgs e)
        {
            lamaCleaner.Run(pictureBox, image =>
            {
                Invoke(() =>
                {
                    pictureBox.Image = image;
                    pictureBox.ResetMask();
                    pictureBox.Refresh();
                });
            });
        }

        private void btUpscaleCommon2x_Click(object sender, EventArgs e)
        {
            upscale("R-ESRGAN 4x+", 2);
        }

        private void btUpscaleAnime2x_Click(object sender, EventArgs e)
        {
            upscale("R-ESRGAN 4x+ Anime6B", 2);
        }

        private void btUpscaleCommon4x_Click(object sender, EventArgs e)
        {
            upscale("R-ESRGAN 4x+", 4);
        }

        private void btUpscaleAnime4x_Click(object sender, EventArgs e)
        {
            upscale("R-ESRGAN 4x+ Anime6B", 4);
        }

        private void upscale(string upscaler, int resizeFactor)
        {
            var form = new UpscaleForm
            (
                panGenerationList,
                pictureBox.Image!,
                upscaler,
                resizeFactor,
                Path.Combine(Path.GetDirectoryName(FilePath)!, Path.GetFileNameWithoutExtension(FilePath) + "-upscaled") + ".png"
            );
            
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                OpenImageFile(form.ResultFilePath);
            }
        }
    }
}