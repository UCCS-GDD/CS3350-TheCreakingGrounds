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

        private Transform headTrans;

        public void Update()
        {
            headTrans = anchorObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
            transform.position = headTrans.position + Quaternion.Euler(headTrans.eulerAngles.x, transform.eulerAngles.y, headTrans.eulerAngles.z) * new Vector3(xOffset, yOffset, zOffset);
        }
    }
}
