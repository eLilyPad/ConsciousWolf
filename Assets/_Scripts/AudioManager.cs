using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip[] clip;

        [SerializeField]int clipIndex = 0;

        public int minTimeBetweenAudio = 3;

        bool isPlaying = false;

        public void PlayRandomSound()
        {
            StartCoroutine(RandomSound());
        }

        IEnumerator RandomSound()
        {
            int randomIndex = Random.Range(0, clip.Length);
            if(!isPlaying)
            {
                StartAudio(randomIndex);
            }
            yield return new WaitForSeconds(minTimeBetweenAudio);
            StopAudio();
        }

        void StartAudio(int index)
        {
            isPlaying = true;
            source.PlayOneShot(clip[index]);
        }
        
        void StopAudio()
        {
            isPlaying = false;
            source.Stop();
        }
    }
}
