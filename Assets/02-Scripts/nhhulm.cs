//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class nhhulm : MonoBehaviour
//{
//    public GameObject cube_my;
//    public GameObject camera;
//    public GameObject ojn;
//    float time = 0;
//    bool right = false;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.V)){
//            //Time.timeScale = 0;
//            time = 0;
//            right = true;
//            ojn.SetActive(false);
//            camera.SetActive(false);
//            cube_my.SetActive(true);
//        }
//        if(right){
//            if(time >= 11){
//                cube_my.SetActive(false);
//                ojn.SetActive(true);
//                camera.SetActive(true);

//            }
//        }
//        time += Time.deltaTime;
//    }
//}
