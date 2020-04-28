﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamerTool.FSM;
using DreamerTool.Extra;
using DreamerTool.UI;
using UnityEngine.Events;
using DreamerTool.GameObjectPool;
using DG.Tweening;
[RequireComponent(typeof(ActorSkillController),typeof(ActorState))]
public class ActorController : MonoBehaviour,IHurt
{
     
    public static ActorController Controller;

    [System.NonSerialized] public ActorSkillController skill_controller;
    [System.NonSerialized] public ActorState actor_state;
    [System.NonSerialized] public Rigidbody2D _rigi;
    [System.NonSerialized] public float start_grivaty;
    [System.NonSerialized] public Animator _anim;
    public GameObject LevelUpEffect;
    public Transform ground_check_pos;
    public float move_speed;
    public float jump_speed;
    public float super_armor_time;
    public int max_jump_count;
    int jump_count = 0;

    public virtual  void Start()
    {
        Controller = this;
    }
    public virtual void Awake()
    {
        
        _rigi = GetComponent<Rigidbody2D>();
        start_grivaty = _rigi.gravityScale;
        _anim = GetComponentInChildren<Animator>();
        skill_controller = GetComponent<ActorSkillController>();
        actor_state = GetComponent<ActorState>();
        EventManager.OnChangeLevel += LevelUp;
    }
    public void LevelUp()
    {
        LevelUpEffect.SetActive(true);
    }
    private void OnDestroy()
    {
        EventManager.OnChangeLevel -= LevelUp;
    }

    public void Transfer(Transform point)
    {
        transform.position = point.position;
    }
    public void Move()
    {
        if (!actor_state.isMoveable)
        {
            _anim.SetBool("run", false);
            return;
        }
        Vector2 move_dir=Vector2.zero;
        if (actor_state.isMoveRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            move_dir = Vector2.right;
        }
        if (actor_state.isMoveLeft)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            move_dir = Vector2.left;
        }
         
        _anim.SetBool("run", actor_state.isMoveRight || actor_state.isMoveLeft);
        _rigi.position +=  move_dir * move_speed * Time.deltaTime; 
        
    }
 
    public virtual void Attack()
    {
        if (actor_state.isAttack)
        {
            if (actor_state.isAttackUp && actor_state.isGround)
            {
                _anim.SetTrigger("pickupattack");
            }
            else if(actor_state.isAttackDown && !actor_state.isGround)
           {
                _anim.SetTrigger("downattack");
            }
            else
                _anim.SetTrigger("attack");

            actor_state.isAttack = false;
            
        }
    
    }
   
    public  void Jump()
    {
        if(actor_state.isGround && jump_count!=0)
        {
            jump_count = 0;
        }
        if (actor_state.isJump)
        {
              
            if (jump_count < max_jump_count && (jump_count!=0 || actor_state.isGround))
            {
                jump_count++;
                _rigi.ResetVelocity();
                _rigi.AddForce  ( Vector2.up * jump_speed,ForceMode2D.Impulse);
            }
             
            actor_state.isJump = false;
        }
    }
    public void Dash()
    {
        if(actor_state.isDash)
        {
            _anim.SetTrigger("dash");
            actor_state.isDash = false;
        }
    }
    public void StateCheck()
    {
        actor_state.isGround = Physics2D.OverlapCircle(ground_check_pos.position, 1, LayerMask.GetMask("Ground"));
    }
    public void AnimCheck()
    {
        _anim.SetBool("shield", actor_state.isShield);
        _anim.SetFloat("speedy", _rigi.velocity.y);
        _anim.SetBool("isground", actor_state.isGround);
        _anim.SetFloat("gravity", _rigi.gravityScale);
    }
    public void FixedUpdate()
    {
        if (!actor_state.isInputable)
            return;
         
        Move();
        Jump();
        
    }
    public void Update()
    {
        AnimCheck();
        StateCheck();
        if (!actor_state.isInputable)
            return;
        Attack();
        Dash();
    }

    public virtual  void GetHurt(AttackData attackData)
    {
        if (actor_state.isSuperArmor)
            return;
        
        if (actor_state.isShield && (attackData.attack_pos- transform.position).normalized.x * transform.right.x>0)
        {
            _rigi.ResetVelocity();
            _rigi.AddForce(-transform.right * 20, ForceMode2D.Impulse);
            GameObjectPoolManager.GetPool("metal_hit").Get(transform.position +transform.right*2, Quaternion.identity, 1.5f);
            return;
        }

        View.CurrentScene.GetView<GameInfoView>().SetScreenEffect(Color.red,1, 0.1f,0.5f);
        _anim.SetTrigger("hit");
        transform.rotation = attackData.attack_pos.x > transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObjectPoolManager.GetPool("hit_effect").Get(transform.position + new Vector3(0, 2, 0), Quaternion.identity, 0.5f);
        Timer.Register(0.25f, () => { View.CurrentScene.GetView<GameInfoView>().SetScreenEffect(Color.red, 0, 0.1f, 0.5f); });
        actor_state.isSuperArmor = true;
        Timer.Register(super_armor_time, () => {   actor_state.isSuperArmor = false; });
        ActorModel.Model.SetHealth(-DreamerTool.Util.DreamerUtil.GetHurtValue(attackData.hurt_value,ActorModel.Model.GetPlayerAttribute(PlayerAttribute.物防)));

        GameStaticMethod.ChangeChildrenSpriteRendererColor(gameObject, Color.red);

     

        switch (attackData.attack_type)
        {
            case HitType.普通:
                break;
            case HitType.击退:
                _rigi.ResetVelocity();
                _rigi.AddForce(-transform.right * 10, ForceMode2D.Impulse);
                break;
            case HitType.击飞:
                _rigi.ResetVelocity();
                _rigi.AddForce(new Vector2(-transform.right.x * 0.5f, 0.5f).normalized * 20, ForceMode2D.Impulse);
                break;
            case HitType.上挑:
                break;
            default:
                break;
        }
    }
}
