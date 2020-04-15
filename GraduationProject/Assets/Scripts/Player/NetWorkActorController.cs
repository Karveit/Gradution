﻿/*****************************
Created by 师鸿博
*****************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using LitJson;
using DreamerTool.GameObjectPool;
using DreamerTool.Extra;
using UnityEngine.Events;

public class NetWorkActorController : ActorController
{
    private FileDataActor _actor;
    public GameObject[] hide_gameObject;
    private PhotonView photonView;
    public TextMesh nameText;
    public GameObject circle;
    private ActorModel _model;
    public ActorModel GetModel()
    {
        return _model;
    }
    public new void Start()
    {
        _actor = GetComponent<FileDataActor>();
        photonView = GetComponent<PhotonView>();
        
        if (photonView.IsMine)
        {
            Controller = this;
        }
        else
        {
            foreach (var gam in hide_gameObject)
            {
                gam.SetActive(false);
            }
            Destroy(_rigi);
        }
        SetModel( JsonMapper.ToObject<ActorModel>(photonView.InstantiationData[0].ToString()));
    }
    public void SetModel(ActorModel model)
    {
        _model = model;
        _actor.SetModel(model);
       
        DreamerTool.UI.View.CurrentScene.GetView<NetWorkGameInfoView>().huds[(int)photonView.Owner.CustomProperties["number"]].SetModel(model);
        nameText.text =DreamerTool.Util.DreamerUtil.GetColorRichText( model.actor_name, photonView.IsMine?Color.white:Color.red);
        circle.SetActive(photonView.IsMine);
    }


    public new void GetHurt(AttackData attackData, UnityAction hurt_call_back = null)
    {
 
        if (actor_state.isShield && (attackData.attack_pos - transform.position).normalized.x * transform.right.x > 0)
        {
            _rigi.ResetVelocity();
            _rigi.AddForce(-transform.right * 20, ForceMode2D.Impulse);
            GameObjectPoolManager.GetPool("metal_hit").Get(transform.position + transform.right * 2, Quaternion.identity, 1.5f);
            return;
        }
        
        _anim.SetTrigger("hit");
        GameStaticMethod.ChangeChildrenSpriteRendererColor(gameObject, Color.red);
        transform.rotation = attackData.attack_pos.x > transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        GameObjectPoolManager.GetPool("hit_effect").Get(transform.position + new Vector3(0, 2, 0), Quaternion.identity, 0.5f);

 
       _model.SetHealth(-DreamerTool.Util.DreamerUtil.GetHurtValue(attackData.hurt_value, _model.GetPlayerAttribute(PlayerAttribute.物防)));

       

        hurt_call_back?.Invoke();

        switch (attackData.attack_type)
        {
            case HitType.普通:
                break;
            case HitType.击退:
                _rigi.ResetVelocity();
                _rigi.AddForce(-transform.right * 20, ForceMode2D.Impulse);
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
    public new void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        base.FixedUpdate();
    }
}
