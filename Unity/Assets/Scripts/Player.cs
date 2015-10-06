using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Player : MonoBehaviour
    {
        //base stats
        public sbyte Brawn;
        public sbyte Speed;
        public sbyte Intellect;
        public sbyte Willpower;

        //vital stats
        public sbyte Wounds;
        public sbyte Traumas;

        //character perk
        public Perk Perk;

        //inventory
        public List<IInventoryItem> Inventory;

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }
    }
}
