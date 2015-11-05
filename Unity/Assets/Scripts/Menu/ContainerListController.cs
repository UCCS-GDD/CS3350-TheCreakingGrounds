using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class ContainerListController : MonoBehaviour
    {
        public Button buttonPrefab;
        public GameObject buttonHolder;
        public Text UIDescription;
        public Text UIName;
        public Text UIType;

        public void ShowInventory(Dictionary<InventoryItem, int> inventory)
        {
            foreach (var kvp in inventory)
            {
                var item = kvp.Key;
                int count = kvp.Value;

                Button button = GameObject.Instantiate<Button>(buttonPrefab);
                button.transform.SetParent(buttonHolder.transform, false);

                if (item.IsArtifact)
                    button.transform.FindChild("Border").gameObject.GetComponent<Image>().color = Color.red;

                button.transform.FindChild("CountPanel").gameObject.GetComponentInChildren<Text>().text = count.ToString();

                if (item.Icon != null)
                    button.GetComponent<Image>().sprite = item.Icon;

                InventoryItem temp = item;
                button.onClick.AddListener(() => { ItemClicked(temp); });
            }
        }

        void ItemClicked(InventoryItem item)
        {
            UIDescription.text = item.Description;
            UIName.text = item.Name;
            UIType.text = item.GetType().Name;
        }

        public void Clear()
        {
            foreach (Transform child in buttonHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
