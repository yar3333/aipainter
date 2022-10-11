using System.Drawing.Drawing2D;

namespace AiPainter;

static class PenTools
{
    public static Pen CreateRoundPen(Color color)
    {
        var pen = new Pen(color);
        pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
        return pen;
    }

    public static Pen CreateRoundPen(Brush brush)
    {
        var pen = new Pen(brush);
        pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
        return pen;
    }
}