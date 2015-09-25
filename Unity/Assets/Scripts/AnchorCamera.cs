using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class AnchorCamera : MonoBehaviour
    {
        public GameObject anchorObject;

        public float xOffset;
        public float yOffset;
        public float zOffset;

        public void Update()
        {
            transform.position = anchorObject.transform.position + (transform.parent.transform.rotation * new Vector3(xOffset, yOffset, zOffset));
            
        }
    }
}
