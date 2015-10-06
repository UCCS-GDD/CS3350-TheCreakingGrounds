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

        private Animator animator;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isOpen", isOpen);
        }

        public void OnActivate()
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            animator.SetTrigger("Activate");
        }
    }
}
