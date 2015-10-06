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
        public float headClampX = 90f;
        public float headClampY = 45f;
        public float camClampY = 85f;

        public GameObject debugCameraObj;

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
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0));
            //Ray ray = new Ray(headRotateTransform.position, new Vector3(0, camPivot, 0));
            //Ray ray = new Ray(headRotateTransform.position, headRotateTransform.rotation * Vector3.forward);
            //Ray ray = new Ray(headRotateTransform.position, Quaternion.Euler(cam.transform.eulerAngles.x + headRotateTransform.eulerAngles.x, cam.transform.eulerAngles.y + transform.eulerAngles.y, cam.transform.eulerAngles.z) * Vector3.forward);

            //Debug.DrawRay(ray.origin, ray.direction);
            //Debug.Log(String.Format("{0}, {1}, {2}", cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z));

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

            if (input.magnitude == 0)
            {
                animationController.SetInteger("animSpeed", 0);
                animationController.SetInteger("animDirection", 0);

                //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                return;
            }

            float direction;

            if (input.y >= 0)
                direction = Mathf.Rad2Deg * Mathf.Atan2(input.y, input.x) - 90;
            else
                direction = Mathf.Rad2Deg * Mathf.Atan2(-input.y, -input.x) - 90;

            //Debug.Log(String.Format("{0}, {1}, {2}", input.x, input.y, direction));

            transform.Rotate(0f, headRotate - direction, 0f, Space.World);
            headRotate = direction;

            if (input.y >= 0)
            {
                animationController.SetInteger("animDirection", 1);
                if (input.y >= Mathf.Abs(input.x))
                {
                    if (Input.GetAxis("Sprint") > 0)
                        animationController.SetInteger("animSpeed", 4);
                    else
                        animationController.SetInteger("animSpeed", (int)(input.magnitude * 3.5));
                }
                else
                {
                    if (Input.GetAxis("Sprint") > 0)
                        animationController.SetInteger("animSpeed", 3);
                    else
                        animationController.SetInteger("animSpeed", (int)(input.magnitude * 2.5));
                }
            }
            else
            {
                animationController.SetInteger("animDirection", -1);
                animationController.SetInteger("animSpeed", 1);
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
            }
            else
            {
                headPivot = camPivot;
            }

            cam.transform.localRotation = Quaternion.identity;
            cam.transform.Rotate(-camPivot, 0f, 0f, Space.Self);
            cam.transform.Rotate(0, headRotate, 0, Space.World);

            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);

            headRotateTransform.rotation = Quaternion.identity;

            headRotateTransform.Rotate(0f, headRotate, 0f, Space.Self);
            headRotateTransform.Rotate(-headPivot, 0f, 0f, Space.World);

            animationController.Play("HeadRotate", -1, headRotateTransform.rotation.eulerAngles.y / 360f);
            animationController.Play("HeadPivot", -1, -headRotateTransform.rotation.eulerAngles.x / 360f);
            animationController.Play("HeadTilt", -1, headRotateTransform.rotation.eulerAngles.z / 360f);
        }
    }
}
