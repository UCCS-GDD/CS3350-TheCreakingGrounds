using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    public class Equippable : InventoryItem
    {
        public override void OnAdd(Player player)
        {
            return;
        }

        public override void OnRemove(Player player)
        {
            return;
        }

        public virtual void OnEquip(Player player)
        {
            foreach (var effect in Effects)
                effect.OnAdd(player);
        }

        public virtual void OnUnequip(Player player)
        {
            foreach (var effect in Effects)
                effect.OnRemove(player);
        }
    }
}
