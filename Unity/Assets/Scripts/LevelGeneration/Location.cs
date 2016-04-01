using UnityEngine;
using System.Collections;

public class Location
{
    public Vector3 Position { get; protected set; }
    public Vector3 Rotation { get; protected set; }

    public Location(Vector3 Position, Vector3 Rotation)
    {
        this.Position = Position;
        this.Rotation = Rotation;
    }

    public Location(Location copy)
    {
        this.Position = copy.Position;
        this.Rotation = copy.Rotation;
    }
}
