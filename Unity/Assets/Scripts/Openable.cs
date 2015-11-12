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

        public AudioSource openStartSound;
        public AudioSource openEndSound;
        public AudioSource closeStartSound;
        public AudioSource closeEndSound;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isOpen", isOpen);

            //Set Network Animator
            for (int i = 0; i < animator.parameterCount; i++)
                GetComponent<NetworkAnimator>().SetParameterAutoSend(i, true);
        }

        public void OnActivate(Player player)
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            animator.SetTrigger("Activate");
        }

        public void OnAnimationStart()
        {
            if (isOpen && openStartSound != null)
                openStartSound.Play();
            else if (!isOpen && closeEndSound != null)
                closeEndSound.Play();
        }

        public void OnAnimationEnd()
        {
            if (isOpen && openEndSound != null)
                openEndSound.Play();
            else if (!isOpen && closeStartSound != null)
                closeStartSound.Play();
        }
    }
}
