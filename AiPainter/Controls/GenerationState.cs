namespace AiPainter.Controls;

public enum GenerationState
{
    WAITING,
    IN_PROCESS,
    IN_PROCESS_WANT_CANCEL,
    PART_FINISHED,
    FULLY_FINISHED,
}
