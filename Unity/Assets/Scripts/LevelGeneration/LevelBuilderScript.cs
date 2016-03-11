using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml.Linq;

public class LevelBuilderScript : MonoBehaviour
{
    public static LevelBuilderScript Instance { get; protected set; }
    public System.Random LevelRNG { get; protected set; }
    public Level Level { get; protected set; }
    public Dictionary<ModuleScript, Vector3> PlacedModules { get; protected set; }
    public ModuleScript RootModule { get; protected set; }
    public Floorplan Floorplan { get; protected set; }

    public int Scale;
    public ModuleScript ModuleContainerPrefab;
    public GameObject WallPrefab;
    public GameObject FloorPrefab;
    public GameObject CeilingPrefab;
    public GameObject DoorPrefab;
    public GameObject ConnectionPrefab;

    private Queue<ModuleScript> unfinishedModules;
    private int loopKill = 500; //limits to approx. rooms

    void Start()
    {
        Instance = this;
        PlacedModules = new Dictionary<ModuleScript, Vector3>();
        unfinishedModules = new Queue<ModuleScript>();
        Floorplan = new Floorplan();

        //here, pick a level "format" or "rule set" and broadcast it to clients
        //this should be able to be passed in from the lobby
        //this is also probably the best place to pick and broadcast a random seed (or, again, accept one from the lobby)
        int seed = 0;
        //int seed = (int)DateTime.Now.Ticks;
        LevelRNG = new System.Random(seed);
        Debug.Log("Seed:" + seed);
        Level = Level.ReadRulesXML("");

        Vector3 position = new Vector3(0, 0, 0);
        RootModule = Instantiate(ModuleContainerPrefab, position, Quaternion.identity) as ModuleScript;
        RootModule.Module = Level.RootModule;

        RootModule.BuildModule(2);
        PlacedModules.Add(RootModule, position);
        Floorplan.Merge(RootModule.Module.Floorplan, 0, 0, 0);

        unfinishedModules.Enqueue(RootModule);
    }

    public void Update()
    {
        ModuleScript rootModule;
        ModuleScript attachModule;
        List<Module> attemptedModules = new List<Module>();
        List<Module> eligibleModules = new List<Module>();
        Vector3 position = new Vector3(0, 0, 0);

        if (unfinishedModules.Count > 0 && loopKill > 0)
        {
            rootModule = unfinishedModules.Dequeue();
            foreach (var connectionPoint in rootModule.ConnectionPoints)
            {
                Vector3 goalPoint = GetDestinationCoords(connectionPoint);
                if (Floorplan[(int)goalPoint.x, (int)goalPoint.z, (int)goalPoint.y] != FloorplanFlags.Nothing)
                    continue;

                loopKill--;
                //create container
                attachModule = Instantiate(ModuleContainerPrefab, position, Quaternion.identity) as ModuleScript;

                eligibleModules = Level.Modules.ToList();
                bool success = false;
                while (success == false && eligibleModules.Count > 0)
                {
                    attemptedModules.Add(eligibleModules[LevelRNG.Next(0, eligibleModules.Count - 1)]); //choose a module to copy and add it to attempted modules
                    eligibleModules.Remove(attemptedModules.Last());
                    attachModule.Module = new Module(attemptedModules.Last());  //copy the most recent addition to attempted modules

                    for (int i = 0; i < 8; i++)
                    {
                        if (i == 4)
                            attachModule.Module.Floorplan.MirrorHorizontal();
                        else if (i > 0)
                            attachModule.Module.Floorplan.Rotate90CW();

                        RebuildModule(attachModule);
                        if (TryMoveToValidPosition(attachModule, connectionPoint))
                        {
                            success = true;
                            Floorplan.Merge(attachModule.Module.Floorplan, Mathf.RoundToInt(attachModule.transform.position.x)/2, Mathf.RoundToInt(attachModule.transform.position.z)/2, Mathf.RoundToInt(attachModule.transform.position.y)/2);
                            break;
                        }
                    }
                }

                if (success)
                    unfinishedModules.Enqueue(attachModule);
                else
                {
                    foreach (var child in attachModule.transform)
                        Destroy((child as Transform).gameObject);
                    Destroy(attachModule.gameObject);
                }
            }
        }
    }

    public void RebuildModule(ModuleScript module)
    {
        foreach (var child in module.transform)
            Destroy((child as Transform).gameObject);
        module.ConnectionPoints.Clear();
        module.BuildModule(2);
    }

    public bool TryMoveToValidPosition(ModuleScript attachModule, GameObject connectionPoint)
    {
        List<GameObject> sisterPoints = attachModule.ConnectionPoints.Where(p => Vector3.Angle(connectionPoint.transform.forward, p.transform.forward) == 180).ToList();
        if (sisterPoints.Any())
        {
            foreach (var point in sisterPoints)
            {
                Vector3 goalCoords = GetDestinationCoords(connectionPoint);
                Vector3 offset = goalCoords - point.transform.position;
                Debug.Log(String.Format("Testing placement of {0} at {1}", attachModule.Module.Name, offset));
                if (!Floorplan.IsConflicting(attachModule.Module.Floorplan, Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.z), Mathf.RoundToInt(offset.y)))
                {
                    attachModule.transform.position = connectionPoint.transform.GetChild(0).position - point.transform.GetChild(0).position;
                    attachModule.ConnectionPoints.Remove(point);
                    return true;
                }
                else
                    Debug.Log("Placement conflicting");
            }
        }

        return false;
    }

    public Vector3 GetDestinationCoords(GameObject connectionPoint)
    {
        Vector3 goalCoords = connectionPoint.transform.position;
        goalCoords += (connectionPoint.transform.GetChild(0).position - connectionPoint.transform.position) * 2;
        return goalCoords/Scale;
    }
}