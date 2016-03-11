using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

public class ModuleScript : MonoBehaviour
{
    public Module Module { get; set; }
    public List<GameObject> ConnectionPoints { get; protected set; }

    public void Awake()
    {
        ConnectionPoints = new List<GameObject>();
    }

    public void BuildModule(int scale)
    {
        Vector3 position;
        FloorplanFlags flag;
        int i, j, k;

        Module.Floorplan.Rotate180();

        foreach (var kvpX in Module.Floorplan)
        {
            i = kvpX.Key;
            foreach (var kvpY in kvpX.Value)
            {
                j = kvpY.Key;
                foreach (var kvpZ in kvpY.Value)
                {
                    k = kvpZ.Key;
                    flag = Module.Floorplan[i, j, k];
                    position = new Vector3(i, k, j) * scale;

                    if ((flag & FloorplanFlags.Ceiling) == FloorplanFlags.Ceiling)
                        (Instantiate(LevelBuilderScript.Instance.CeilingPrefab, position, Quaternion.identity) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.Floor) == FloorplanFlags.Floor)
                        (Instantiate(LevelBuilderScript.Instance.FloorPrefab, position, Quaternion.identity) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.WallNorth) == FloorplanFlags.WallNorth)
                        (Instantiate(LevelBuilderScript.Instance.WallPrefab, position, Quaternion.identity) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.WallSouth) == FloorplanFlags.WallSouth)
                        (Instantiate(LevelBuilderScript.Instance.WallPrefab, position, Quaternion.Euler(0, 180, 0)) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.WallEast) == FloorplanFlags.WallEast)
                        (Instantiate(LevelBuilderScript.Instance.WallPrefab, position, Quaternion.Euler(0, 90, 0)) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.WallWest) == FloorplanFlags.WallWest)
                        (Instantiate(LevelBuilderScript.Instance.WallPrefab, position, Quaternion.Euler(0, 270, 0)) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.DoorNorth) == FloorplanFlags.DoorNorth)
                        (Instantiate(LevelBuilderScript.Instance.DoorPrefab, position, Quaternion.identity) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.DoorSouth) == FloorplanFlags.DoorSouth)
                        (Instantiate(LevelBuilderScript.Instance.DoorPrefab, position, Quaternion.Euler(0, 180, 0)) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.DoorEast) == FloorplanFlags.DoorEast)
                        (Instantiate(LevelBuilderScript.Instance.DoorPrefab, position, Quaternion.Euler(0, 90, 0)) as GameObject).transform.SetParent(transform, false);

                    if ((flag & FloorplanFlags.DoorWest) == FloorplanFlags.DoorWest)
                        (Instantiate(LevelBuilderScript.Instance.DoorPrefab, position, Quaternion.Euler(0, 270, 0)) as GameObject).transform.SetParent(transform, false);

                    GameObject tempPoint;
                    if ((flag & FloorplanFlags.ConnectionNorth) == FloorplanFlags.ConnectionNorth)
                    {
                        tempPoint = (Instantiate(LevelBuilderScript.Instance.ConnectionPrefab, position, Quaternion.identity) as GameObject);
                        tempPoint.transform.SetParent(transform, false);
                        ConnectionPoints.Add(tempPoint);
                    }

                    if ((flag & FloorplanFlags.ConnectionSouth) == FloorplanFlags.ConnectionSouth)
                    {
                        tempPoint = (Instantiate(LevelBuilderScript.Instance.ConnectionPrefab, position, Quaternion.Euler(0, 180, 0)) as GameObject);
                        tempPoint.transform.SetParent(transform, false);
                        ConnectionPoints.Add(tempPoint);
                    }

                    if ((flag & FloorplanFlags.ConnectionEast) == FloorplanFlags.ConnectionEast)
                    {
                        tempPoint = (Instantiate(LevelBuilderScript.Instance.ConnectionPrefab, position, Quaternion.Euler(0, 90, 0)) as GameObject);
                        tempPoint.transform.SetParent(transform, false);
                        ConnectionPoints.Add(tempPoint);
                    }

                    if ((flag & FloorplanFlags.ConnectionWest) == FloorplanFlags.ConnectionWest)
                    {
                        tempPoint = (Instantiate(LevelBuilderScript.Instance.ConnectionPrefab, position, Quaternion.Euler(0, 270, 0)) as GameObject);
                        tempPoint.transform.SetParent(transform, false);
                        ConnectionPoints.Add(tempPoint);
                    }
                }
            }
        }
    }
}
