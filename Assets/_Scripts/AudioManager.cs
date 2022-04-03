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

        void Start()
        {
            StartCoroutine(PlayRandomSound());
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartAudio(clipIndex);
            }
        }

        IEnumerator PlayRandomSound()
        {
            
            while(true)
            {
                yield return new WaitForSeconds(Random.Range(1,10));

                int randomIndex = Random.Range(0, clip.Length);

                StartAudio(randomIndex);

                yield return new WaitForSeconds(minTimeBetweenAudio);
                StopAudio();
            }
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
