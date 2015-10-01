using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class CameraFollow : MonoBehaviour
    {
        public GameObject followObject;

        public float xOffset;
        public float yOffset;
        public float zOffset;

        public void Update()
        {
            transform.position = followObject.transform.position
                + followObject.transform.rotation * new Vector3(xOffset, yOffset, zOffset);
        }
    }
}
