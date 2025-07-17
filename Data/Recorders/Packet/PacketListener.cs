using PacketDotNet;

namespace GameSense.Data.Recorders.Packet;

public abstract class PacketListener(PacketRecorder recorder)
{
    public ProtocolType Protocol { get; protected init; } = ProtocolType.Tcp;
    public ushort Port { get; protected set; }
    protected PacketParser? Parser { get; init; }

    public void OnPacketArrival(PacketDotNet.Packet packet)
    {
        if (Parser is null)
        {
            Console.WriteLine($"PacketListener '{this}' has not parser set.");
            return;
        } 
        
        var gameEvent = Parser.Parse(packet);
        
        if (recorder.Demo is null)
        {
            Console.WriteLine($"Could not add GameEvent '{gameEvent}' to demo. Demo is null.");
            return;
        } 
        
        recorder.Demo.GameEvents.Add(gameEvent);
    }

}