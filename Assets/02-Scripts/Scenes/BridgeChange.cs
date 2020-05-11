using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ns
{
    public class BridgeChange : MonoBehaviour
    {
        public VisualEffect visualEffect;

         private void Start() {
            visualEffect = GetComponent<VisualEffect>();
        }

        void Update(){
            if (Input.GetKeyDown(KeyCode.O))ChangeToOpen() ;
            if (Input.GetKeyDown(KeyCode.P)) ChangeToClose();
        }
        public void ChangeToOpen()
        {
            visualEffect.Play();
        }

        public void ChangeToClose()
        {
            visualEffect.Stop();
        }
    }
}
