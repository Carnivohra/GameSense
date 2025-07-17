namespace GameSense.Data;

public class GameEvent
{
    public long Timestamp { get; set; }
    public EventTrigger Trigger { get; set; }
    public EventType Type { get; set; }
}