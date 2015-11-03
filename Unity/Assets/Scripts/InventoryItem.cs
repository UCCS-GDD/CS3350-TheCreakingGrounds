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
        public string Description;
        public Effect[] Effects;
    }
}
