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

        /// <summary>
        /// 
        /// </summary>
        private void GetReticleTarget()
        {
            //create a ray emitting from the center of the camera
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            //create a list of objects 
            List<RaycastHit> rayHits = new List<RaycastHit>(Physics.RaycastAll(ray).Where(h => 
                {
                    //restrict ray hits such that they exclude the current player, are less than 2 game units from the current player, and have an Activator in their parent hierarchy
                    return (h.collider.gameObject != gameObject && h.distance <= 2.0f && h.collider.gameObject.GetParentActivator() != null);
                })
                //sort ray hits by distance from the player
                .OrderBy(h => h.distance));

            //choose the nearest ray hit as THE reticle object
            ReticleInfo = rayHits.FirstOrDefault();

            //make sure the chosen hit has a collider, otherwise return
            if (ReticleInfo.collider != null)
            {
                GameObject targetActivator = ReticleInfo.collider.gameObject.GetParentActivator();

                //if the targeted activator is different from the current targeted activator, update it and post a message of the new activator
                if (targetActivator != reticleObject)
                {
                    reticleObject = targetActivator;
                    Debug.Log(targetActivator.name);
                }
            }
            else
            {
                reticleObject = null;
            }

            //activate targeted object
            if (reticleObject != null && Input.GetButtonDown("Activate") && reticleObject.GetComponent<Assets.Scripts.Activator>() != null)
                reticleObject.GetComponent<Assets.Scripts.Activator>().OnActivate();
        }

        private void DoMovement()
        {
            //get multiplatform input
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };

            //if no input is detected, reset animation vars and return
            if (input.magnitude == 0)
            {
                animationController.SetInteger("animSpeed", 0);
                animationController.SetInteger("animDirection", 0);

                return;
            }

            float direction;

            //determine movement direction,
            if (input.y >= 0)
                direction = Mathf.Rad2Deg * Mathf.Atan2(input.y, input.x) - 90;
            //reverse movement direction for backpeddaling
            else
                direction = Mathf.Rad2Deg * Mathf.Atan2(-input.y, -input.x) - 90;

            //player is moving, ensure that the body is always pointing in the direction of movement
            transform.Rotate(0f, headRotate - direction, 0f, Space.World);
            headRotate = direction;

            //player is standing still or moving forward
            if (input.y >= 0)
            {
                animationController.SetInteger("animDirection", 1);

                //Player is moving more forward than sideways, animate as moving forward at an angle
                if (input.y >= Mathf.Abs(input.x))
                {
                    if (Input.GetAxis("Sprint") > 0)
                        animationController.SetInteger("animSpeed", 3);
                    else
                        animationController.SetInteger("animSpeed", (int)(input.magnitude * 1.5f));
                }
                //player is moving more sideways than forward, animate as strafing
                else
                {
                    if (Input.GetAxis("Sprint") > 0)
                        animationController.SetInteger("animSpeed", 2);
                    else
                        animationController.SetInteger("animSpeed", (int)(input.magnitude * 1.5));
                }
            }
            //player is moving backwards
            else
            {
                animationController.SetInteger("animDirection", -1);
                animationController.SetInteger("animSpeed", 1);
            }
        }

        private void DoMouseLook()
        {
            //dude I don't even know

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
