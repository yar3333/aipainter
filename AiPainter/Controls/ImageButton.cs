namespace AiPainter.Controls;

public class ImageButton : PictureBox
{
    protected override void OnPaintBackground(PaintEventArgs pe)
    {
        pe.Graphics.Clear(Color.Transparent);
    }
}
