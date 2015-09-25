using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class CurrentPlayer : Player
    {
        public static GameObject Instance;

        public RaycastHit ReticleInfo { get; protected set; }

        public GameObject reticleObject;



        public override void Start()
        {
            base.Start();
            Instance = gameObject;
        }

        public override void Update()
        {
            base.Update();

            GetReticleTarget();
            GetMovementInput();
        }

        private void GetReticleTarget()
        {
            var camera = gameObject.transform.FindChild("PlayerCamera");

            Ray ray = new Ray(camera.transform.position, camera.transform.rotation * Vector3.forward);

            List<RaycastHit> rayHits = new List<RaycastHit>(Physics.RaycastAll(ray).Where(h => h.collider.gameObject != gameObject && h.distance <= 2.0f).OrderBy(h => h.distance));

            ReticleInfo = rayHits.FirstOrDefault();

            if (ReticleInfo.collider != null)
            {
                if (ReticleInfo.collider.gameObject != reticleObject)
                {
                    reticleObject = ReticleInfo.collider.gameObject;
                    Debug.Log(ReticleInfo.collider.transform.name);
                }
            }
            else
            {
                reticleObject = null;
            }


            if (reticleObject != null && Input.GetButtonDown("Activate") && reticleObject.GetComponent<Assets.Scripts.Acitvator>() != null)
                reticleObject.GetComponent<Assets.Scripts.Acitvator>().OnActivate();
        }

        private void GetMovementInput()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");


        }
    }
}
