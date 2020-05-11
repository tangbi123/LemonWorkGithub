using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


namespace NewUIMgr

{
    public class DonghuaMgr : MonoBehaviour
    {
        private VideoPlayer videoPlayer;
        private float videoFrame;

        void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();

            Play();

            StartCoroutine(PlayVideo());

        }

        IEnumerator PlayVideo()
        {
            while (true)
            {
                videoFrame = videoPlayer.frame;

                if (videoFrame >= (videoPlayer.frameCount * 0.7f) || Input.GetKeyDown(KeyCode.Space))
                {
                    videoPlayer.Stop();
                    break;
                }
                yield return null;
            }
            Globe.nextSceneName = "01-Start";
            SceneManager.LoadScene("Loading");
            StopCoroutine(PlayVideo());
        }

        public void Play()
        {
            videoPlayer.isLooping = false;
            videoPlayer.Play();
        }
    }
}
