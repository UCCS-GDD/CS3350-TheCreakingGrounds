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

        public override void Start()
        {
            base.Start();
            Instance = gameObject;
        }

        public override void Update()
        {
            base.Update();

            //get reticle target
            RaycastHit info;
            Physics.Raycast(gameObject.transform.position, gameObject.transform.FindChild("PlayerCamera").rotation.eulerAngles, out info, 2.0f);
            ReticleInfo = info;
        }
    }
}
