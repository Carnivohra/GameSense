# 🧠 GameSense

**GameSense** is a modular AI framework focused on understanding and optimizing gameplay behavior. It’s designed to evolve into a game-aware artificial intelligence that can analyze player behavior and in-game environments across different genres and game types.

> Goal: Create a generic learning environment where games are abstracted, and trainable AI can learn to achieve optimal results.

**⚠️ Status:** Work in Progress.
GameSense is currently under active development and not yet ready for production use.

---

## ✨ Features

- 🔌 **Plugin-based Architecture** – Add support for new games via external DLLs
- 🧠 **AI-Centric Design** – The core of GameSense is an intelligent agent that observes and learns from player behavior and environmental context
- 📡 **PacketRecorder Module** – Capture and interpret multiplayer game traffic through custom network parsers (optional)
- 🎮 **Recorder Support** – Support for logging player and environmental data even in offline/singleplayer games
- 🧰 **Extensible API** – Build games, recorders, parsers, and listeners with minimal effort
- ♻️ **Cross-Game Reasoning** – The AI interprets shared mechanics like movement, combat, and interaction across multiple games
- 🌐 **Environment Awareness** – Learn not only from the player's behavior but also from in-game surroundings, entities, and world state

---

## 📦 Dependencies

GameSense uses the following external libraries via NuGet:

- [SharpPcap](https://github.com/chmorgan/sharppcap) – Licensed under [LGPL-3.0](https://www.gnu.org/licenses/lgpl-3.0.html)
- [PacketDotNet](https://github.com/chmorgan/packetnet) – Licensed under [LGPL-3.0](https://www.gnu.org/licenses/lgpl-3.0.html)

These packages are used via dynamic linking (NuGet). No modifications have been made.
If you distribute compiled binaries of GameSense, please include the original license files of these packages.

---

## 🔧 Usage

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

**📦 Plugin Metadata Requirements**

To enable GameSense to automatically discover and load your plugin correctly, each compiled DLL must embed a meta.json resource file in the following format:

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
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Type = EventType.Unknown,
            Trigger = EventTrigger.System
        };
    }
}
```

---

## 📄 License

This project is licensed under the [MIT License](./LICENSE).
