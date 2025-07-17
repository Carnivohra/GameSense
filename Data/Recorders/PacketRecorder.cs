using GameSense.Data.Recorders.Packet;
using GameSense.Utils;

namespace GameSense.Data.Recorders;

public class PacketRecorder : Recorder
{
    protected List<PacketListener> Listeners { get; } = [];

    protected override void OnStarted()
    {
        foreach (var listener in Listeners) 
            DeviceManager.Listeners.Add(listener);
    }

    protected override void OnStopped()
    {
        foreach (var listener in Listeners)
            DeviceManager.Listeners.Remove(listener);
    }
}