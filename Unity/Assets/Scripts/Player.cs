using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Assets.Scripts.Menu;
using Assets.Scripts.Items;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Player : NetworkBehaviour
    {
        public static Player Instance;
        public static List<Player> allPlayers = new List<Player>();

        //Debug Stuff
        public Text debugText;

        //base stats
        public Stat Brawn;
        public Stat Speed;
        public Stat Intellect;
        public Stat Willpower;

        //vital stats
        public Stat Wounds;
        public Stat Traumas;
        int lastWounds;
        int lastTraumas;

        //character perk
        public List<Perk> Perks;

        //inventory
        public Dictionary<InventoryItem, int> Inventory = new Dictionary<InventoryItem,int>();

        //effects
        public Dictionary<Effect, float> Effects = new Dictionary<Effect, float>();

        //derrived stats
        [NonSerialized]
        public Stat Stamina;

        [NonSerialized]
        public InventoryItem EquippedItem1;
        [NonSerialized]
        public InventoryItem EquippedItem2;

        public GameObject serverManagement;

        //
        //below is movement and state info vars
        //
        public RaycastHit ReticleInfo { get; protected set; }

        [NonSerialized]
        public GameObject reticleObject;

        [NonSerialized]
        public float headRotate;

        [NonSerialized]
        public float headPivot;

        [NonSerialized]
        public float camPivot;

        public bool isDoll = false;
        public bool isInMenu = false;

        public Camera cam;
        public Animator animationController;
        public Transform headRotateTransform;
        public float headClampX = 90f;
        public float headClampY = 45f;
        public float camClampY = 85f;

        private Quaternion headHolder = new Quaternion();

        public UIManager UIPrefab;

        [NonSerialized]
        public UIManager UI;

        public bool IsSprinting { get; protected set; }
        public bool IsWinded { get; protected set; }
        public bool ReadyForCreakening { get; protected set; }
        public bool IsDead { get; protected set; }

        public void ShowMouse()
        {
            gameObject.GetComponent<disableMouse>().ShowCursor();
            isInMenu = true;
        }

        public void HideMouse()
        {
            gameObject.GetComponent<disableMouse>().HideCursor();
            isInMenu = false;
        }

        public virtual void Start()
        {
            if (isDoll)
                return;

            sbyte stamina = (sbyte)Mathf.RoundToInt(Mathf.Pow((Speed.BaseValue * GameSettings.BaseSprintMult), (GameSettings.BaseSprintExponent)) * GameSettings.BaseSprintTime);
            Stamina = new Stat(stamina);
            lastTraumas = Traumas.CurrentValue;
            lastWounds = Wounds.CurrentValue;

            //Find Debug Text
            debugText = GameObject.Find("DebugText").GetComponent<Text>();
            debugText.text = "Found Debug";

            //Delayed start setup
            StartCoroutine(delayedStart());
        }

        IEnumerator delayedStart()
        {
            yield return new WaitForSeconds(2f);

            CmdUpdateStats(Brawn.ToDataString(), Speed.ToDataString(), Intellect.ToDataString(), Willpower.ToDataString(),
                Wounds.ToDataString(), Traumas.ToDataString(), gameObject.transform.FindChild("Model").GetChild(0).gameObject.name.Replace("(Clone)", ""));
            //serverManagement.GetComponent<awakenManager>().setHandle( gameObject );
        }

        public virtual void Update()
        {
            if (isDoll || IsDead)
                return;

            DoMovement();
            DoMouseLook();
            GetReticleTarget();
            CheckForDamage();
            UpdateEffects();

            if (Input.GetButtonDown("Inventory"))
            {
                if (!UI.statusPanel.gameObject.activeSelf)
                {
                    ShowMouse();
                    UI.statusPanel.gameObject.SetActive(true);
                    UI.statusPanel.InitializeMenu(this);
                }
                else
                {
                    UI.statusPanel.gameObject.SetActive(false);
                    HideMouse();
                }
            }

            if (!IsWinded && Stamina <= 0)
                IsWinded = true;

            if (IsWinded && Stamina.Damage < 1)
                IsWinded = false;

            if (!IsSprinting)
                Stamina.RestoreStat(GameSettings.BaseStaminaRegen * Time.deltaTime);

            if (IsSprinting)
                Stamina.DamageStat(GameSettings.BaseSprintDrain * Time.deltaTime);
        }

        public void LateUpdate()
        {
            InventoryItem item;
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                do
                {
                    item = Container.PickRandomItem();
                } while (item.IsArtifact);
                AddItem(item, 1);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                do
                {
                    item = Container.PickRandomItem();
                } while (!item.IsArtifact);
                AddItem(item, 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetReticleTarget()
        {
            //Make sure the UI has been created
            if (UI == null)
            {
                UI = Instantiate(UIPrefab);
            }

            //create a ray emitting from the center of the camera
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            //create a list of objects 
            List<RaycastHit> rayHits = new List<RaycastHit>(Physics.RaycastAll(ray).Where(h => 
                {
                    //restrict ray hits such that they exclude the current player, are less than the activation distancs from the current player, and have an Activator in their parent hierarchy
                    return (h.collider.gameObject != gameObject && h.distance <= GameSettings.ActivateDistance && h.collider.gameObject.GetParentActivator() != null);
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
                    UI.ReticleSprite.color = Color.green;
                    Debug.Log(targetActivator.name);

                    //Display object in DebugText
                    debugText.text = targetActivator.name;
                }
            }
            else
            {
                reticleObject = null;
                UI.ReticleSprite.color = Color.white;

                //Set debug text to nothing (Hide it)
                debugText.text = "";
            }

            //activate targeted object
            if (!isInMenu && reticleObject != null && Input.GetButtonDown("Activate") && reticleObject.GetComponent<Assets.Scripts.Activator>() != null)
                if (reticleObject.GetComponent<Assets.Scripts.Activator>() is Container)
                {
                    //StartCoroutine(OpeningChest(reticleObject));
                    string chestName = reticleObject.transform.name;
                    string user = gameObject.name;

                    CmdTellServerWhichChestWasActivated(chestName, user);
                }
                else
                {
                    string uIdenity = reticleObject.transform.name;
                    string meIdenity = gameObject.name;
                    CmdTellServerWhichDoorWasActivated(uIdenity, meIdenity);
                    //reticleObject.GetComponent<Assets.Scripts.Activator>().OnActivate(this);
                }
        }

        [Command]
        void CmdTellServerWhichDoorWasActivated(string uniqueID, string userID)
        {
            GameObject go = GameObject.Find(uniqueID);
            Player user = GameObject.Find(userID).GetComponent<Player>();
            go.GetComponent<Assets.Scripts.Activator>().OnActivate(user);
        }

        [Command]
        void CmdTellServerWhichChestWasActivated(string chestName, string userName)
        {
            //Find Server Manager
            serverManagement = GameObject.Find("ServerManagement");

            Debug.Log("Activated " + chestName + " for " + userName); 

            GameObject chest = GameObject.Find(chestName);

            uint chestID = chest.GetComponent<NetworkIdentity>().netId.Value;

            if (serverManagement.GetComponent<interactManager>().containerIDList.Contains(chestID))
            {
                string items = String.Join(",", Container.GenerateInventory().Select(kvp =>
                    {
                        return kvp.Key.Name + "_" + kvp.Value.ToString();
                    }).ToArray());

                //Send every user the command
                RpcOpenChestClient(userName, chestName, items);

                //Remove the container from the ID list so it won't open again
                serverManagement.GetComponent<interactManager>().containerIDList.Remove(chestID);
            }

            else
            {
                //RpcChestEmpty(userName); DISABLED, CAUSES ERROR AT DEBUG.TEXT
            }
        }

        [ClientRpc]
        public void RpcOpenChestClient(string userName, string chestName, string itemsDict)
        {
            Debug.Log("RpcOpenChestClient: " + userName + "  " + chestName);

            //If you're the player
            if(gameObject.name.CompareTo(userName) == 0)
            {
                string[] itemCounts = itemsDict.Split(',');
                var items = new Dictionary<InventoryItem, int>();
                foreach(string kvp in itemCounts)
                {
                    InventoryItem item = InventoryItem.Parse(kvp.Split('_')[0]);
                    int count = int.Parse(kvp.Split('_')[1]);

                    items.Add(item, count);
                }
                GameObject chest = GameObject.Find(chestName);
                if (reticleObject == chest)
                {
                    UI.searchPanel.gameObject.SetActive(true);
                    UI.searchPanel.ShowSearchMenu(items, this);
                }
            }
        }

        [ClientRpc]
        public void RpcChestEmpty(string userName)
        {
            Debug.Log("RpcChestEmpty: " + userName);

            //If you're the player
            if (gameObject.name.CompareTo(userName) == 0)
            {
                debugText.text = "Opened";
            }
        }

        public void startAwakening()
        {
            string user = gameObject.name;

            CmdStartAwakening(user);
        }

        [Command]
        public void CmdStartAwakening(string cursedPlayerName)
        {
            //Find Server Manager
            serverManagement = GameObject.Find("ServerManagement");

            serverManagement.GetComponent<awakenManager>().cursedPlayerName = cursedPlayerName;
            string curseName = serverManagement.GetComponent<awakenManager>().curseName;

            RpcStartAwakening(cursedPlayerName, curseName);
        }

        [ClientRpc]
        public void RpcStartAwakening(string cursedPlayer, string curseName)
        {
            GetComponent<GibberingMadness>().StartCurse();
            GameObject smoke = Instantiate(GetComponent<GibberingMadness>().gibberingMadnessVia);
            smoke.transform.SetParent(transform, false);

            //If I started the curse
            if (enabled)
            {
                Debug.Log("This Client Stared The Awakening");
            }

            //If I didn't start the curse
            else
            {
                Debug.Log("This Client Didn't Stared The Awakening");

                GameObject voices = Instantiate(GetComponent<GibberingMadness>().gibberingMadnessSound);

                voices.transform.SetParent(transform, false);
            }

            Player.Instance.UI.creakeningPanel.ShowCreakeningPanel(cursedPlayer, GibberingMadness.curseName, GibberingMadness.curseBriefing, GibberingMadness.playerBriefing);

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
            if (input.magnitude == 0 || isInMenu)
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

            //determine if player is sprinting
            IsSprinting = Input.GetAxis("Sprint") > 0 && !IsWinded;

            //player is standing still or moving forward
            if (input.y >= 0)
            {
                animationController.SetInteger("animDirection", 1);

                //Player is moving more forward than sideways, animate as moving forward at an angle
                if (input.y >= Mathf.Abs(input.x))
                {
                    if (IsSprinting)
                        animationController.SetInteger("animSpeed", 3);
                    else
                        animationController.SetInteger("animSpeed", (int)(input.magnitude * 1.5f));
                }
                //player is moving more sideways than forward, animate as strafing
                else
                {
                    if (IsSprinting)
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
            if (isInMenu)
                return;

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

        void CheckForDamage()
        {
            if (Wounds.CurrentValue < lastWounds)
                StartCoroutine(FadeAlpha(UI.woundFlash));

            if (Traumas.CurrentValue < lastTraumas)
                StartCoroutine(FadeAlpha(UI.traumaFlash));

            lastTraumas = Traumas.CurrentValue;
            lastWounds = Wounds.CurrentValue;

            if (Traumas.CurrentValue <= 0 || Wounds.CurrentValue <= 0)
            {
                CmdIDied();
            }

        }

        [Command]
        public void CmdIDied()
        {
            RpcIDied();
        }

        [ClientRpc]
        public void RpcIDied()
        {
            IsDead = true;
            gameObject.GetComponent<Collider>().enabled = false;
            cam.GetComponent<AnchorToHead>().enabled = false;
            Transform models = gameObject.transform.FindChild("Model");
            for (int i = 0; i < models.childCount; i++)
            {
                var child = models.GetChild(i);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            var gibbering = gameObject.GetComponent<GibberingMadness>();
            if (gibbering.isCursed)
            {
                if (Player.Instance == this)
                {
                    Player.Instance.UI.outcomePanel.ShowOutcome("You've died. The survivors have won!");
                }
                else
                {
                    GameObject.Destroy(gameObject.transform.FindChild("GibberingMadnessAudio(Clone)").gameObject);
                    GameObject.Destroy(gameObject.transform.FindChild("GibberingMadnessVisuals(Clone)").gameObject);
                    Player.Instance.UI.outcomePanel.ShowOutcome("The Gibbering Madness has been defeated! Congratulations, you are victorious!");
                }
            }
            else
            {
                if (Player.Instance == this)
                {
                    UI.outcomePanel.ShowOutcome("You've died. With luck your fellow survivors will not suffer the same fate.");
                }
                else
                {
                    gibbering = Player.Instance.gameObject.GetComponent<GibberingMadness>();
                    if (gibbering.isCursed && Player.allPlayers.Count(p => !p.IsDead) <= 1)
                        Player.Instance.UI.outcomePanel.ShowOutcome("All of the survivors have been defeated! Congratulations, you are victorious!");
                }
            }

        }
        

        [Command]
        public void CmdUpdateStats(string BrawnData, string SpeedData, string IntellectData, string WillpowerData, string WoundsData, string TraumasData, string Model)
        {
            RpcUpdateStats(BrawnData, SpeedData, IntellectData, WillpowerData, WoundsData, TraumasData, Model);
        }

        [ClientRpc]
        public void RpcUpdateStats(string BrawnData, string SpeedData, string IntellectData, string WillpowerData, string WoundsData, string TraumasData, string Model)
        {
            if (enabled)
                return;

            Brawn = Stat.Parse(BrawnData);
            Speed = Stat.Parse(SpeedData);
            Intellect = Stat.Parse(IntellectData);
            Willpower = Stat.Parse(WillpowerData);
            Wounds = Stat.Parse(WoundsData);
            Traumas = Stat.Parse(TraumasData);

            Transform models = gameObject.transform.FindChild("Model");
            if (models.transform.FindChild(Model) == null)
            {
                for (int i = 0; i < models.childCount; i++)
                {
                    var child = models.GetChild(i);
                    child.SetParent(null);
                    Destroy(child.gameObject);
                }
                GameObject model = Instantiate(Resources.Load<GameObject>("CharacterModels/Models/" + Model));
                model.transform.SetParent(models, false); 
                
                gameObject.GetComponent<Animator>().Rebind();
            }
        }

        [ClientRpc]
        public void RpcTakeDamage(float damageAmount, string stat)
        {
            switch(stat)
            {
                case "Traumas":
                    Traumas.DamageStat(damageAmount);
                    break;
                case "Wounds":
                    Wounds.DamageStat(damageAmount);
                    break;
                default:
                    Debug.Log("Unknown stat: " + stat);
                    break;
            }
        }


        [Command]
        public void CmdReadyForCreakening()
        {
            RpcReadyForCreakening();
        }

        [ClientRpc]
        public void RpcReadyForCreakening()
        {
            ReadyForCreakening = true;

            if (Player.allPlayers.All(p => p.ReadyForCreakening))
            {
                CmdAllReadyForCreakening();
            }
        }


        [Command]
        public void CmdAllReadyForCreakening()
        {
            RpcAllReadyForCreakening();
        }

        [ClientRpc]
        public void RpcAllReadyForCreakening()
        {
            Player.Instance.UI.creakeningPanel.HidePanel();
        }

        public void AddEffect(Effect effect)
        {
            if (Effects.ContainsKey(effect))
            {
                if (effect.Duration > 0)
                {
                    Effects[effect] += effect.Duration;
                }
                else
                {
                    Effects[effect] -= 1;
                }
            }
            else
            {
                if (effect.Duration < 0)
                    Effects.Add(effect, -1);
                else
                    Effects.Add(effect, effect.Duration + Time.time);
                effect.OnAdd(this);
            }

            if (!isDoll)
            {
                CmdUpdateStats(Brawn.ToDataString(), Speed.ToDataString(), Intellect.ToDataString(), Willpower.ToDataString(),
                    Wounds.ToDataString(), Traumas.ToDataString(), gameObject.transform.FindChild("Model").GetChild(0).gameObject.name.Replace("(Clone)", ""));
            }
        }

        public void RemoveEffect(Effect effect)
        {
            Effects.Remove(effect);
            effect.OnRemove(this);


            if (!isDoll)
            {
                CmdUpdateStats(Brawn.ToDataString(), Speed.ToDataString(), Intellect.ToDataString(), Willpower.ToDataString(),
                    Wounds.ToDataString(), Traumas.ToDataString(), gameObject.transform.FindChild("Model").GetChild(0).gameObject.name.Replace("(Clone)", ""));
            }
        }

        void UpdateEffects()
        {
            foreach (var kvp in Effects.Where(kvp => kvp.Value < 0 || kvp.Value >= Time.time))
            {
                Effect effect = kvp.Key;
                float endTime = kvp.Value;

                effect.OnUpdate(this);
            }
            foreach (var kvp in Effects.Where(kvp => kvp.Value > 0 && kvp.Value < Time.time))
            {
                Effect effect = kvp.Key;
                float endTime = kvp.Value;

                RemoveEffect(effect);
            }
        }

        IEnumerator FadeAlpha(Image image)
        {
            image.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            image.gameObject.SetActive(false);
        }

        public void OnFootStep()
        {
            GetComponent<AudioSource>().Play();
        }

        public void AddItems(Dictionary<InventoryItem, int> inventory)
        {
            foreach (var kvp in inventory)
            {
                AddItem(kvp.Key, kvp.Value);
            }
        }

        public void AddItem(InventoryItem item, int count)
        {
            if (item.IsJunk)
                return;

            if (Inventory.ContainsKey(item))
                Inventory[item] += count;
            else
                Inventory.Add(item, count);

            if (item.IsArtifact)
            {
                startAwakening();
            }
        }
    }

    public enum PlayerStat
    {
        Brawn,
        Speed,
        Intellect,
        Willpower,
        Wounds,
        Traumas
    }
}
