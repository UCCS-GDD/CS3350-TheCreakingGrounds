using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class InventoryItem : MonoBehaviour
    {
        public Sprite Icon;
        public string Name;
        public string FlavorText;

        protected Effect[] Effects
        {
            get
            {
                return gameObject.GetComponents<Effect>();
            }
        }

        public abstract void OnAdd(Player player);
        public abstract void OnRemove(Player player);

        public virtual void OnUpdate(Player player)
        {
            foreach (var effect in Effects)
                effect.OnUpdate(player);
        }
    }
}
