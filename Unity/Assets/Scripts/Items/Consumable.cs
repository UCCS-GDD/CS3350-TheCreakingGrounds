using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Consumable : InventoryItem
    {
        public override void OnAdd(Player player)
        {
            return;
        }

        public override void OnRemove(Player player)
        {
            return;
        }

        public virtual void OnConsume(Player player)
        {
            foreach (var effect in Effects)
            {
                effect.OnAdd(player);
            }
        }
    }
}
