using GameSense.Utils;

namespace GameSense.Core;

public abstract class Plugin
{
    public bool Enabled { get; private set; }
    protected List<Game> Games { get; } = [];
    
    public void Enable()
    {
        foreach (var game in Games) GameManager.Games.Add(game);
        
        Enabled = true;
        OnEnable();
    }

    protected abstract void OnEnable();
    
    public void Disable()
    {
        foreach (var game in Games) GameManager.Games.Remove(game);
        
        Enabled = false;
        OnDisable();
    }
    
    protected abstract void OnDisable();
    
}