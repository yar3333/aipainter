using System.Reflection;
using AiPainter.Helpers;

namespace AiPainter
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AiPainter.AboutDialog.rtf")!;
            using var reader = new StreamReader(stream);
            rtbText.Rtf =  reader.ReadToEnd();
        }

        private void rtbText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ProcessHelper.OpenUrlInBrowser(e.LinkText!);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
