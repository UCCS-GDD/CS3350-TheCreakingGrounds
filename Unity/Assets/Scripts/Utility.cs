using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utility
    {
        public static GameObject GetParentActivator(this GameObject gObj)
        {
            GameObject curObj = gObj;

            while (curObj.GetComponent<Activator>() == null)
            {
                if (curObj.transform.parent != null)
                    curObj = curObj.transform.parent.gameObject;
                else
                    return null;
            }

            return curObj;
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
        public static int ItemCountMax = 100;
        public static int ItemCountMean = 99;
        public static float ArtifactGenerationChance = 0.05f;
    }
}
