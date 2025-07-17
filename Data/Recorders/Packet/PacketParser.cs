namespace GameSense.Data.Recorders.Packet;

public abstract class PacketParser
{
    public abstract GameEvent? Parse(PacketDotNet.Packet packet);
}