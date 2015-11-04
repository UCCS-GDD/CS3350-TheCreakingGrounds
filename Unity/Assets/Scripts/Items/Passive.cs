using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    public class Passive : InventoryItem
    {
        public override void OnAdd(Player player)
        {
            foreach (var effect in Effects)
                effect.OnAdd(player);
        }

        public override void OnRemove(Player player)
        {
            foreach (var effect in Effects)
                effect.OnAdd(player);
        }
    }
}
