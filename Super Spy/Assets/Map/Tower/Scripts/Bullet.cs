﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject mTarget;
    float mMoveSpeed;
    int mAttack;//shang害zhi

    public void InitData(GameObject target, float move_speed, int atk)
    {
        mTarget = target;
        mMoveSpeed = move_speed;
        mAttack = atk;
    }
    
    private Vector3 mTargetPos;
    // Update is called once per frame
    void Update()
    {
        if (mTarget == null)//追人追到一半那个人死了
            GameObject.Destroy(gameObject);
        else
        {
            mTargetPos = mTarget.transform.position;
            float rang = Vector3.Distance(transform.position, mTargetPos);//子dan和di人距离
            float total_time = rang / this.mMoveSpeed;//子dan到di人的shijian
            if (Time.deltaTime >= total_time)
            {//追上人了
                
				mTarget.GetComponent<AttackBase>().BeAttacked(gameObject, mAttack);
                GameObject.Destroy(gameObject);
            }
            else//子dan追人
            {
                this.transform.position = Vector3.Lerp(this.transform.position, this.mTargetPos, Time.deltaTime / total_time);
            }
        }
    }
}      
