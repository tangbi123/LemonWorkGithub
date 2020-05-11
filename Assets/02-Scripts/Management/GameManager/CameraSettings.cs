using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

namespace GameManager
{
    public class CameraSettings : MonoBehaviour
    {
        public Camera camera;
        public enum InputChoice
        { KeyboardAndMouse,  Controller }

        [Serializable]
        public struct InvertSettings
        {
            public bool invertX;
            public bool invertY;
        }

        public Transform follow;
        public Transform lookAt;
        public CinemachineFreeLook keyboardAndMouseCamera;
        public CinemachineFreeLook controllerCamera;
        public InputChoice inputChoice;
        public InvertSettings keyboardAndMouseInvertSettings;
        public InvertSettings controllerInvertSettings;
        public bool allowRuntimeCameraSettingsChanges;

        public CinemachineFreeLook Current
        { 
            get { return inputChoice == InputChoice.KeyboardAndMouse ? keyboardAndMouseCamera : controllerCamera; } 
        }
        
        void Reset()
        {
            Transform keyboardAndMouseCameraTransform = transform.Find("KeyboardAndMouseFreeLookRig");
            if (keyboardAndMouseCameraTransform != null)
                keyboardAndMouseCamera = keyboardAndMouseCameraTransform.GetComponent<CinemachineFreeLook>();

            Transform controllerCameraTransform = transform.Find("ControllerFreeLookRig");
            if (controllerCameraTransform != null)
                controllerCamera = controllerCameraTransform.GetComponent<CinemachineFreeLook>();

            //CharacterControl playerController = FindObjectOfType<CharacterControl>();
            ////Debug.Log("playerController.name" + playerController.name);
            //if (playerController != null && playerController.name == "Shuren")
            //{
            //    follow = playerController.transform;

            //    lookAt = follow.Find("Shuren");

            //    if (playerController.cameraSettings == null)
            //        playerController.cameraSettings = this;
            //}
            lookAt = GameObject.FindWithTag("Player").transform;

            follow = GameObject.FindWithTag("Player").transform;
        }

        void Start()
        {
            UpdateCameraSettings();

            lookAt = GameObject.FindWithTag("Player").transform;

            follow = GameObject.FindWithTag("Player").transform;
        }

        void Update()
        {
            if (allowRuntimeCameraSettingsChanges) UpdateCameraSettings();
        }

        void UpdateCameraSettings()
        {
            if (follow != null && lookAt != null)
            {
                keyboardAndMouseCamera.Follow = follow;
                keyboardAndMouseCamera.LookAt = lookAt;
                keyboardAndMouseCamera.m_XAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertX;
                keyboardAndMouseCamera.m_YAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertY;

                controllerCamera.m_XAxis.m_InvertInput = controllerInvertSettings.invertX;
                controllerCamera.m_YAxis.m_InvertInput = controllerInvertSettings.invertY;
                controllerCamera.Follow = follow;
                controllerCamera.LookAt = lookAt;

            }
            keyboardAndMouseCamera.Priority = inputChoice == InputChoice.KeyboardAndMouse ? 1 : 0;
            controllerCamera.Priority = inputChoice == InputChoice.Controller ? 1 : 0;
        }

        public void ChangeLemon()
        {
            
            follow = GameObject.Find("Lemon").transform;
            lookAt = GameObject.Find("Lemon").transform;

            keyboardAndMouseCamera.m_Orbits[0].m_Height = 2.5f;
            keyboardAndMouseCamera.m_Orbits[0].m_Radius = 9f;

            keyboardAndMouseCamera.m_Orbits[1].m_Height = 2f;
            keyboardAndMouseCamera.m_Orbits[1].m_Radius = 8f;

            keyboardAndMouseCamera.m_Orbits[2].m_Height = 1.5f;
            keyboardAndMouseCamera.m_Orbits[2].m_Radius = 7f;
        }

        public void ChangeShuren()
        {
            follow = GameObject.Find("Shuren").transform;
            lookAt = GameObject.Find("Shuren").transform;

            keyboardAndMouseCamera.m_Orbits[0].m_Height = 3f;
            keyboardAndMouseCamera.m_Orbits[0].m_Radius = 7f;

            keyboardAndMouseCamera.m_Orbits[1].m_Height = 2f;
            keyboardAndMouseCamera.m_Orbits[1].m_Radius = 6f;

            keyboardAndMouseCamera.m_Orbits[2].m_Height = 1f;
            keyboardAndMouseCamera.m_Orbits[2].m_Radius = 5f;
        }

        public void ChangeEyu()
        {
            follow = GameObject.Find("Eyu").transform;
            lookAt = GameObject.Find("Eyu").transform;

            keyboardAndMouseCamera.m_Orbits[0].m_Height = 15f;
            keyboardAndMouseCamera.m_Orbits[0].m_Radius = 25f;

            keyboardAndMouseCamera.m_Orbits[1].m_Height = 8f;
            keyboardAndMouseCamera.m_Orbits[1].m_Radius = 23f;

            keyboardAndMouseCamera.m_Orbits[2].m_Height = 5f;
            keyboardAndMouseCamera.m_Orbits[2].m_Radius = 20f;
        }

        public void ShakeCamera(float duration, float liliang, int times, float dir, bool isGuiwei)
        {
            Debug.Log("zhenping");
        }
   
    }
}
