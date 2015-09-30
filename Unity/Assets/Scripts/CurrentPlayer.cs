using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts
{
    class CurrentPlayer : Player
    {
        public static GameObject Instance;
        public RaycastHit ReticleInfo { get; protected set; }

        [NonSerialized]
        public GameObject reticleObject;

        [NonSerialized]
        public float headRotate;

        [NonSerialized]
        public float headPivot;

        [NonSerialized]
        public float camPivot;

        public Camera cam;
        public Animator animationController;
        public Transform headRotateTransform;
        public float headClampX = 60f;
        public float headClampY = 45f;
        public float camClampY = 85f;

        private Quaternion headHolder = new Quaternion();

        public override void Start()
        {
            base.Start();
            Instance = gameObject;
        }

        public override void Update()
        {
            base.Update();

            DoMovement();
            DoMouseLook();
            GetReticleTarget();
        }

        private void GetReticleTarget()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.rotation * Vector3.forward);

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

        private void DoMovement()
        {
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };

            if (input.y > 0)
            {
                animationController.SetInteger("animDirection", 1);

                transform.Rotate(0f, headRotate, 0f, Space.World);
                headRotate = 0;

                if (Input.GetAxis("Sprint") > 0)
                    animationController.SetInteger("animSpeed", 4);
                else
                    animationController.SetInteger("animSpeed", (int)(input.y * 3.5));
            }
            else
            {
                animationController.SetInteger("animSpeed", 0);
                animationController.SetInteger("animDirection", 0);
            }
        }

        private void DoMouseLook()
        {
            Vector3 input = new Vector3
            {
                x = Input.GetAxis("Look X"),
                y = Input.GetAxis("Look Y"),
                z = 0
            };

            headRotate += input.x;
            camPivot = Mathf.Clamp(camPivot + input.y, -camClampY, camClampY);

            if (Math.Abs(headRotate) > headClampX)
            {
                float bodyX;
                if (headRotate < 0)
                {
                    bodyX = headRotate + headClampX;
                    headRotate = -headClampX;
                }
                else
                {
                    bodyX = headRotate - headClampX;
                    headRotate = headClampX;
                }
                
                transform.Rotate(0f, bodyX, 0f, Space.World);
            }

            if (Math.Abs(camPivot) > headClampY)
            {
                float camY;
                if (camPivot < 0)
                {
                    camY = camPivot + headClampY;
                    headPivot = -headClampY;
                }
                else
                {
                    camY = camPivot - headClampY;
                    headPivot = headClampY;
                }

                cam.transform.localRotation = Quaternion.identity;
                cam.transform.Rotate(-camY, 0f, 0f, Space.Self);
            }
            else
            {
                headPivot = camPivot;
                cam.transform.localRotation = Quaternion.identity;
           }

            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);

            headRotateTransform.rotation = Quaternion.identity;

            headRotateTransform.Rotate(0f, headRotate, 0f, Space.Self);
            headRotateTransform.Rotate(-headPivot, 0f, 0f, Space.World);

            animationController.Play("HeadRotate", -1, headRotateTransform.rotation.eulerAngles.y / 360f);
            animationController.Play("HeadPivot", -1, -headRotateTransform.rotation.eulerAngles.x / 360f);
            animationController.Play("HeadTilt", -1, headRotateTransform.rotation.eulerAngles.z / 360f);
        }

        private Quaternion ClampHeadRotation(Quaternion headRot)
        {
            float x = headRot.eulerAngles.x;
            float y = headRot.eulerAngles.y;
            float z = headRot.eulerAngles.z;

            return headRot;
        }
    }
}
