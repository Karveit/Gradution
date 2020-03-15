﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamerTool.UI;
using DG.Tweening;
public class PlayerSwordAttackTrigger : BaseAttackTrigger
{

   
    public override void OnTriggerEnter2D(Collider2D collision)
    {   
        if(collision.gameObject.tag=="Enemy")
        {
            ActorModel.Model.SetEngery(ActorModel.Model.GetCurrentWeapon().回复能量);
            ( View.CurrentScene as GameScene) .HitCount++;
            if(attack_type == HitType.击飞)
            {
                
                Camera.main.GetComponent< Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
                Time.timeScale = 0.2f;
            }
                collision.gameObject.GetComponent<IHurt>().GetHurt(ActorModel.Model.GetPlayerAttribute(PlayerAttribute.攻击力),attack_type,ActorController._controller.transform.position);
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        
        
    }

   
}
