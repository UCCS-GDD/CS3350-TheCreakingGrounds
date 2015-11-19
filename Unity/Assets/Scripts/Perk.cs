using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Perk : MonoBehaviour
    {
        public string Name = "";
        public string FlavorText = "";
        public Sprite Icon = null;
        public bool IsStartPerk = true;

        private Effect[] Effects
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

        public void OnAdd(Player player)
        {
            foreach (var effect in Effects)
                player.AddEffect(effect);
        }

        public void OnUpdate(Player player)
        {
            foreach (var effect in Effects)
                effect.OnUpdate(player);
        }
        
        public void OnRemove(Player player)
        {
            foreach (var effect in Effects)
                effect.OnRemove(player);
        }
    }
}
