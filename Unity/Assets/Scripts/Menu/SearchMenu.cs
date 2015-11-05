using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    class SearchMenu  : MonoBehaviour
    {
        public ContainerListController itemList;

        void Start()
        {
            GetComponent<Container>().GenerateInventory();
            Invoke("Kick", 3);
        }

        void Kick()
        {
            ShowSearchMenu(GetComponent<Container>(), null);
        }

        public void ShowSearchMenu(Container inventory, Player player)
        {
            itemList.ShowInventory(inventory);
            enabled = true;
        }
    }
}
