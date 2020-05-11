using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class neweee : MonoBehaviour
{
    // Start is called before the first frame update
    public float time_hj;
    public PlayableDirector timeline;
    public GameObject timelk;
    void Start()
    {
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            //Time.timeScale = 0;
            //timeline.Stop();
            //timeline.set
            timelk.SetActive(true);
        }
        
    }
}
