namespace GameSense.Data;

public abstract class Recorder
{
    public bool Running { get; protected set; }
    public GameDemo? Demo { get; private set; }
    
    public void Start()
    {
        if (Running)
        {
            Console.WriteLine($"Recorder '{this}' is already running.");
            return;
        }
        
        Demo = new GameDemo();
        Running = true;
        OnStarted();
    }

    protected abstract void OnStarted();
    
    public GameDemo? Stop()
    {
        if (!Running)
        {
            Console.WriteLine($"Recorder '{this}' is not running.");
            return null;
        }
        
        Running = false;
        OnStopped();
        return Demo;
    }
    
    protected abstract void OnStopped();
    
}