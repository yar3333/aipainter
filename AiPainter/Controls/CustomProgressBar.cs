namespace AiPainter.Controls;

class CustomProgressBar : ProgressBar
{
    public string? CustomText { get; set; }
    public Color TextColor { get; set; } = Color.Black;

    public CustomProgressBar()
    {
        SetStyle
        (
            ControlStyles.UserPaint 
          | ControlStyles.AllPaintingInWmPaint 
          | ControlStyles.OptimizedDoubleBuffer,
            true
        );
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        using var brush = new SolidBrush(BackColor);
        pevent.Graphics.FillRectangle(brush, ClientRectangle);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var rect = ClientRectangle;
        var g = e.Graphics;

        ProgressBarRenderer.DrawHorizontalBar(g, rect);

        rect.Inflate(-1, -1);
        using var backBrush = new SolidBrush(BackColor);
        g.FillRectangle(backBrush, rect);

        rect.Inflate(-2, -2);
        if (Value > 0)
        {
            var clip = rect with { Width = (int)Math.Round((float)Value / Maximum * rect.Width) };
            ProgressBarRenderer.DrawHorizontalChunks(g, clip);
        }

        var percent = (int)((double)Value / Maximum * 100);
        var text = CustomText ?? percent.ToString() + '%';

        using var brush = new SolidBrush(TextColor);

        using var f = new Font(FontFamily.GenericSerif, 10);
        var len = g.MeasureString(text, f);
        var location = new Point(Convert.ToInt32(Width / 2 - len.Width / 2), Convert.ToInt32(Height / 2 - len.Height / 2));
        g.DrawString(text, f, brush, location);
    }
}
