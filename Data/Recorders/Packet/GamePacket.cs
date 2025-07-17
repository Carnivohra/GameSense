namespace GameSense.Data.Recorders.Packet;

public abstract class GamePacket
{
    public abstract byte[] WritePayload();
    public abstract void ParsePayload(byte[] payload);
}