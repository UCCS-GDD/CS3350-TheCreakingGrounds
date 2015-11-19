using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts
{
    public abstract class Effect : MonoBehaviour
    {
        public float Duration = -1.0f;
        public string DescriptionOverride;

        public abstract void OnAdd(Player player);

        public abstract void OnUpdate(Player player);

        public abstract void OnRemove(Player player);

        public abstract string EffectDescription();
    }
}
