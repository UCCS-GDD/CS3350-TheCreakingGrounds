using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Items;

namespace Assets.Scripts.Menu
{
    public class StatusMenu : MonoBehaviour
    {
        public GameObject itemContainer;
        public Button buttonPrefab;

        public Text UIDescription;
        public Text UIName;
        public Text UIType;
        public Button UseItem;

        public GameObject Brawn;
        public GameObject Speed;
        public GameObject Intellect;
        public GameObject Willpower;
        public GameObject Traumas;
        public GameObject Wounds;

        public Button eq1Button;
        public Image eq1Icon;
        public Text eq1Name;
        public Text eq1Desc;

        public Button eq2Button;
        public Image eq2Icon;
        public Text eq2Name;
        public Text eq2Desc;

        Player player;

        public void InitializeMenu(Player player)
        {
            this.player = player;

            player.ShowMouse();

            foreach (Transform child in itemContainer.transform)
                Destroy(child.gameObject);

            foreach (var kvp in player.Inventory.Where(p => p.Value > 0))
            {
                var item = kvp.Key;
                int count = kvp.Value;

                Button button = GameObject.Instantiate<Button>(buttonPrefab);
                button.transform.SetParent(itemContainer.transform, false);

                if (item.IsArtifact)
                    button.transform.FindChild("Border").gameObject.GetComponent<Image>().color = Color.red;

                button.transform.FindChild("CountPanel").gameObject.GetComponentInChildren<Text>().text = count.ToString();

                if (item.Icon != null)
                    button.GetComponent<Image>().sprite = item.Icon;

                InventoryItem temp = item;
                button.onClick.AddListener(() => { ItemClicked(temp); });
            }

            foreach (Transform blip in Brawn.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Brawn.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = Color.red;
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Brawn.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Brawn.CurrentValue.ToString();

            foreach (Transform blip in Speed.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Speed.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = Color.yellow;
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Speed.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Speed.CurrentValue.ToString();

            foreach (Transform blip in Intellect.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Intellect.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = Color.blue;
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Intellect.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Intellect.CurrentValue.ToString();

            foreach (Transform blip in Willpower.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Willpower.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = new Color(1, 0, 1, 1);
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Willpower.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Willpower.CurrentValue.ToString();

            foreach (Transform blip in Wounds.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Wounds.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = Color.red;
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Wounds.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Wounds.CurrentValue.ToString();

            foreach (Transform blip in Traumas.transform.FindChild("Blips"))
            {
                GameObject go = blip.gameObject;
                if (player.Traumas.CurrentValue >= int.Parse(go.name))
                    go.GetComponent<Image>().color = new Color(1, 0, 1, 1);
                else
                    go.GetComponent<Image>().color = Color.black;
            }
            Traumas.transform.FindChild("Value").GetComponentInChildren<Text>().text = player.Traumas.CurrentValue.ToString();

            if (player.EquippedItem1 != null)
            {
                eq1Icon.sprite = player.EquippedItem1.Icon;
                eq1Name.text = player.EquippedItem1.Name;
                eq1Desc.text = player.EquippedItem1.Description;
            }
            else
            {
                eq1Icon.sprite = null;
                eq1Name.text = "";
                eq1Desc.text = "";
            }

            if (player.EquippedItem2 != null)
            {
                eq2Icon.sprite = player.EquippedItem2.Icon;
                eq2Name.text = player.EquippedItem2.Name;
                eq2Desc.text = player.EquippedItem2.Description;
            }
            else
            {
                eq2Icon.sprite = null;
                eq2Name.text = "";
                eq2Desc.text = "";
            }
        }

        public void ItemClicked(InventoryItem item)
        {
            UIDescription.text = item.Description;
            UIName.text = item.Name;
            switch (item.GetType().Name)
            {
                case "Consumable":
                    UIType.text = "Consume";
                    break;
                case "Equippable":
                    UIType.text = "Equip";
                    break;
                case "Passive":
                    UIType.text = "Passive";
                    break;
                default:
                    break;
            }

            InventoryItem temp = item;
            UseItem.onClick.RemoveAllListeners();
            UseItem.onClick.AddListener(() => { UseClicked(temp); });
        }

        public void UseClicked(InventoryItem item)
        {
            if (item is Consumable)
            {
                player.Inventory[item] -= 1;

                if (player.Inventory[item] <= 0)
                    UseItem.onClick.RemoveAllListeners();
            }
            else if (item is Equippable)
            {
                if (player.EquippedItem1 == null)
                {
                    player.EquippedItem1 = item;
                }
                else if (player.EquippedItem2 == null)
                {
                    player.EquippedItem2 = item;
                }
            }

            InitializeMenu(player);
        }

        public void Eq1Clicked()
        {
            player.EquippedItem1 = null;
            InitializeMenu(player);
        }

        public void Eq2Clicked()
        {
            player.EquippedItem2 = null;
            InitializeMenu(player);
        }
    }
}
