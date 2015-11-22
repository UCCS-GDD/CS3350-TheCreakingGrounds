using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Container : MonoBehaviour, Activator
    {
        public static Dictionary<InventoryItem, int> GeneratedItems = new Dictionary<InventoryItem,int>();
        static List<InventoryItem> mundaneItems;
        static List<InventoryItem> artifacts;
        public Dictionary<InventoryItem, int> Items = new Dictionary<InventoryItem,int>();

        void Start()
        {
            //GenerateInventory();
        }

        public static List<InventoryItem> Artifacts
        {
            get 
            {
                if (artifacts == null)
                    artifacts = Resources.LoadAll<InventoryItem>("Data/Items").Where(i => i.IsArtifact).ToList();

                return artifacts; 
            }
        }

        public static List<InventoryItem> MundaneItems
        {
            get
            {
                if (mundaneItems == null)
                    mundaneItems = Resources.LoadAll<InventoryItem>("Data/Items").Where(i => !i.IsArtifact).ToList();

                return mundaneItems; 
            }
        }

        public static Dictionary<InventoryItem, int> GenerateInventory()
        {
            var items = new Dictionary<InventoryItem, int>();
            for (
                int i = Random.value >= 0.5 ? Random.Range(GameSettings.ItemCountMean, GameSettings.ItemCountMax + 1) : Random.Range(GameSettings.ItemCountMin, GameSettings.ItemCountMean + 1);
                i > 0; i--)
            {
                InventoryItem item = PickRandomItem();
                if (items.ContainsKey(item))
                    items[item] += 1;
                else
                    items.Add(item, 1);
            }

            return items;
        }

        public static InventoryItem PickRandomItem()
        {
            List<InventoryItem> choices;
            if (Random.value <= GameSettings.ArtifactGenerationChance)
            {
                choices = Artifacts.Where(a => !GeneratedItems.ContainsKey(a)).ToList();
                if (choices.Count <= 0)
                    choices = MundaneItems;
            }
            else
                choices = MundaneItems;

            InventoryItem outItem = choices[Random.Range(0, choices.Count)];

            if (GeneratedItems.ContainsKey(outItem))
                GeneratedItems[outItem] += 1;
            else
                GeneratedItems.Add(outItem, 1);

            return outItem;
        }

        public void OnActivate(Player player)
        {
            player.UI.searchPanel.gameObject.SetActive(true);
            player.UI.searchPanel.ShowSearchMenu(Items, player);
            Items = new Dictionary<InventoryItem, int>();
        }
    }
}
