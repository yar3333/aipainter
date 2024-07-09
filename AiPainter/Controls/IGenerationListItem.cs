namespace AiPainter.Controls;

public interface IGenerationListItem
{
    public bool HasWorkToRun { get; }
    public bool InProcess { get; }
    public bool WantToBeRemoved { get; }

    public void Run();
    
    ///////////////////////////////////////////
    public int Width { get; set; }
    public int Top { get; set; }
    public Size ClientSize { get; }
    public AnchorStyles Anchor { get; set; }
    public Control Parent  { get; set; }
    public void Dispose();
}
