using GameSense.Data.Recorders.Packet;
using PacketDotNet;
using SharpPcap;

namespace GameSense.Utils;

public static class DeviceManager
{
    public static List<PacketListener> Listeners { get; } = [];
    private static readonly CaptureDeviceList Devices = CaptureDeviceList.Instance;

    public static void Initialize()
    {
        foreach (var device in Devices)
        {
            device.OnPacketArrival += OnPacketArrival;
            device.Open();
            device.StartCapture();
        }
    }

    public static void Terminate()
    {
        foreach (var device in Devices)
        {
            device.StopCapture();
            device.Close();
            device.OnPacketArrival -= OnPacketArrival;
        }
    }

    private static void OnPacketArrival(object sender, PacketCapture packetCapture)
    {
        try
        {
            var rawCapture = packetCapture.GetPacket();
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            
            if (packet is null)
                return;

            var ipPacket = packet.Extract<IPPacket>();

            if (ipPacket is null)
                return;

            var protocol = ProtocolType.Tcp;
            TransportPacket transportPacket = packet.Extract<TcpPacket>();

            if (transportPacket is null)
            {
                protocol = ProtocolType.Udp;
                transportPacket = packet.Extract<UdpPacket>();
            }

            if (transportPacket is null)
                return;
            
            if (transportPacket.PayloadData == null || transportPacket.PayloadData.Length < 4)
                return;
            
            foreach (var listener in Listeners)
            {
                if (listener.Protocol != protocol)
                    continue;
                
                if (transportPacket.DestinationPort != listener.Port && transportPacket.SourcePort != listener.Port)
                    continue;
                
                listener.OnPacketArrival(transportPacket);
            }
        }
        catch (Exception) { }
    }
    
}