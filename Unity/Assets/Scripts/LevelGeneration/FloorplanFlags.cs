using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags]
public enum FloorplanFlags
{
    Nothing = 0x00,
    Floor = 0x01,
    Ceiling = 0x02,
    WallNorth = 0x04,
    WallSouth = 0x08,
    WallEast = 0x10,
    WallWest = 0x20,
    DoorNorth = 0x40,
    DoorSouth = 0x80,
    DoorEast = 0x0100,
    DoorWest = 0x0200,
    WindowNorth = 0x0400,
    WindowSouth = 0x0800,
    WindowEast = 0x1000,
    WindowWest = 0x2000,
    ConnectionNorth = 0x4000,
    ConnectionSouth = 0x8000,
    ConnectionEast = 0x010000,
    ConnectionWest = 0x020000
}