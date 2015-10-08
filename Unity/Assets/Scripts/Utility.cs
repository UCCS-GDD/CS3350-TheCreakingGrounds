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
}
