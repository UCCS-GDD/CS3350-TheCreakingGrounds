using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class CharacterCreate : MonoBehaviour
    {
        public InputField StatPoints;

        public InputField BrawnField;
        int brawn = 4;

        public InputField SpeedField;
        int speed = 4;

        public InputField IntellectField;
        int intellect = 4;

        public InputField WillpowerField;
        int willpower = 4;

        int statPoints = 4;

        public Perk CurrentPerk;

        public InputField charNameField;

        private static bool disableEntry = false;

        public void Start()
        {
            StatPoints.text = statPoints.ToString();
            BrawnField.text = brawn.ToString();
            SpeedField.text = speed.ToString();
            IntellectField.text = intellect.ToString();
            WillpowerField.text = willpower.ToString();

            UpdateBlips(BrawnField);
            UpdateBlips(SpeedField);
            UpdateBlips(IntellectField);
            UpdateBlips(WillpowerField);
        }

        public void StatsIncremented(string stat)
        {
            switch (stat.ToLower())
            {
                case "brawn":
                    UpdateStats(BrawnField, ref brawn, brawn + 1);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref speed, speed + 1);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref intellect, intellect + 1);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref willpower, willpower + 1);
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
                    UpdateStats(BrawnField, ref brawn, brawn - 1);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref speed, speed - 1);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref intellect, intellect - 1);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref willpower, willpower - 1);
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
                    UpdateStats(BrawnField, ref brawn, value);
                    break;
                case "speed":
                    UpdateStats(SpeedField, ref speed, value);
                    break;
                case "intellect":
                    UpdateStats(IntellectField, ref intellect, value);
                    break;
                case "willpower":
                    UpdateStats(WillpowerField, ref willpower, value);
                    break;
                default:
                    return;
            }

        }

        void UpdateStats(InputField statField, ref int curVal, int value)
        {
            int diffVal = value - curVal;
            int endVal = Mathf.Clamp(Math.Min(curVal + statPoints, curVal + diffVal), 1, 10);
            diffVal = endVal - curVal;
            statPoints -= diffVal;

            StatPoints.text = statPoints.ToString();
            statField.text = endVal.ToString();

            curVal = endVal;

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
    }
}
