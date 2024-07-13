using AiPainter.Adapters.StableDiffusion;
using AiPainter.Helpers;

namespace AiPainter.Controls;

sealed class SmartImageList : Panel
{
    public string ImagesFolder
    {
        // ReSharper disable once InconsistentlySynchronizedField
        get => storedImageList.Folder;
        set
        {
            var v = Path.Combine(Application.StartupPath, !string.IsNullOrEmpty(value) ? value : Program.Config.ImagesFolder);

            // ReSharper disable once InconsistentlySynchronizedField
            if (storedImageList.Folder != v)
            {
                // ReSharper disable once InconsistentlySynchronizedField
                storedImageList = new(v);
                updateImages(null);
            }
        }
    }

    private static StoredImageList storedImageList = new(Path.Combine(Application.StartupPath, Program.Config.ImagesFolder));
    
    private readonly HScrollBar hPicScroll;

    // ReSharper disable once InconsistentNaming
    public MainForm mainForm = null;

    public SmartImageList()
    {
        hPicScroll = new HScrollBar();
        hPicScroll.Dock = DockStyle.Bottom;
        hPicScroll.Minimum = 0;
        hPicScroll.SmallChange = 1;
        hPicScroll.Scroll += (_, ee) => updateImages(ee.NewValue);
        hPicScroll.Parent = this;

        MouseWheel += (_, ee) =>
        {
            hPicScroll.Value = Math.Max(hPicScroll.Minimum, Math.Min(hPicScroll.Maximum - hPicScroll.LargeChange + 1, hPicScroll.Value + (ee.Delta > 0 ? -1 : 1)));
            updateImages(null);
        };

        MouseDown += (_, args) =>
        {
            if (args.Button != MouseButtons.Right) return;

            new SmartImageListItemContextMenu(mainForm!, null).Show(Cursor.Position);
        };
        
        Task.Run(backgroundAutoUpdateThread);
    }

    private void backgroundAutoUpdateThread()
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
                            if (hPicScroll.Value == Math.Max(0, hPicScroll.Maximum - hPicScroll.LargeChange + 1) && !ClientRectangle.Contains(PointToClient(Cursor.Position)))
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

    public void updateImages(int? hPicScrollValue)
    {
        var sz = Math.Max(50, ClientSize.Height - hPicScroll.Height);
        var x = 0;
        var n = 0;

        lock (storedImageList)
        {
            hPicScroll.LargeChange = (ClientSize.Width + 5) / (sz + 5);
            hPicScroll.Maximum = Math.Max(0, storedImageList.Count - 1);
            hPicScroll.Value = Math.Max(0, Math.Min(hPicScroll.Value, hPicScroll.Maximum - hPicScroll.LargeChange + 1));
            if (hPicScrollValue != null) hPicScrollValue = Math.Max(0, Math.Min(hPicScrollValue.Value, hPicScroll.Maximum - hPicScroll.LargeChange + 1));

            var i = hPicScrollValue ?? hPicScroll.Value;
            while (x < ClientSize.Width && i < storedImageList.Count)
            {
                var pb = (SmartImageListItem?)Controls.Find("pic" + n, false).SingleOrDefault() ?? createSmartImagePreview(n);

                pb.Image = storedImageList.GetAt(i).Bitmap!;
                pb.Location = new Point(x, 0);
                pb.Size = new Size(sz, sz);
                pb.FilePath = storedImageList.GetAt(i).FilePath;
                pb.Visible = true;
                //toolTip.SetToolTip(pb.PictureBox, Path.GetFileName(pb.FilePath) + "\n\nClick to load image. Double click to load also original parameters from *.json file.");

                x += sz + 5;
                i++;
                n++;
            }
        }

        while (x < ClientSize.Width)
        {
            var pb = (SmartImageListItem?)Controls.Find("pic" + n, false).FirstOrDefault();
            if (pb != null) pb.Visible = false;
            x += sz + 5;
            n++;
        }

        Refresh();
    }

    private SmartImageListItem createSmartImagePreview(int n)
    {
        var pb = new SmartImageListItem();
        pb.Name = "pic" + n;
        pb.Parent = this;

        pb.OnImageClick = () => mainForm.OpenImageFile(pb.FilePath, false);

        pb.OnImageDoubleClick = () =>
        {
            var sdGenerationParameters = SdPngHelper.LoadGenerationParameters(pb.FilePath);
            if (sdGenerationParameters != null)
            {
                mainForm.panStableDiffusion.LoadParametersToSdGenerationPanel(sdGenerationParameters);
            }
        };

        pb.OnImageRemove = () =>
        {
            lock (storedImageList)
            {
                File.Delete(pb.FilePath);
                storedImageList.Remove(pb.FilePath);

                updateImages(null);
            }
        };

        pb.OnImageContextMenu = () =>
        {
            new SmartImageListItemContextMenu(mainForm, pb.FilePath).Show(Cursor.Position);
        };

        return pb;
    }
}