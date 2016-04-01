using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Level
{
    public string DefaultModule { get; protected set; }
    public List<Module> Modules { get; protected set; }
    public uint MaxRoomCount { get; protected set; }
    public Module RootModule { get; protected set; }

    private Level()
    {
        Modules = new List<Module>();
        MaxRoomCount = 50;
        DefaultModule = "DefaultModule";
        RootModule = (Module.ReadXML(string.Format("Assets/LevelBuilderModules/{0}.xml", DefaultModule)));
    }

    public static Level ReadRulesXML(string path)
    {
        Level level = new Level();
        //here, read rules from the given file
        foreach (var xml in Directory.GetFiles("Assets/LevelBuilderModules").Where(f => Path.GetFileNameWithoutExtension(f) != level.DefaultModule && Path.GetExtension(f) == ".xml"))
        {
            level.Modules.Add(Module.ReadXML(xml));
        }
        //fill modules with the necessary modules
        //fill max room count
        //fill root module
        return level;
    }

    public static float GetDistance(Module origin, Module destination)
    {
        return 0;
    }
}
