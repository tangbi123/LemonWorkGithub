using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest2 : MonoBehaviour
{
    
    //public AnimatorStateInfo animSta;
    public int speed = 5;

    private float vertical;
    private float horizontal;
    private Vector3 moveDir;
    private Animator anim;
    private int attackCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.Q))Attack();
        
    }
    public void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        moveDir = new Vector3(horizontal, 0, vertical);
        this.transform.Translate(moveDir.normalized * speed * Time.deltaTime);

        //控制动画
        if(moveDir.magnitude >= 0.05f)
        {
            anim.SetBool("move", true);
        }
        else anim.SetBool("move", false);
    }

    public void Attack()
    {
        if (attackCount == 0)
        {
            anim.SetInteger("attack", 1);
            attackCount++;
        }
        else if (attackCount == 1)
        {
            anim.SetInteger("attack", 2);
            attackCount++;
        }
        else if (attackCount == 2)
        {
            anim.SetInteger("attack", 3);
            attackCount++;
        }
        else
        {
            attackCount = 0;
            anim.SetInteger("attack", 0);
        }
    }

    public void test()
    {
        
    }


}
