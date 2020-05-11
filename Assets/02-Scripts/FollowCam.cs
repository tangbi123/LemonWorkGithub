using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 相机跟随
/// </summary>
public class FollowCam : MonoBehaviour {
    
    
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;//追踪速度
    //public GameObject target;

   private Transform targetTr;
    private Transform tr;//当前位置
   

	// Use this for initialization
	void Start () {
        tr = this.GetComponent<Transform>();
        targetTr = GameObject.FindWithTag("Player").transform;
    }

	void LateUpdate () {

        if (targetTr != null)
        {
            FollowTarget();
            //targetTr = null;
        }
	}

    /// <summary>
    /// 相机跟随标签为 Playerer 的目标
    /// </summary>
    void FollowTarget()
    {
        targetTr = GameObject.FindWithTag("Player").transform;
        tr.position = Vector3.Lerp(tr.position,
            targetTr.position - (targetTr.forward * dist) + (Vector3.up * height),
            Time.deltaTime * dampTrace);
        tr.LookAt(targetTr.position);
    }

}
