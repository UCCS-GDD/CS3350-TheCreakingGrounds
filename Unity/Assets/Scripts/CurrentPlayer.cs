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

            GetMouseInput();
            GetReticleTarget();
            GetMovementInput();
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

        private void GetMovementInput()
        {
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };            
        }

        private void GetMouseInput()
        {
            Vector3 input = new Vector3
            {
                x = Input.GetAxis("Mouse X"),
                y = Input.GetAxis("Mouse Y"),
                z = 0
            };

            if (headRotate + input.x > headClampX)
            {
                float newX = headClampX - headRotate;
                transform.Rotate(0f, input.x - newX, 0f, Space.World);
                input.x = newX;
            }
            else if (headRotate + input.x < -headClampX)
            {
                float newX = -headClampX - headRotate;
                transform.Rotate(0f, input.x - newX, 0f, Space.World);
                input.x = newX;
            }
            else
                headRotate += input.x;

            if (headPivot + input.y > headClampY)
            {
                float newY = headClampY - headPivot;
                //cam.transform.Rotate(-input.y + newY, 0f, 0f, Space.Self);
                input.y = newY;
            }
            else if (headPivot + input.y < -headClampY)
            {
                float newY = -headClampY - headPivot;
                //cam.transform.Rotate(-input.y + newY, 0f, 0f, Space.Self);
                input.y = newY;
            }
            else
                headRotate += input.x;

            headPivot += input.y;

            headRotateTransform.rotation = headHolder;

            headRotateTransform.Rotate(0f, input.x, 0f, Space.Self);
            headRotateTransform.Rotate(-input.y, 0f, 0f, Space.World);

            headHolder = ClampHeadRotation(headRotateTransform.rotation);

            animationController.Play("HeadRotate", -1, headHolder.eulerAngles.y / 360f);
            animationController.Play("HeadPivot", -1, -headHolder.eulerAngles.x / 360f);
            animationController.Play("HeadTilt", -1, headHolder.eulerAngles.z / 360f);
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
