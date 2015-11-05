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
        public bool IsArtifact = false;
        public string FlavorText;

        protected Effect[] Effects
        {
            get
            {
                return gameObject.GetComponents<Effect>();
            }
        }

        public String Description
        {
            get
            {
                return String.Format("{0}\n\n{1}", FlavorText, String.Join("\n", Effects.Select<Effect, string>(e => e.EffectDescription()).ToArray()));
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
