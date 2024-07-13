using AiPainter.Helpers;

namespace AiPainter.Controls;

sealed class SmartImageListItemContextMenu : ContextMenuStrip
{
    public SmartImageListItemContextMenu(MainForm mainForm, string? imageFilePath)
    {
        Opened += (_, _) => mainForm.toolTip.Active = false;
        Closed += (_, _) => mainForm.toolTip.Active = true;
        
        var parentFolder = Path.GetDirectoryName(mainForm.ImagesFolder);
        if (parentFolder != null && parentFolder != Application.StartupPath.TrimEnd('\\', '/'))
        {
            Items.Add("Open parent folder (" + Path.GetFileName(parentFolder) + ")", null, (_, _) =>
            {
                mainForm.ImagesFolder = parentFolder;
            });
        }
        else
        {
            Items.Add(new ToolStripMenuItem("Open parent folder") { Enabled = false }); 
        }

        var subfolders = Directory.Exists(mainForm.ImagesFolder) 
                             ? Directory.GetDirectories(mainForm.ImagesFolder)
                             : new string[] {};
        if (subfolders.Length > 0)
        {
            var openSubfolderMenuItem = new ToolStripMenuItem("Open subfolder");
            foreach (var dir in subfolders)
            {
                openSubfolderMenuItem.DropDownItems.Add(Path.GetFileName(dir), null, (_, _) =>
                {
                    mainForm.ImagesFolder = dir;
                });
            }
            Items.Add(openSubfolderMenuItem);
        }
        else
        {
            Items.Add(new ToolStripMenuItem("Open subfolder") { Enabled = false }); 
        }

        if (imageFilePath != null)
        {
            Items.Add(new ToolStripSeparator());
            
            {
                var moveImageToSubfolderMenuItem = new ToolStripMenuItem("Move image to subfolder");
                moveImageToSubfolderMenuItem.DropDownItems.Add("<NEW>", null, (_, _) =>
                {
                    var dialog = new FolderNameDialog(Path.GetFileNameWithoutExtension(imageFilePath), null);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        moveImageFile(imageFilePath, Path.Combine(mainForm.ImagesFolder, dialog.ResultFolderName));
                    }
                });
                if (subfolders.Length > 0)
                {
                    moveImageToSubfolderMenuItem.DropDownItems.Add(new ToolStripSeparator());
                    foreach (var dir in subfolders)
                    {
                        moveImageToSubfolderMenuItem.DropDownItems.Add(Path.GetFileName(dir), null, (_, _) =>
                        {
                            moveImageFile(imageFilePath, dir);
                        });
                    }
                }
                Items.Add(moveImageToSubfolderMenuItem);
            }
            
            {
                var moveImageToSubfolderAndOpenMenuItem = new ToolStripMenuItem("Move image to subfolder and open");
                moveImageToSubfolderAndOpenMenuItem.DropDownItems.Add("<NEW>", null, (_, _) =>
                {
                    var dialog = new FolderNameDialog(Path.GetFileNameWithoutExtension(imageFilePath), null);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var destImagePath = moveImageFile(imageFilePath, Path.Combine(mainForm.ImagesFolder, dialog.ResultFolderName));
                        mainForm.OpenImageFile(destImagePath);
                        mainForm.ImagesFolder = Path.GetDirectoryName(destImagePath)!;
                    }
                });
                if (subfolders.Length > 0)
                {
                    moveImageToSubfolderAndOpenMenuItem.DropDownItems.Add(new ToolStripSeparator());
                    foreach (var dir in subfolders)
                    {
                        moveImageToSubfolderAndOpenMenuItem.DropDownItems.Add(Path.GetFileName(dir), null, (_, _) =>
                        {
                            var destImagePath = moveImageFile(imageFilePath, dir);
                            mainForm.OpenImageFile(destImagePath);
                            mainForm.ImagesFolder = Path.GetDirectoryName(destImagePath)!;
                        });
                    }
                }
                Items.Add(moveImageToSubfolderAndOpenMenuItem);
            }

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
        }

        Items.Add(new ToolStripSeparator());

        Items.Add("Show in Explorer", null, (_, _) =>
        {
            if (imageFilePath != null) ProcessHelper.ShowFileInExplorer(imageFilePath);
            else                       ProcessHelper.ShowFolderInExplorer(mainForm.ImagesFolder);
        });
        
        Items.Add("Rename folder (" + Path.GetFileName(mainForm.ImagesFolder) + ")", null, (_, _) =>
        {
            var name = Path.GetFileName(mainForm.ImagesFolder);
            var dialog = new FolderNameDialog(name, null);
            if (dialog.ShowDialog(this) == DialogResult.OK && dialog.ResultFolderName != name)
            {
                var destFolder = Path.Combine(Path.GetDirectoryName(mainForm.ImagesFolder)!, dialog.ResultFolderName!);
                if (!Directory.Exists(destFolder))
                {
                    var srcFolder = mainForm.ImagesFolder;
                    mainForm.ImagesFolder = destFolder;
                    Directory.Move(srcFolder, destFolder);
                }
            }
        });
        
        if (imageFilePath != null)
        {
            Items.Add(new ToolStripSeparator());
            
            Items.Add("Open image with default application", null, (_, _) =>
            {
                ProcessHelper.OpenUrlInBrowser(imageFilePath);
            });
        }
    }

    private static string moveImageFile(string srcFilePath, string destDir)
    {
        if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

        var destImagePath = Path.Combine(destDir, Path.GetFileName(srcFilePath));
        File.Move(srcFilePath, destImagePath);

        return destImagePath;
    }
}