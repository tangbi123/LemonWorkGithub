using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.audio
{
    /// <summary>
    /// 1、从audioSources 拿出一个空闲的 AudioSource
    /// 2、释放多余AudioSource
    /// 3、播放一个音效
    /// 4、停止播放一个音效
    /// </summary>
    class AudioSourceManager
    {
        List<AudioSource> audioSources;
        GameObject ower;

        public AudioSourceManager (GameObject tempOwer)
        {
            ower = tempOwer;
            InitAS();
        }
        public void InitAS()
        {

            audioSources = new List<AudioSource>();
            for (int i = 0; i < 3; i++)
            {
                AudioSource tempSource = ower.AddComponent<AudioSource>();
                tempSource.loop = false;
                audioSources.Add(tempSource);
            }
        }
        public void Stop(string audioName)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if(audioSources[i].isPlaying && audioSources[i].clip.name.Equals(audioName))
                {
                    audioSources[i].Stop();
                }
            }
        }

        public AudioSource GetFreeAudio()
        {
            bool isGet = false;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if(!audioSources[i].isPlaying)
                {
                    isGet = true;
                    return audioSources[i];
                }
            }
            //上面无法返回freeAudio

            AudioSource tempAudio = ower.AddComponent<AudioSource>();
            audioSources.Add(tempAudio);
            return tempAudio;
        }

        /// <summary>
        /// 释放多余freeaudio
        /// </summary>
        public void ReleaseFreeAudio()
        {
            List<AudioSource> tempSources = new List<AudioSource>();
            int tempCount = 0;

            //遍历找出freeAudio
            for (int i = 0; i < audioSources.Count; i++)
            {
                if(!audioSources[i].isPlaying)
                {
                    tempCount++;

                    if(tempCount > 3)
                    {
                        tempSources.Add(audioSources[i]);
                    }
                }
            }

            //释放freeAudio
            for (int i = 0; i < tempSources.Count; i++)
            {
                AudioSource tempSource = tempSources[i];
                audioSources.Remove(tempSource);
                GameObject.Destroy(tempSource);
            }

            tempSources.Clear();
            tempSources = null;

        }
    }
}
