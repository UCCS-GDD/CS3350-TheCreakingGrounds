using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Perk : MonoBehaviour
    {
        public string Name = "";
        public string Description = "";
        public Sprite Icon = null;

        public abstract void OnAdd(Player player);

        public abstract void OnRemove(Player player);
    }
}
