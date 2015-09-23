using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class EndTable : MonoBehaviour, Acitvator
    {
        public GameObject drawer;
        public bool isOpen = false;

        public void Awake()
        {
            drawer.GetComponent<Animator>().SetBool("isOpen", isOpen);
        }

        public void OnActivate()
        {
            isOpen = !isOpen;
            drawer.GetComponent<Animator>().SetBool("isOpen", isOpen);
        }
    }
}
