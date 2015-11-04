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

        public abstract void OnAdd(Player player);

        public virtual void OnUpdate(Player player)
        {
            if (Duration >= 0)
            {
                Duration -= Time.deltaTime;
                if (Duration < 0)
                    OnRemove(player);
            }
        }

        public abstract void OnRemove(Player player);

        public abstract string EffectDescription();
    }
}
