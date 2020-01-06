﻿/*****************************
Created by 师鸿博
*****************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class InactiveButtons : SerializedMonoBehaviour
{
    public ItemSprite current_stay_item;
    public Image main_inactive_image;
    public Dictionary<InactiveType, Sprite> inactive_sprite_dic = new Dictionary<InactiveType, Sprite>();
    public InactiveType inactive_type = InactiveType.攻击;

    private void Start()
    {
        SetInactiveType(inactive_type);
    }
    public void SetInactiveType(InactiveType _type,ItemSprite item =null)
    {
        current_stay_item = item;
        this.inactive_type = _type;
        if (_type == InactiveType.攻击)
        {
            main_inactive_image.GetComponent<RectTransform>().anchoredPosition = new Vector2(-6.6f, -13.8f);
            main_inactive_image.sprite = WeaponConfig.Get(ActorController._controller.model.current_weapon_id).GetSprite();
        }
        else
        {
            main_inactive_image.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            main_inactive_image.sprite = inactive_sprite_dic[_type];
        }

        main_inactive_image.SetNativeSize();
    }
    public void Inactive_Click()
    {
        switch (inactive_type)
        {
            case InactiveType.攻击:
                ActorController._controller.actor_state.isAttack = true;
                break;
            case InactiveType.拾取:
                if(current_stay_item!=null)
                {
                    Destroy(current_stay_item.gameObject);
                }
                break;
            default:
                break;
        }
    }
}
