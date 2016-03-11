using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;

public class Module
{
    public String Name { get; protected set; }
    public List<String> Keywords { get; protected set; }
    public Floorplan Floorplan { get; protected set; }
    public List<Location> DecorationPoints { get; protected set; }

    protected Module()
    {
        Keywords = new List<string>();
        Floorplan = new Floorplan();
        DecorationPoints = new List<Location>();
    }

    public Module(Module copy)
    {
        this.Name = copy.Name;
        this.Keywords = copy.Keywords.Copy();
        this.Floorplan = new Floorplan(copy.Floorplan);
        DecorationPoints = new List<Location>();
        foreach (var location in copy.DecorationPoints)
            DecorationPoints.Add(new Location(location));
    }

    public XElement WriteXML()
    {
        XElement outEle = new XElement("Module",
                            new XAttribute("Name", Name),
                            new XElement("Keywords"),
                            new XElement("Floorplan"),
                            new XElement("DecorationPoints")
                        );

        XElement ele = outEle.Element("Keywords");
        foreach (var keyword in Keywords)
            ele.Add(new XElement("Keyword", keyword));

        ele = outEle.Element("Floorplan");
        Floorplan.WriteXML(ele);

        return outEle;
    }

    public static Module ReadXML(string path)
    {
        Module outModule = new Module();
        XDocument doc = XDocument.Load(path);

        outModule.Name = doc.Root.Attribute("Name").Value;
        outModule.Keywords = new List<string>();
        foreach (var ele in doc.Root.Element("Keywords").Elements())
            outModule.Keywords.Add(ele.Value);

        outModule.Floorplan = Floorplan.ReadXML(doc.Root.Element("Floorplan"));

        return outModule;
    }
}
