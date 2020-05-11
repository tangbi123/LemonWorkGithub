using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    ///<summary>
    ///
    ///<summary>
    public class Globe
    {
        public static string nextSceneName;
    }

public class Loading : MonoBehaviour
    {
        private Text fillText;
        private Image fill;

        private float loadingSpeed = 1;
        private float targetValue = 0;

        private AsyncOperation operation;


        
        void Start()
        {
            fillText = GameObject.Find("fillText").GetComponent<Text>();
            fill = GameObject.Find("fill").GetComponent<Image>();

            fill.fillAmount = 0;
            if(SceneManager.GetActiveScene().name == "Loading")
            {
                //print("yibujiazai");
                StartCoroutine(AsyncLoading());
            }
        }
        
        IEnumerator AsyncLoading()
        {
            operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
            operation.allowSceneActivation = false;

            yield return operation;
        }

        void Update()
        {
            targetValue = operation.progress;

            if(operation.progress >= 0.9f)
            {
                targetValue = 1.0f;
            }

            if(targetValue != fill.fillAmount)
            {
                fill.fillAmount = Mathf.Lerp(fill.fillAmount, targetValue,Time.deltaTime * loadingSpeed);

                if (Mathf.Abs(fill.fillAmount - targetValue) < 0.01f)
                    fill.fillAmount = targetValue;
            }
            //fill.fillAmount = targetValue;
            
            fillText.text = ((int)(fill.fillAmount * 100)).ToString() + "/100";
            if(fillText.text == "100/100")
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}

