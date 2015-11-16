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

        public string openStartSoundPath;
        public string openEndSoundPath;
        public string closeStartSoundPath;
        public string closeEndSoundPath;

        AudioClip[] openStartSounds;
        AudioClip[] openEndSounds;
        AudioClip[] closeStartSounds;
        AudioClip[] closeEndSounds;

        private AudioSource audioSource;

        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            animator = GetComponent<Animator>();
            animator.SetBool("isOpen", isOpen);

            //Set Network Animator
            for (int i = 0; i < animator.parameterCount; i++)
                GetComponent<NetworkAnimator>().SetParameterAutoSend(i, true);

            openStartSounds = Resources.LoadAll<AudioClip>(openStartSoundPath);
            openEndSounds = Resources.LoadAll<AudioClip>(openEndSoundPath);
            closeStartSounds = Resources.LoadAll<AudioClip>(closeStartSoundPath);
            closeEndSounds = Resources.LoadAll<AudioClip>(closeEndSoundPath);
        }

        public void OnActivate(Player player)
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            animator.SetTrigger("Activate");
        }

        public void OnAnimationStart()
        {
            if (animator.GetBool("isOpen") && openStartSounds.Count() > 0)
                audioSource.PlayOneShot(openStartSounds.PickRandom());
            else if (!animator.GetBool("isOpen") && closeEndSounds.Count() > 0)
                audioSource.PlayOneShot(closeEndSounds.PickRandom());
        }

        public void OnAnimationEnd()
        {
            if (animator.GetBool("isOpen") && openEndSounds.Count() > 0)
                audioSource.PlayOneShot(openEndSounds.PickRandom());
            else if (!animator.GetBool("isOpen") && closeStartSounds.Count() > 0)
                audioSource.PlayOneShot(closeStartSounds.PickRandom());
        }
    }
}
