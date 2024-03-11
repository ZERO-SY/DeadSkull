using Spectre.Console;
using DeadSkull_Lib;
using SharpPcap;

namespace DeadSkull;

internal class DeadSkull
{
    private static void Main(string[] args)
    {
        while (true)
        {
            // Enable Console To View UNICODE
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            // Cheack If All Requirment Is Exist & Update it
            LoadingTools();
            // Loading LOGO
            ASCII_LOGO();
            // Program Menu

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Retrieve the device list
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine.");
                return;
            }

            // Pick a Wi-Fi interface (you can choose the desired index)
            Console.WriteLine("Available Wi-Fi devices:");
            for (var i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"[{i}] - {devices[i].ToString()}");
            }

            Console.Write("Please choose a Wi-Fi device to capture: ");
            var devIndex = int.Parse(Console.ReadLine());
            var device = devices[devIndex];

            // Open the device in promiscuous mode
            device.Open(DeviceModes.Promiscuous);

            // Set a filter to capture only Wi-Fi handshake packets
            string filter = "wlan type mgt subtype assoc-req or wlan type mgt subtype assoc-resp";
            device.Filter = filter;

            // Register the packet arrival event handler
            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            Console.WriteLine($"Capturing from '{device.Description}', hit 'Ctrl-C' to exit...");

            // Start capturing an infinite number of packets
            device.StartCapture();
        }
    }

    private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
    {
        var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
        var wlanPacket = (PacketDotNet.WlanPacket)packet;

        // Extract relevant information from the Wi-Fi handshake packets
        if (wlanPacket != null && wlanPacket.FrameControl.SubType == PacketDotNet.WlanFrameControlSubTypes.AssocRequest)
        {
            Console.WriteLine($"Association Request from {wlanPacket.SourceAddress}");
        }
        else if (wlanPacket != null && wlanPacket.FrameControl.SubType == PacketDotNet.WlanFrameControlSubTypes.AssocResponse)
        {
            Console.WriteLine($"Association Response to {wlanPacket.SourceAddress}");
        }
    }

    private static void LoadingTools()
    {
        //Waiting to loading tool
        AnsiConsole.Status()
           .Start("Please Wait...", ctx =>
           {
               // Waiting Display
               AnsiConsole.MarkupLine("[red]Welcome To DeadSkull Tool...[/]");
               Thread.Sleep(3000);
           });
    }

    private static void ASCII_LOGO()
    {
        //ASCII LOGO
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[red bold] ______   _______ _______ ______    _______ ___ ___  ___ ___ ___     ___     \r\n|   _  \\ |   _   |   _   |   _  \\  |   _   |   Y   )|   Y   |   |   |   |    \r\n|.  |   \\|.  1___|.  1   |.  |   \\ |   1___|.  1  / |.  |   |.  |   |.  |    \r\n|.  |    |.  __)_|.  _   |.  |    \\|____   |.  _  \\ |.  |   |.  |___|.  |___ \r\n|:  1    |:  1   |:  |   |:  1    /|:  1   |:  |   \\|:  1   |:  1   |:  1   |\r\n|::.. . /|::.. . |::.|:. |::.. . / |::.. . |::.| .  |::.. . |::.. . |::.. . |\r\n`------' `-------`--- ---`------'  `-------`--- ---'`-------`-------`-------'\tV(1.0)\n\n[[By ZERO]]\r\n                                                                             \r\n[/]");

        AnsiConsole.WriteLine();
        Rule rule = new("[red]Let's Be Evil 😈[/]");
        _ = rule.LeftJustified();
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }
}