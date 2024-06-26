﻿namespace AiPainter.Controls;

public interface IImageGenerator
{
    int GetOriginalCount();
    string GetBasePromptText();
    int GetStepsMax();
    string GetTooltip();
    
    void SetControl(GenerationListItem control);
    
    Task RunAsync();

    void LoadParamsBackToPanel();
    void Cancel();
}
