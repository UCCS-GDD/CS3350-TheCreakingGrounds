using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class TriggerGhost : MonoBehaviour
    {
        bool isTriggered = false;
        float t = 0.0f;

        public GameObject Ghost;

        Vector3 lerpStart;

        public Vector3 lerpEnd;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == CurrentPlayer.Instance)
                isTriggered = true;
        }

        public void Update()
        {
            if (isTriggered && t <= 1)
            {
                Ghost.transform.position = Vector3.Lerp(lerpStart, lerpEnd, t);
                t += Time.deltaTime;
            }
        }
    }
}
