using GameSense.Data;

namespace GameSense.Core;

public class Game
{
    public string? Name { get; protected set; }
    public string? Version { get; protected set; }
    public Recorder? Recorder { get; protected init; }
}