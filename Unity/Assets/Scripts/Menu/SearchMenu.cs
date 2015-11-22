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

        Dictionary<InventoryItem, int> inventory;
        Player player;

        void Update()
        {
            if (player != null && inventory != null && Input.GetButtonDown("Activate"))
                TakeAllClicked(inventory, player);
        }

        public void ShowSearchMenu(Dictionary<InventoryItem, int> inventory, Player player)
        {
            this.inventory = inventory;
            this.player = player;

            player.ShowMouse();
            itemList.ShowInventory(inventory);
            takeAllButton.onClick.RemoveAllListeners();
            takeAllButton.onClick.AddListener(() => { TakeAllClicked(inventory, player); });
        }

        public void TakeAllClicked(Dictionary<InventoryItem, int> inventory, Player player)
        {
            this.inventory = null;
            this.player = null;

            player.HideMouse();
            player.AddItems(inventory);
            itemList.Clear();
            gameObject.SetActive(false);
        }
    }
}
