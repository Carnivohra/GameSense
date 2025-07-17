using GameSense.Utils;

namespace GameSense;

public static class Program
{
    public static void Main(string[] args)
    {
        PluginManager.Initialize();
        DeviceManager.Initialize();

        foreach (var game in GameManager.Games) 
            game.Recorder?.Start();

        Console.ReadKey();
    }
    
}