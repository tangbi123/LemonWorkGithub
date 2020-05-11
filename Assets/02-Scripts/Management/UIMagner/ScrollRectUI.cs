using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    ///<summary>
    ///
    ///<summary>
	public class ScrollRectUI : MonoBehaviour,IEndDragHandler 
    {
        private float smoothing = 5f;
        private float[] pages = new float[] { 0, 0.5f, 1 };
        public ScrollRect scrollRect;

        private float targetHorizontalPosition;

        void Start()
        {
            //scrollRect = this.GetComponent<ScrollRect>();

        }

        void Update()
        {

            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
                targetHorizontalPosition, Time.fixedDeltaTime * smoothing);
        }

        public void OnEndDrag(PointerEventData envenData)
        {
            float posX = scrollRect.horizontalNormalizedPosition;
            int index = 0;
            float offset = Mathf.Abs(pages[index] - posX);

            for (int i = 0; i < pages.Length; i++)
            {
                float offsetTemp = Mathf.Abs(pages[i] - posX);
                if (offsetTemp < offset)
                {
                    index = i;
                    offset = offsetTemp;
                }
            }
            targetHorizontalPosition = pages[index];
            //print(scrollRect.horizontalNormalizedPosition);
        }
    }
}

