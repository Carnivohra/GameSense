using System.Reflection;
using System.Text.Json;
using GameSense.Core;

namespace GameSense.Utils;

public static class PluginManager
{
    public static Dictionary<string, Plugin> Plugins { get; } = new();
    private const string PluginsPath = "Plugins/";

    public static void Initialize()
    {
        CreateDefaultDirectory();
        LoadAll();
    }

    public static void Terminate()
    {
        UnloadAll();
    }
    
    private static void CreateDefaultDirectory()
    {
        if (Directory.Exists(PluginsPath))
            return;
        
        var directory = Directory.CreateDirectory(PluginsPath);
        Console.WriteLine($"Directory '{directory.Name}' has been created.");
    }
    
    private static void Load(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"File '{fileName}' not found.");
            return;
        }

        var name = Path.GetFileName(fileName);
        
        if (!fileName.EndsWith(".dll"))
        {
            Console.WriteLine($"File '{name}' is no supported format.");
            return;
        }
        
        var assembly = Assembly.LoadFile(fileName);
        var metaJson = GetMeta(assembly);
        
        if (metaJson is null)
        {
            Console.WriteLine($"Could not load 'meta.json' from file '{name}'.");
            return;
        }
        
        var rootElement = metaJson.RootElement;
        var pluginElement = rootElement.GetProperty("plugin");
        var pluginTypeName = pluginElement.GetString();

        if (pluginTypeName is null)
        {
            Console.WriteLine($"Meta property '{pluginElement.ValueKind}' is not defined for file '{name}'.");
            return;   
        }
        
        var type = assembly.GetType(pluginTypeName);

        if (type is null)
        {
            Console.WriteLine($"Could not find type '{pluginTypeName}' in file '{name}'.");
            return;
        }

        var plugin = (Plugin?) Activator.CreateInstance(type);

        if (plugin is null)
        {
            Console.WriteLine($"Could not create instance of type '{type}' in file '{name}'.");
            return;
        }
        
        plugin.Enable();
        Plugins.Add(pluginTypeName, plugin);
        Console.WriteLine($"Plugin '{pluginTypeName}' has been loaded successfully.");
    }

    private static JsonDocument? GetMeta(Assembly assembly)
    {
        var name = assembly.GetName().Name;
        var stream = assembly.GetManifestResourceStream(name + ".Resources.meta.json");

        if (stream is null) return null;
        
        var reader =  new StreamReader(stream);
        var jsonString = reader.ReadToEnd();
        return JsonDocument.Parse(jsonString);
    }
    
    private static void LoadAll()
    {
        var fileNames = Directory.GetFiles(PluginsPath, "*.dll");
        var baseDirectory = AppContext.BaseDirectory;
        
        foreach (var fileName in fileNames)
        {
            var absolutePath = Path.Combine(baseDirectory, fileName);
            Load(absolutePath);
        }
    }
    
    private static void Unload(string fileName)
    {
        var plugin = Plugins[fileName];
        plugin.Disable();
        Plugins.Remove(fileName);
    }

    private static void UnloadAll()
    {
        foreach (var pluginName in Plugins.Keys.ToList())
            Unload(pluginName);
    }
    
}