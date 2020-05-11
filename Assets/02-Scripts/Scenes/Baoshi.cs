using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    public class Baoshi : MonoBehaviour
    {
        public MeshRenderer[] renders;
        public Material[] materials;

        private bool flag = false;
        void Start()
        {
            renders = GetComponentsInChildren<MeshRenderer>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U)) ChangeToRed();
            if (Input.GetKeyDown(KeyCode.Y)) ChangeToYellow();
        }
        public void OnCollidsionEnter(Collision coll)
        {
            if (coll.gameObject.tag == "Player")
            {
                flag = !flag;
                if (flag)
                {
                    ChangeToRed();
                    Debug.Log("red");
                }
                else
                {
                    ChangeToYellow();
                    Debug.Log("Yellow");
                }
            }
        }

        public void ChangeToRed()
        {
            foreach (var render in renders) render.material = materials[1];
        }

        public void ChangeToYellow()
        {
            foreach (var render in renders) render.material = materials[0];
        }
    }
}
