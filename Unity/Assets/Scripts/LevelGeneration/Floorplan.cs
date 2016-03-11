using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public class Floorplan : IEnumerable<KeyValuePair<int, Dictionary<int, Dictionary<int, FloorplanFlags>>>>
{
    Dictionary<int, Dictionary<int, Dictionary<int, FloorplanFlags>>> grid = new Dictionary<int, Dictionary<int, Dictionary<int, FloorplanFlags>>>();

    public Floorplan() { }

    public Floorplan(Floorplan copy)
    {
        int x, y, z;
        foreach (var kvpX in copy)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    this[x, y, z] = copy[x, y, z];
                }
            }
        }
    }

    public FloorplanFlags this[int x, int y, int z]
    {
        get
        {
            if (grid.ContainsKey(x) && grid[x].ContainsKey(y) && grid[x][y].ContainsKey(z))
                return grid[x][y][z];
            else
                return FloorplanFlags.Nothing;
        }
        protected set
        {
            if (grid.ContainsKey(x) && grid[x].ContainsKey(y) && grid[x][y].ContainsKey(z))
            {
                grid[x][y][z] = value;
            }
            else if (grid.ContainsKey(x) && grid[x].ContainsKey(y))
            {
                grid[x][y].Add(z, value);
            }
            else if (grid.ContainsKey(x))
            {
                grid[x].Add(y, new Dictionary<int, FloorplanFlags>());
                grid[x][y].Add(z, value);
            }
            else
            {
                grid.Add(x, new Dictionary<int, Dictionary<int, FloorplanFlags>>());
                grid[x].Add(y, new Dictionary<int, FloorplanFlags>());
                grid[x][y].Add(z, value);
            }
        }
    }

    public bool IsConflicting(Floorplan other, int offsetX, int offsetY, int offsetZ)
    {
        FloorplanFlags goalSquare;
        FloorplanFlags proposedSquare;
        int thisX, thisY, thisZ, otherX, otherY, otherZ;
        foreach (var kvpX in other)
        {
            otherX = kvpX.Key;
            foreach (var kvpY in kvpX.Value)
            {
                otherY = kvpY.Key;
                foreach (var kvpZ in kvpY.Value)
                {
                    otherZ = kvpZ.Key;

                    thisX = kvpX.Key + offsetX;
                    thisY = kvpY.Key + offsetY;
                    thisZ = kvpZ.Key + offsetZ;

                    goalSquare = this[thisX, thisY, thisZ];
                    if (goalSquare != FloorplanFlags.Nothing)
                        return true;

                    proposedSquare = other[otherX, otherY, otherZ];

                    //doors match
                    if (proposedSquare.HasAllFlags(FloorplanFlags.DoorEast))
                    {
                        goalSquare = this[thisX + 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.DoorWest)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.DoorWest))
                    {
                        goalSquare = this[thisX - 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.DoorEast)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.DoorNorth))
                    {
                        goalSquare = this[thisX, thisY + 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.DoorSouth)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.DoorSouth))
                    {
                        goalSquare = this[thisX, thisY - 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.DoorNorth)) return true;
                    }

                    //walls match
                    if (proposedSquare.HasAllFlags(FloorplanFlags.WallEast))
                    {
                        goalSquare = this[thisX + 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WallWest)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WallWest))
                    {
                        goalSquare = this[thisX - 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WallEast)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WallNorth))
                    {
                        goalSquare = this[thisX, thisY + 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WallSouth)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WallSouth))
                    {
                        goalSquare = this[thisX, thisY - 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WallNorth)) return true;
                    }

                    //windows match
                    if (proposedSquare.HasAllFlags(FloorplanFlags.WindowEast))
                    {
                        goalSquare = this[thisX + 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WindowWest)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WindowWest))
                    {
                        goalSquare = this[thisX - 1, thisY, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WindowEast)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WindowNorth))
                    {
                        goalSquare = this[thisX, thisY + 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WindowSouth)) return true;
                    }

                    if (proposedSquare.HasAllFlags(FloorplanFlags.WindowSouth))
                    {
                        goalSquare = this[thisX, thisY - 1, thisZ];
                        if (goalSquare != FloorplanFlags.Nothing && !goalSquare.HasAllFlags(FloorplanFlags.WindowNorth)) return true;
                    }
                }
            }
        }

        return false;
    }

    public void Merge(Floorplan other, int offsetX, int offsetY, int offsetZ)
    {
        int x, y, z;
        foreach (var kvpX in other)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    this[x + offsetX, y + offsetY, z + offsetZ] = other[x, y, z];
                }
            }
        }
    }

    public void MirrorHorizontal()
    {
        Floorplan transform = new Floorplan();

        int x, y, z;
        int xOffset = grid.Keys.Min() + grid.Keys.Max();
        foreach (var kvpX in grid)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    if (this[x, y, z] != FloorplanFlags.Nothing)
                        transform[-x + xOffset, y, z] = this[x, y, z].FlipHorizontal();
                }
            }
        }

        grid = transform.grid;
    }

    public void MirrorVertical()
    {
        Floorplan transform = new Floorplan();

        int x, y, z;
        int yOffset = grid.Min(kvp => kvp.Value.Keys.Min()) + grid.Max(kvp => kvp.Value.Keys.Max());
        foreach (var kvpX in grid)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    if (this[x, y, z] != FloorplanFlags.Nothing)
                        transform[x, -y + yOffset, z] = this[x, y, z].FlipVertical();
                }
            }
        }

        grid = transform.grid;
    }

    public void Rotate90CW()
    {
        Floorplan transform = new Floorplan();

        int x, y, z;
        int xOffset = -grid.Min(kvp => kvp.Value.Keys.Min()) + grid.Keys.Min();
        int yOffset = grid.Keys.Max() + grid.Min(kvp => kvp.Value.Keys.Min());
        foreach (var kvpX in grid)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    if (this[x, y, z] != FloorplanFlags.Nothing)
                        transform[y + xOffset, -x + yOffset, z] = this[x, y, z].Rotate90CW();
                }
            }
        }

        grid = transform.grid;
    }

    public void Rotate90CCW()
    {
        Floorplan transform = new Floorplan();

        int x, y, z;
        int xOffset = -grid.Min(kvp => kvp.Value.Keys.Min()) - grid.Keys.Min();
        int yOffset = -grid.Keys.Max() - grid.Min(kvp => kvp.Value.Keys.Min());
        foreach (var kvpX in grid)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    if (this[x, y, z] != FloorplanFlags.Nothing)
                        transform[-y + xOffset, x + yOffset, z] = this[x, y, z].Rotate90CCW();
                }
            }
        }

        grid = transform.grid;
    }

    public void Rotate180()
    {
        Floorplan transform = new Floorplan();

        int x, y, z;
        int xOffset = grid.Keys.Min() + grid.Keys.Max();
        int yOffset = grid.Min(kvp => kvp.Value.Keys.Min()) + grid.Max(kvp => kvp.Value.Keys.Max());
        foreach (var kvpX in grid)
        {
            foreach (var kvpY in kvpX.Value)
            {
                foreach (var kvpZ in kvpY.Value)
                {
                    x = kvpX.Key;
                    y = kvpY.Key;
                    z = kvpZ.Key;

                    if (this[x, y, z] != FloorplanFlags.Nothing)
                        transform[-x + xOffset, -y + yOffset, z] = this[x, y, z].Rotate180();
                }
            }
        }

        grid = transform.grid;
    }

    public void WriteXML(XElement parent)
    {
        XElement xEle, yEle, zEle;
        foreach (var kvpX in grid)
        {
            xEle = new XElement("Column", new XAttribute("Value", kvpX.Key));
            parent.Add(xEle);
            foreach (var kvpY in kvpX.Value)
            {
                yEle = new XElement("Row", new XAttribute("Value", kvpY.Key));
                xEle.Add(yEle);
                foreach (var kvpZ in kvpY.Value)
                {
                    zEle = new XElement("Floor", new XAttribute("Value", kvpZ.Key), kvpZ.Value);
                    yEle.Add(zEle);
                }
            }
        }
    }

    public static Floorplan ReadXML(XElement parent)
    {
        int x, y, z;
        Floorplan outPlan = new Floorplan();
        foreach (XElement xEle in parent.Elements())
        {
            foreach (XElement yEle in xEle.Elements())
            {
                foreach (XElement zEle in yEle.Elements())
                {
                    x = int.Parse(xEle.Attribute("Value").Value);
                    y = int.Parse(yEle.Attribute("Value").Value);
                    z = int.Parse(zEle.Attribute("Value").Value);

                    outPlan[x, y, z] = (FloorplanFlags)Enum.Parse(typeof(FloorplanFlags), zEle.Value);
                }
            }
        }

        return outPlan;
    }

    public IEnumerator<KeyValuePair<int, Dictionary<int, Dictionary<int, FloorplanFlags>>>> GetEnumerator()
    {
        var enumerator = grid.GetEnumerator();
        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
