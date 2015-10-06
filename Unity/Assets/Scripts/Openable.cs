using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Openable : MonoBehaviour, Activator
    {
        public bool isOpen = false;

        public void Awake()
        {
            GetComponent<Animator>().SetBool("isOpen", isOpen);
        }

        public void OnActivate()
        {
            isOpen = !isOpen;
            GetComponent<Animator>().SetBool("isOpen", isOpen);
        }
    }
}
