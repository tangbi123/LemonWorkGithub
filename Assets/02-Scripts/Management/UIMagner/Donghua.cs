using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

namespace UI
{
    ///<summary>
    ///
    ///<summary>
	public class Donghua : MonoBehaviour
    {
        private VideoPlayer videoPlayer;
        private float videoFrame;

        void Start()
        {
            videoPlayer = GameObject.Find("开场动画").GetComponent<VideoPlayer>();

            Play();

            StartCoroutine(PlayVideo());
        }

        IEnumerator PlayVideo()
        {
            while (true)
            {
                videoFrame = videoPlayer.frame;
                if (videoFrame >= (videoPlayer.frameCount * 0.97) || Input.GetKeyDown(KeyCode.Space))
                {
                    Globe.nextSceneName = "01-Start";
                    SceneManager.LoadScene("Loading");
                    yield return new WaitForSeconds(0.5f);
                    videoPlayer.Stop();
                    //start.enabled = !start.enabled;
                    
                    StopCoroutine("PlayVideo");
                    break;

                }
                yield return null;
            }
        }

        public void Play()
        {
            videoPlayer.isLooping = false;
            videoPlayer.Play();
            //GameObject.Find("BG").active = false;
           // start.enabled = !start.enabled;
        }
    }
}

