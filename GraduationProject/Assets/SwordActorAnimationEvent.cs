﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamerTool.GameObjectPool;
using DG.Tweening;
public class SwordActorAnimationEvent : BaseActorAnimationEvent
{
    [Header("------------预制体----------")]
    public GameObject pickupslash_prefab;
    public GameObject sword_slash_prefab;
    public GameObject heavy_sword_slash_prefab;
    public GameObject skill_1_prefab;
 
    
    List<int> effect_rotation = new List<int>() { 45, 130, 60,0};
    BaseGameObjectPool pick_up_slash_pool;
    BaseGameObjectPool sword_slash_pool;
    BaseGameObjectPool heavy_sword_slash_pool;
    BaseGameObjectPool skill_1_pool;
    private void Awake()
    {
        pick_up_slash_pool = new BaseGameObjectPool(pickupslash_prefab);
        sword_slash_pool = new BaseGameObjectPool(sword_slash_prefab);
        heavy_sword_slash_pool = new BaseGameObjectPool(heavy_sword_slash_prefab);
        skill_1_pool = new BaseGameObjectPool(skill_1_prefab);
         
    }
    public void SetPickUpSlash()
    {
       var temp = pick_up_slash_pool.Get(transform.position + new Vector3(0, 2, -5), Quaternion.Euler(-45, 90 * transform.right.x, 180),0.5f);
        temp.GetComponentInChildren<SwordAttackTrigger>().attack_type = HitType.上挑;
    }
    public void SetSkill1()
    {
        skill_1_pool.Get(transform.position + transform.right * 2+new Vector3(0,-1.5f,0), Quaternion.Euler(new Vector3(-Quaternion.FromToRotation(Vector2.right, _controller.skill_controller.SkillDirection).eulerAngles.z,90,0)), 3);
    }
    public void SetSlash(int index)
    {
        if (_controller.actor_state.isGround)
        {
            _rigi.ResetVelocity();
            _rigi.AddForce(transform.right * 5, ForceMode2D.Impulse);
        }
        GameObject temp;
        temp = sword_slash_pool.Get(transform.position + new Vector3(0, 2, -index), Quaternion.Euler(transform.eulerAngles.y, 90, transform.eulerAngles.y + effect_rotation[index]),0.5f);

        if (index == 3)
        {
            temp.GetComponentInChildren<SwordAttackTrigger>().attack_type = HitType.击飞;
        }
        else
        {
            temp.GetComponentInChildren<SwordAttackTrigger>().attack_type = HitType.击退;
        }

    }
    public void SetHeavySlash()
    {
        GameObject temp;
 
        temp = heavy_sword_slash_pool.Get(transform.position + new Vector3(0, 2, 0), Quaternion.Euler(new Vector3(180-Quaternion.FromToRotation(Vector2.right, _controller.skill_controller.SkillDirection).eulerAngles.z,90,0)), 0.35f);
    }
    public void OnAttackEnter()
    { 
        if (!_controller.actor_state.isGround)
        {
             
            _controller._rigi.ResetVelocity();
            _controller._rigi.ClearGravity();
        }
        _controller.actor_state.isMoveable = false;

    }
    public void OnHeavyAttackEnter()
    {
        _controller._rigi.ResetVelocity();
        _controller._rigi.ClearGravity();
        _controller.actor_state.isInputable = false;
    }
 
    public void HeavyAttackMove()
    {
        _rigi.velocity = _controller.skill_controller.SkillDirection * 200;
        GetComponentInParent<AfterImage>().IsUpdate = true;
    }
    public void HeavyAttackReset()
    {
        _rigi.velocity = Vector2.zero;
        GetComponentInParent<AfterImage>().IsUpdate = false;
         
    }

    public void OnAttackExit()
    {

    }

}

