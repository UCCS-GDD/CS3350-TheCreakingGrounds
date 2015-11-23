using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
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

            while (curObj.GetComponent<Activator>() == null)
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
        private static float artifactGenerationStartPoint = 0.1f;

        public static float SearchTime = 0.1f;

		public static float chanceToRepeatTrack = 0.25f;

        public static float ArtifactGenerationChance
        {
            get 
            {
                var interactManager = Utility.ServerController.GetComponent<interactManager>();
                float containerCount = interactManager.containerList.Length;
                float openedCount = (containerCount - interactManager.containerIDList.Count) + Container.Artifacts.Count;

                float progressPercent = artifactGenerationStartPoint + openedCount / containerCount;

                return Mathf.Clamp(progressPercent, 0, 1);
            }
        }
    }
}
