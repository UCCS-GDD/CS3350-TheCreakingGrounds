using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts;

public static class Utility
{
    private static GameObject serverController;
    public static GameObject ServerController
    {
        get
        {
            if (serverController == null)
                serverController = GameObject.Find("ServerManagement");
            return serverController;
        }
        set
        {
            serverController = value;
        }
    }

    public static GameObject GetParentActivator(this GameObject gObj)
    {
        GameObject curObj = gObj;

        while (curObj.GetComponent<Assets.Scripts.Activator>() == null)
        {
            if (curObj.transform.parent != null)
                curObj = curObj.transform.parent.gameObject;
            else
                return null;
        }

        return curObj;
    }
    public static void FillFromResources<T>(this IEnumerable<T> fillArray, string resourcePath) where T : UnityEngine.Object 
    {
        fillArray = Resources.LoadAll<T>(resourcePath);
    }

    public static T PickRandom<T>(this IEnumerable<T> pickArray)
    {
        return pickArray.ToArray()[UnityEngine.Random.Range(0, pickArray.Count())];
    }
    public static List<T> Copy<T>(this List<T> copyFrom)
    {
        T[] array = new T[copyFrom.Count];
        copyFrom.CopyTo(array);
        return new List<T>(array);
    }

    public static FloorplanFlags Rotate90CW(this FloorplanFlags original)
    {
        FloorplanFlags outFlags = original & (FloorplanFlags.Nothing | FloorplanFlags.Ceiling | FloorplanFlags.Floor);

        if ((original & FloorplanFlags.DoorEast) == FloorplanFlags.DoorEast)
            outFlags |= FloorplanFlags.DoorSouth;

        if ((original & FloorplanFlags.DoorNorth) == FloorplanFlags.DoorNorth)
            outFlags |= FloorplanFlags.DoorEast;

        if ((original & FloorplanFlags.DoorSouth) == FloorplanFlags.DoorSouth)
            outFlags |= FloorplanFlags.DoorWest;

        if ((original & FloorplanFlags.DoorWest) == FloorplanFlags.DoorWest)
            outFlags |= FloorplanFlags.DoorNorth;


        if ((original & FloorplanFlags.WallEast) == FloorplanFlags.WallEast)
            outFlags |= FloorplanFlags.WallSouth;

        if ((original & FloorplanFlags.WallNorth) == FloorplanFlags.WallNorth)
            outFlags |= FloorplanFlags.WallEast;

        if ((original & FloorplanFlags.WallSouth) == FloorplanFlags.WallSouth)
            outFlags |= FloorplanFlags.WallWest;

        if ((original & FloorplanFlags.WallWest) == FloorplanFlags.WallWest)
            outFlags |= FloorplanFlags.WallNorth;


        if ((original & FloorplanFlags.WindowEast) == FloorplanFlags.WindowEast)
            outFlags |= FloorplanFlags.WindowSouth;

        if ((original & FloorplanFlags.WindowNorth) == FloorplanFlags.WindowNorth)
            outFlags |= FloorplanFlags.WindowEast;

        if ((original & FloorplanFlags.WindowSouth) == FloorplanFlags.WindowSouth)
            outFlags |= FloorplanFlags.WindowWest;

        if ((original & FloorplanFlags.WindowWest) == FloorplanFlags.WindowWest)
            outFlags |= FloorplanFlags.WindowNorth;


        if ((original & FloorplanFlags.ConnectionEast) == FloorplanFlags.ConnectionEast)
            outFlags |= FloorplanFlags.ConnectionSouth;

        if ((original & FloorplanFlags.ConnectionNorth) == FloorplanFlags.ConnectionNorth)
            outFlags |= FloorplanFlags.ConnectionEast;

        if ((original & FloorplanFlags.ConnectionSouth) == FloorplanFlags.ConnectionSouth)
            outFlags |= FloorplanFlags.ConnectionWest;

        if ((original & FloorplanFlags.ConnectionWest) == FloorplanFlags.ConnectionWest)
            outFlags |= FloorplanFlags.ConnectionNorth;

        return outFlags;
    }

    public static FloorplanFlags Rotate90CCW(this FloorplanFlags original)
    {
        FloorplanFlags outFlags = original & (FloorplanFlags.Nothing | FloorplanFlags.Ceiling | FloorplanFlags.Floor);

        if ((original & FloorplanFlags.DoorEast) == FloorplanFlags.DoorEast)
            outFlags |= FloorplanFlags.DoorNorth;

        if ((original & FloorplanFlags.DoorNorth) == FloorplanFlags.DoorNorth)
            outFlags |= FloorplanFlags.DoorWest;

        if ((original & FloorplanFlags.DoorSouth) == FloorplanFlags.DoorSouth)
            outFlags |= FloorplanFlags.DoorEast;

        if ((original & FloorplanFlags.DoorWest) == FloorplanFlags.DoorWest)
            outFlags |= FloorplanFlags.DoorSouth;


        if ((original & FloorplanFlags.WallEast) == FloorplanFlags.WallEast)
            outFlags |= FloorplanFlags.WallNorth;

        if ((original & FloorplanFlags.WallNorth) == FloorplanFlags.WallNorth)
            outFlags |= FloorplanFlags.WallWest;

        if ((original & FloorplanFlags.WallSouth) == FloorplanFlags.WallSouth)
            outFlags |= FloorplanFlags.WallEast;

        if ((original & FloorplanFlags.WallWest) == FloorplanFlags.WallWest)
            outFlags |= FloorplanFlags.WallSouth;


        if ((original & FloorplanFlags.WindowEast) == FloorplanFlags.WindowEast)
            outFlags |= FloorplanFlags.WindowNorth;

        if ((original & FloorplanFlags.WindowNorth) == FloorplanFlags.WindowNorth)
            outFlags |= FloorplanFlags.WindowWest;

        if ((original & FloorplanFlags.WindowSouth) == FloorplanFlags.WindowSouth)
            outFlags |= FloorplanFlags.WindowEast;

        if ((original & FloorplanFlags.WindowWest) == FloorplanFlags.WindowWest)
            outFlags |= FloorplanFlags.WindowSouth;


        if ((original & FloorplanFlags.ConnectionEast) == FloorplanFlags.ConnectionEast)
            outFlags |= FloorplanFlags.ConnectionNorth;

        if ((original & FloorplanFlags.ConnectionNorth) == FloorplanFlags.ConnectionNorth)
            outFlags |= FloorplanFlags.ConnectionWest;

        if ((original & FloorplanFlags.ConnectionSouth) == FloorplanFlags.ConnectionSouth)
            outFlags |= FloorplanFlags.ConnectionEast;

        if ((original & FloorplanFlags.ConnectionWest) == FloorplanFlags.ConnectionWest)
            outFlags |= FloorplanFlags.ConnectionSouth;

        return outFlags;
    }

    public static FloorplanFlags Rotate180(this FloorplanFlags original)
    {
        FloorplanFlags outFlags = original & (FloorplanFlags.Nothing | FloorplanFlags.Ceiling | FloorplanFlags.Floor);

        if ((original & FloorplanFlags.DoorEast) == FloorplanFlags.DoorEast)
            outFlags |= FloorplanFlags.DoorWest;

        if ((original & FloorplanFlags.DoorNorth) == FloorplanFlags.DoorNorth)
            outFlags |= FloorplanFlags.DoorSouth;

        if ((original & FloorplanFlags.DoorSouth) == FloorplanFlags.DoorSouth)
            outFlags |= FloorplanFlags.DoorNorth;

        if ((original & FloorplanFlags.DoorWest) == FloorplanFlags.DoorWest)
            outFlags |= FloorplanFlags.DoorEast;


        if ((original & FloorplanFlags.WallEast) == FloorplanFlags.WallEast)
            outFlags |= FloorplanFlags.WallWest;

        if ((original & FloorplanFlags.WallNorth) == FloorplanFlags.WallNorth)
            outFlags |= FloorplanFlags.WallSouth;

        if ((original & FloorplanFlags.WallSouth) == FloorplanFlags.WallSouth)
            outFlags |= FloorplanFlags.WallNorth;

        if ((original & FloorplanFlags.WallWest) == FloorplanFlags.WallWest)
            outFlags |= FloorplanFlags.WallEast;


        if ((original & FloorplanFlags.WindowEast) == FloorplanFlags.WindowEast)
            outFlags |= FloorplanFlags.WindowWest;

        if ((original & FloorplanFlags.WindowNorth) == FloorplanFlags.WindowNorth)
            outFlags |= FloorplanFlags.WindowSouth;

        if ((original & FloorplanFlags.WindowSouth) == FloorplanFlags.WindowSouth)
            outFlags |= FloorplanFlags.WindowNorth;

        if ((original & FloorplanFlags.WindowWest) == FloorplanFlags.WindowWest)
            outFlags |= FloorplanFlags.WindowEast;


        if ((original & FloorplanFlags.ConnectionEast) == FloorplanFlags.ConnectionEast)
            outFlags |= FloorplanFlags.ConnectionWest;

        if ((original & FloorplanFlags.ConnectionNorth) == FloorplanFlags.ConnectionNorth)
            outFlags |= FloorplanFlags.ConnectionSouth;

        if ((original & FloorplanFlags.ConnectionSouth) == FloorplanFlags.ConnectionSouth)
            outFlags |= FloorplanFlags.ConnectionNorth;

        if ((original & FloorplanFlags.ConnectionWest) == FloorplanFlags.ConnectionWest)
            outFlags |= FloorplanFlags.ConnectionEast;

        return outFlags;
    }

    public static FloorplanFlags FlipHorizontal(this FloorplanFlags original)
    {
        FloorplanFlags outFlags = original & (FloorplanFlags.Nothing | FloorplanFlags.Ceiling | FloorplanFlags.Floor | FloorplanFlags.DoorNorth | FloorplanFlags.DoorSouth | FloorplanFlags.ConnectionNorth | FloorplanFlags.ConnectionSouth | FloorplanFlags.WallNorth | FloorplanFlags.WallSouth | FloorplanFlags.WindowNorth | FloorplanFlags.WindowSouth);

        if ((original & FloorplanFlags.DoorEast) == FloorplanFlags.DoorEast)
            outFlags |= FloorplanFlags.DoorWest;

        if ((original & FloorplanFlags.DoorWest) == FloorplanFlags.DoorWest)
            outFlags |= FloorplanFlags.DoorEast;


        if ((original & FloorplanFlags.WallEast) == FloorplanFlags.WallEast)
            outFlags |= FloorplanFlags.WallWest;

        if ((original & FloorplanFlags.WallWest) == FloorplanFlags.WallWest)
            outFlags |= FloorplanFlags.WallEast;


        if ((original & FloorplanFlags.WindowEast) == FloorplanFlags.WindowEast)
            outFlags |= FloorplanFlags.WindowWest;

        if ((original & FloorplanFlags.WindowWest) == FloorplanFlags.WindowWest)
            outFlags |= FloorplanFlags.WindowEast;


        if ((original & FloorplanFlags.ConnectionEast) == FloorplanFlags.ConnectionEast)
            outFlags |= FloorplanFlags.ConnectionWest;

        if ((original & FloorplanFlags.ConnectionWest) == FloorplanFlags.ConnectionWest)
            outFlags |= FloorplanFlags.ConnectionEast;

        return outFlags;
    }

    public static FloorplanFlags FlipVertical(this FloorplanFlags original)
    {
        FloorplanFlags outFlags = original & (FloorplanFlags.Nothing | FloorplanFlags.Ceiling | FloorplanFlags.Floor | FloorplanFlags.DoorEast | FloorplanFlags.DoorWest | FloorplanFlags.ConnectionEast | FloorplanFlags.ConnectionWest | FloorplanFlags.WallEast | FloorplanFlags.WallWest | FloorplanFlags.WindowEast | FloorplanFlags.WindowWest);

        if ((original & FloorplanFlags.DoorNorth) == FloorplanFlags.DoorNorth)
            outFlags |= FloorplanFlags.DoorSouth;

        if ((original & FloorplanFlags.DoorSouth) == FloorplanFlags.DoorSouth)
            outFlags |= FloorplanFlags.DoorNorth;


        if ((original & FloorplanFlags.WallNorth) == FloorplanFlags.WallNorth)
            outFlags |= FloorplanFlags.WallSouth;

        if ((original & FloorplanFlags.WallSouth) == FloorplanFlags.WallSouth)
            outFlags |= FloorplanFlags.WallNorth;


        if ((original & FloorplanFlags.WindowNorth) == FloorplanFlags.WindowNorth)
            outFlags |= FloorplanFlags.WindowSouth;

        if ((original & FloorplanFlags.WindowSouth) == FloorplanFlags.WindowSouth)
            outFlags |= FloorplanFlags.WindowNorth;


        if ((original & FloorplanFlags.ConnectionNorth) == FloorplanFlags.ConnectionNorth)
            outFlags |= FloorplanFlags.ConnectionSouth;

        if ((original & FloorplanFlags.ConnectionSouth) == FloorplanFlags.ConnectionSouth)
            outFlags |= FloorplanFlags.ConnectionNorth;

        return outFlags;
    }

    public static bool HasAllFlags(this FloorplanFlags original, FloorplanFlags flags)
    {
        return (original & flags) == flags;
    }

    public static bool HasNoFlags(this FloorplanFlags original, FloorplanFlags flags)
    {
        return (original & flags) == 0;
    }

    public static bool HasAnyFlags(this FloorplanFlags original, FloorplanFlags flags)
    {
        return !original.HasNoFlags(flags);
    }
}

public static class GameSettings
{
    public static float ActivateDistance = 2.0f;
    public static float BaseStaminaRegen = 1f;
    public static float BaseSprintDrain = 1f;
    public static float BaseSprintTime = 2f;
    public static float BaseSprintMult = 1f;
    public static float BaseSprintExponent = 1.3f;

    public static int ItemCountMin = 1;
    public static int ItemCountMax = 5;
    public static int ItemCountMean = 2;
    private static float artifactGenerationStartPoint = 0f;

    public static float SearchTime = 0.1f;

	public static float chanceToRepeatTrack = 0.25f;

    public static float ArtifactGenerationChance
    {
        get 
        {
            var interactManager = Utility.ServerController.GetComponent<interactManager>();
            float containerCount = interactManager.containerList.Length;
            float openedCount = (containerCount - interactManager.containerIDList.Count) + Container.Artifacts.Count;

            float progressPercent = Mathf.Pow(3, artifactGenerationStartPoint + ( openedCount / containerCount)) - 2;

            return Mathf.Clamp(progressPercent, 0, 1);
        }
    }
}
