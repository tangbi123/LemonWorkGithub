using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.audio
{
    class AudioManager:MonoBehaviour
    {
        public static AudioManager Instance;
        AudioSourceManager audioSourceManager;
        ClipsManager clipsManager;
        public AudioSource asBgm;
        void Awake()
        {
            Instance = this;

            audioSourceManager = new AudioSourceManager(this.gameObject);
            clipsManager = new ClipsManager();
            asBgm = audioSourceManager.GetFreeAudio();
            asBgm.loop = true;
            PlayBgm("bgm1");
        }

        public void Play(string audioName)
        {
            //拿一个空闲audio
            AudioSource tempSource = audioSourceManager.GetFreeAudio();
            //拿一个clip
            SingleClip tempClip = clipsManager.FindClipByName(audioName);
            //播放
            tempClip.Play(tempSource);

        }

        public void Stop(string audioName)
        {
            audioSourceManager.Stop(audioName);
        }

        /// <summary>
        /// 播放和切换bgm
        /// </summary>
        /// <param name="audioName"></param>
        public void PlayBgm(string audioName)
        {
            //拿一个clip
            SingleClip tempClip = clipsManager.FindClipByName(audioName);
            //播放
            tempClip.Play(asBgm);
        }
    }
}
