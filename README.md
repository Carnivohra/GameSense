# 🧠 GameSense

**GameSense** is a modular AI framework for analyzing and optimizing gameplay. It allows dynamic integration of any game through a plugin-based system (DLL) and is designed to record gameplay data for use in machine learning or AI logic.

> Goal: Create a generic learning environment where games are abstracted, and trainable AI can learn to achieve optimal results.

⚠️ Status: Work in Progress
GameSense is currently under active development and not yet ready for production use.

---

## ✨ Features

- 🔌 **Plugin-based**: Load external games via DLLs with a simple interface
- 🧠 **AI-ready design**: Recorders track gameplay for analysis and training
- 📦 **PacketRecorder support**: Record gameplay events from raw network packets with custom parsers
- 🔄 **Dynamic Loading**: DLL-based game modules are automatically scanned and initialized
- 🧰 **Extensible API**: Base classes for games, plugins, recorders, parsers & listeners

---

## 🛠 Project Structure

```plaintext
GameSense/
├── Core/
│   ├── Game.cs          # Base class for a game
│   └── Plugin.cs        # Abstract plugin entry point
├── Data/
│   ├── Recorder.cs      # Base class for recorders
│   ├── GameDemo.cs      # Represents a recording session
│   └── Recorders/
│       └── Packet/
│           ├── PacketRecorder.cs
│           ├── PacketListener.cs
│           └── PacketParser.cs
└── Utils/               # GameManager, PluginManager, DeviceManager
```

## Usage

**🧩 Plugin Architecture**

A plugin is a DLL that inherits from the abstract Plugin class and registers one or more games:

```C#
public class MyPlugin : Plugin
{
    public MyPlugin() 
    {
        Games.Add(new MyGame());
    }

    protected override void OnEnable()
    {
        //Plugin enable logic
    }

    protected override void OnDisable()
    {
        //Plugin disable logic
    }
}
```

**📦 Plugin Metadata (Required!)**

To enable GameSense to discover and load your plugin correctly, each compiled DLL must embed a meta.json resource file in the following format:

```json
{
  "plugin": "MyPluginNamespace.MyPlugin"
}
```

The plugin key must contain the fully qualified class name of your Plugin implementation.

The file should be embedded as a resource at:
``Resources/meta.json`` inside your project.

GameSense will scan the ``Plugins/`` folder, read this metadata, load the assembly, and activate your plugin via reflection.

**🎮 Defining a Game**

Games inherit from the Game class and can include a Recorder instance to track gameplay data:

```C#
public class MyGame : Game
{
    public MyGame()
    {
        Name = "MyGame";
        Version = "1.0";
        Recorder = new MyPacketRecorder();
    }
}
```

**🎥 Recording Gameplay (Packet-based)**

To track network-based gameplay, extend PacketRecorder:

```C#
public class MyPacketRecorder : PacketRecorder
{
    public MyPacketRecorder()
    {
        Listeners.Add(new MyListener(this));
    }
}
```

Create a listener and parser for incoming packets:

```C#
public class MyListener : PacketListener
{
    public MyListener(PacketRecorder recorder) : base(recorder)
    {
        Port = 1234;
        Protocol = ProtocolType.Tcp;
        Parser = new MyPacketParser();
    }
}
```

```C#
public class MyPacketParser : PacketParser
{
    public override GameEvent Parse(PacketDotNet.Packet packet)
    {
        return new GameEvent
        {
            Timestamp = DateTime.UtcNow,
            Description = "Parsed game event"
        };
    }
}
```
