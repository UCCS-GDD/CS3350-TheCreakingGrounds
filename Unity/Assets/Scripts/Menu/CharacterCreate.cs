using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Assets.Scripts.Menu
{
    public class CharacterCreate : MonoBehaviour
    {
        public Player Player;
        public InputField StatPoints;
        public InputField BrawnField;
        public InputField SpeedField;
        public InputField IntellectField;
        public InputField WillpowerField;
        public InputField WoundsField;
        public InputField TraumasField;
        public Text messageText;

        int statPoints = 4;

        public Perk CurrentPerk;

        public InputField charNameField;

        private static bool disableEntry = false;

        public Button readyButton;

        public GameObject[] Models;
        int modelIndex = 0;

        bool fixAnim = false;

        public void Start()
        {
            GameObject.DontDestroyOnLoad(Player);
            UpdateDisplay();
        }



        public void UpdateDisplay()
        {
            StatPoints.text = statPoints.ToString();
            BrawnField.text = Player.Brawn.ToString();
            SpeedField.text = Player.Speed.ToString();
            IntellectField.text = Player.Intellect.ToString();
            WillpowerField.text = Player.Willpower.ToString();
            WoundsField.text = Player.Wounds.ToString();
            TraumasField.text = Player.Traumas.ToString();

            UpdateBlips(BrawnField);
            UpdateBlips(SpeedField);
            UpdateBlips(IntellectField);
            UpdateBlips(WillpowerField);
            UpdateBlips(WoundsField);
            UpdateBlips(TraumasField);
        }

        public void StatsIncremented(string stat)
        {
            switch (stat.ToLower())
            {
                case "brawn":
                    UpdateStats(BrawnField, ref Player.Brawn, Player.Brawn.CurrentValue + 1);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref Player.Speed, Player.Speed.CurrentValue + 1);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref Player.Intellect, Player.Intellect.CurrentValue + 1);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref Player.Willpower, Player.Willpower.CurrentValue + 1);
                    break;
                default:
                    return;
            }
        }

        public void StatsDecremented(string stat)
        {
            switch (stat.ToLower())
            {
                case "brawn":
                    UpdateStats(BrawnField, ref Player.Brawn, Player.Brawn.CurrentValue - 1);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref Player.Speed, Player.Speed.CurrentValue - 1);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref Player.Intellect, Player.Intellect.CurrentValue - 1);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref Player.Willpower, Player.Willpower.CurrentValue - 1);
                    break;
                default:
                    return;
            }
        }

        public void StatsEntered(string stat)
        {
            if (!disableEntry)
            {
                disableEntry = true;
                switch (stat.ToLower())
                {
                    case "brawn":
                        StatsClicked(stat + "_" + BrawnField.text);
                        break;
                    case "speed":
                        StatsClicked(stat + "_" + SpeedField.text);
                        break;
                    case "intellect":
                        StatsClicked(stat + "_" + IntellectField.text);
                        break;
                    case "willpower":
                        StatsClicked(stat + "_" + WillpowerField.text);
                        break;
                    default:
                        return;
                }
                disableEntry = false;
            }
        }

        public void StatsClicked(string stat_Value)
        {
            string stat;
            int value;
            string[] args = stat_Value.Split(new char[] { '_', ',', '.', '-', ' ' });
            stat = args[0];
            value = Mathf.Clamp(int.Parse(args[1]), 1, 10);

            switch(stat.ToLower())
            {
                case "brawn":
                    UpdateStats(BrawnField, ref Player.Brawn, value);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref Player.Speed, value);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref Player.Intellect, value);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref Player.Willpower, value);
                    break;
                default:
                    return;
            }

        }

        void UpdateStats(InputField statField, ref Stat curVal, int value)
        {
            while (curVal.CurrentValue < value && curVal.CurrentValue <= 9 && statPoints > 0)
            {
                curVal.baseVal += 1;
                statPoints -= 1;
            }
            while (curVal.CurrentValue > value && curVal.BaseValue >= 2)
            {
                curVal.baseVal -= 1;
                statPoints += 1;
            }

            StatPoints.text = statPoints.ToString();
            statField.text = curVal.CurrentValue.ToString();

            UpdateBlips(statField);
        }

        void UpdateBlips(InputField blipsInputField)
        {
            List<Button> onBlips = new List<Button>(blipsInputField.transform.parent.transform.FindChild("Blips").GetComponentsInChildren<Button>().Where(b => int.Parse(b.gameObject.name) <= int.Parse(blipsInputField.text)));
            List<Button> offBlips = new List<Button>(blipsInputField.transform.parent.transform.FindChild("Blips").GetComponentsInChildren<Button>().Where(b => !onBlips.Contains(b)));

            foreach (var blip in onBlips)
            {
                var color = blip.colors;
                color.normalColor = new Color(color.highlightedColor.r == 0 ? 0 : 255, color.highlightedColor.g == 0 ? 0 : 255, color.highlightedColor.b == 0 ? 0 : 255, 1);
                color.pressedColor = Color.black;

                blip.colors = color;
            }

            foreach (var blip in offBlips)
            {
                var color = blip.colors;
                color.pressedColor = new Color(color.highlightedColor.r == 0 ? 0 : 1, color.highlightedColor.g == 0 ? 0 : 1, color.highlightedColor.b == 0 ? 0 : 1, 1);
                color.normalColor = Color.black;

                blip.colors = color;
            }
        }

        public void ReadyClicked()
        {
            //Stat points has not been fully used yet
            if(statPoints > 0)
            {
                messageText.text = "Stat Points Still Available";
            }

            //If a perk hasn't been chosen
            else if (CurrentPerk == null)
            {
                messageText.text = "Perk Not Choosen";
            }

            //Char Creation is done
            else 
            {
                //Save Player stats to text
                //Setup Save File Writer
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

                //Create Save File
                PlayerData data = new PlayerData();
                data.brawn = (sbyte)Player.Brawn.BaseValue;
                data.speed = (sbyte)Player.Speed.BaseValue;
                data.intellect = (sbyte)Player.Intellect.BaseValue;
                data.willpower = (sbyte)Player.Willpower.BaseValue;
                data.model = Player.gameObject.transform.FindChild("Model").GetChild(0).gameObject.name.Replace("(Clone)", "");
                data.perk = CurrentPerk.Name;

                //Serialize data and save, then closes file
                bf.Serialize(file, data);
                file.Close();

                Debug.Log("Character Saved: " + Application.persistentDataPath + "/playerInfo.dat");

                //Close Canvas
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }

        public void NextAvatarClicked()
        {
            modelIndex++;
            if (modelIndex >= Models.Count())
                modelIndex = 0;

            ChangeAvatar(Models[modelIndex]);
        }

        public void PreviousAvatarClicked()
        {
            modelIndex--;
            if (modelIndex < 0)
                modelIndex = Models.Count() - 1;

            ChangeAvatar(Models[modelIndex]);
        }

        void ChangeAvatar(GameObject newAvatar)
        {
            GameObject.Destroy(Player.transform.FindChild("Model").GetChild(0).gameObject);
            GameObject model = GameObject.Instantiate(newAvatar);
            model.transform.SetParent(Player.transform.FindChild("Model"), false);
            StartCoroutine(UpdateAnimator());
        }

        IEnumerator UpdateAnimator()
        {
            yield return new WaitForEndOfFrame();
            Player.GetComponent<Animator>().Rebind();
        }
    }
}

[Serializable]
class PlayerData
{
    public sbyte brawn;
    public sbyte speed;
    public sbyte intellect;
    public sbyte willpower;
    public string model;
    public string perk;
}