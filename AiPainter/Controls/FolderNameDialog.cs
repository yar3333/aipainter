namespace AiPainter.Controls;

public partial class FolderNameDialog : Form
{
    public string? ResultFolderName;

    public FolderNameDialog(string defaultName, string[]? suggestions)
    {
        InitializeComponent();

        cbFolderName.DataSource = suggestions;
        cbFolderName.Text = defaultName;
    }

    private void btOk_Click(object sender, EventArgs e)
    {
        ResultFolderName = cbFolderName.Text;
    }
}
