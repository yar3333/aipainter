using AiPainter.Helpers;

namespace AiPainter.Controls;

sealed class SmartImageListItemContextMenu : ContextMenuStrip
{
    public SmartImageListItemContextMenu(MainForm mainForm, string imageFilePath)
    {
        Opened += (_, _) => mainForm.toolTip.Active = false;
        Closed += (_, _) => mainForm.toolTip.Active = true;
        
        var parentFolder = Path.GetDirectoryName(mainForm.ImagesFolder);
        if (parentFolder != Application.StartupPath.TrimEnd('\\', '/'))
        {
            Items.Add("Open parent folder (" + Path.GetFileName(parentFolder) + ")", null, (_, _) =>
            {
                mainForm.ImagesFolder = parentFolder;
            });
        }
        else
        {
            Items.Add(new ToolStripMenuItem("Open parent") { Enabled = false }); 
        }

        var subfolderMenuItem = new ToolStripMenuItem("Open subfolder");
        foreach (var dir in Directory.GetDirectories(mainForm.ImagesFolder!))
        {
            subfolderMenuItem.DropDownItems.Add(Path.GetFileName(dir), null, (_, _) =>
            {
                mainForm.ImagesFolder = dir;
            });
        }
        if (subfolderMenuItem.DropDownItems.Count > 0) Items.Add(subfolderMenuItem);
        else                                           Items.Add(new ToolStripMenuItem("Open subfolder") { Enabled = false }); 

        Items.Add(new ToolStripSeparator());
        
        Items.Add("Move image to subfolder", null, (_, _) =>
        {
            var baseFileName = Path.GetFileNameWithoutExtension(imageFilePath);
            var srcDir = Path.GetDirectoryName(imageFilePath)!;

            moveImageFile(imageFilePath, Path.Combine(srcDir, baseFileName));
        });

        Items.Add("Move image to subfolder and open", null, (_, _) =>
        {
            var baseFileName = Path.GetFileNameWithoutExtension(imageFilePath);
            var srcDir = Path.GetDirectoryName(imageFilePath)!;

            var destImagePath = moveImageFile(imageFilePath, Path.Combine(srcDir, baseFileName));
            mainForm.OpenImageFile(destImagePath);
            mainForm.ImagesFolder = Path.GetDirectoryName(destImagePath);
        });

        if (parentFolder != Application.StartupPath.TrimEnd('\\', '/'))
        {
            Items.Add(new ToolStripSeparator());

            Items.Add("Move image to parent folder (" + Path.GetFileName(Path.GetDirectoryName(mainForm.ImagesFolder)) + ")", null, (_, _) =>
            {
                var srcDir = Path.GetDirectoryName(imageFilePath)!;
                var destDir = Path.GetDirectoryName(srcDir);
                if (destDir == null) return;

                moveImageFile(imageFilePath, destDir);

                if (Directory.GetDirectories(srcDir).Length == 0 && Directory.GetFiles(srcDir).Length == 0)
                {
                    try { Directory.Delete(srcDir); } catch { }
                }
            });

            Items.Add("Move image to parent folder and open (" + Path.GetFileName(Path.GetDirectoryName(mainForm.ImagesFolder)) + ")", null, (_, _) =>
            {
                var srcDir = Path.GetDirectoryName(imageFilePath)!;
                var destDir = Path.GetDirectoryName(srcDir);
                if (destDir == null) return;

                var destImagePath = moveImageFile(imageFilePath, destDir);
                mainForm.OpenImageFile(destImagePath);
                mainForm.ImagesFolder = destDir;

                if (Directory.GetDirectories(srcDir).Length == 0 && Directory.GetFiles(srcDir).Length == 0)
                {
                    try { Directory.Delete(srcDir); } catch { }
                }
            });
        }

        Items.Add(new ToolStripSeparator());

        Items.Add("Show in Explorer", null, (_, _) =>
        {
            ProcessHelper.ShowFileInExplorer(imageFilePath);
        });
        
        Items.Add("Rename folder (" + Path.GetFileName(mainForm.ImagesFolder) + ")", null, (_, _) =>
        {
            var name = Path.GetFileName(mainForm.ImagesFolder)!;
            var dialog = new FolderNameDialog(name, null);
            if (dialog.ShowDialog(this) == DialogResult.OK && dialog.ResultFolderName != name)
            {
                var destFolder = Path.Combine(Path.GetDirectoryName(mainForm.ImagesFolder)!, dialog.ResultFolderName!);
                if (!Directory.Exists(destFolder))
                {
                    var srcFolder = mainForm.ImagesFolder!;
                    mainForm.ImagesFolder = destFolder;
                    Directory.Move(srcFolder, destFolder);
                }
            }
        });
        
        Items.Add(new ToolStripSeparator());
        
        Items.Add("Open image with default application", null, (_, _) =>
        {
            ProcessHelper.OpenUrlInBrowser(imageFilePath);
        });
    }

    private static string moveImageFile(string srcFilePath, string destDir)
    {
        var baseFileName = Path.GetFileNameWithoutExtension(srcFilePath);
        var srcDir = Path.GetDirectoryName(srcFilePath)!;

        if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

        var destImagePath = Path.Combine(destDir, Path.GetFileName(srcFilePath));
        File.Move(srcFilePath, destImagePath);
        if (File.Exists(Path.Combine(srcDir, baseFileName) + ".json"))
        {
            File.Move(Path.Combine(srcDir, baseFileName) + ".json", Path.Combine(destDir, baseFileName) + ".json");
        }

        return destImagePath;
    }
}