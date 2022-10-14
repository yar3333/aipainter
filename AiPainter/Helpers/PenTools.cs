using System.Drawing.Drawing2D;

namespace AiPainter.Helpers;

static class PenTools
{
    public static Pen CreateRoundPen(Brush brush)
    {
        var pen = new Pen(brush);
        pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
        return pen;
    }
}