using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lemon.audio
{
    class SingleClip
    {
        AudioClip myClip;

        public SingleClip(AudioClip tempClip)
        {
            myClip = tempClip;
            
        }

        public void Play(AudioSource tempSource)
        {
            Debug.Log(myClip.name);
            tempSource.clip = myClip;
            tempSource.Play();
        }
    }
}
