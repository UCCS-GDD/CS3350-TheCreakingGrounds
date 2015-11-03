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

        private List<Effect> Effects;

        public String Description
        {
            get
            {
                return String.Format("{0}\n\n{1}", FlavorText, String.Join("\n", Effects.Select<Effect, string>(e => e.EffectDescription()).ToArray()));
            }
        }

        void Start()
        {
            Effects = new List<Effect>(GetComponentsInChildren<Effect>());
        }

        public void OnAdd(Player player)
        {
            foreach (var effect in Effects)
                effect.OnAdd(player);
        }
        
        public void OnRemove(Player player)
        {
            foreach (var effect in Effects)
                effect.OnRemove(player);
        }
    }
}
