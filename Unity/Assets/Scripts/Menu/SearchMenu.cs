using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class SearchMenu  : MonoBehaviour
    {
        public ContainerListController itemList;
        public Button takeAllButton;

        public void ShowSearchMenu(Dictionary<InventoryItem, int> inventory, Player player)
        {
            player.ShowMouse();
            itemList.ShowInventory(inventory);
            takeAllButton.onClick.RemoveAllListeners();
            takeAllButton.onClick.AddListener(() => { TakeAllClicked(inventory, player); });
        }

        public void TakeAllClicked(Dictionary<InventoryItem, int> inventory, Player player)
        {
            player.HideMouse();
            player.AddItems(inventory);
            itemList.Clear();
            gameObject.SetActive(false);
        }
    }
}
