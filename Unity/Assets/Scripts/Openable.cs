using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class Openable : MonoBehaviour, Activator
    {
        public bool isOpen = false;

        private Animator animator;

        public AudioSource openSound;
        public AudioSource closeSound;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isOpen", isOpen);

            //Set Network Animator
            for (int i = 0; i < animator.parameterCount; i++)
                GetComponent<NetworkAnimator>().SetParameterAutoSend(i, true);
        }

        public void OnActivate()
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            animator.SetTrigger("Activate");
            if (openSound != null && isOpen == true)
                openSound.Play();
            else if (closeSound != null && isOpen == false)
                closeSound.Play();
        }
    }
}
