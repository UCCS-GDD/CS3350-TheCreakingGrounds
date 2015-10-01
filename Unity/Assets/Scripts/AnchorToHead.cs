using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class AnchorToHead : MonoBehaviour
    {
        public GameObject anchorObject;

        public float xOffset;
        public float yOffset;
        public float zOffset;

        public void Update()
        {
            transform.position = anchorObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).position
                + (anchorObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).rotation * new Vector3(xOffset, yOffset, zOffset));
        }
    }
}
