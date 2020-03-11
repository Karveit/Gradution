﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DreamerTool.Util;
using DreamerTool.UI;
using DG.Tweening;
public class PlayerView : MonoBehaviour
{
    public Text player_name_text;
    public Text player_level_text;
    public Text[] player_attribute_text;
  //  public Text player_attribute_text;
    private void Awake()
    {
        Init();

        EventManager.OnChangeLevel += UpdatePlayerLevel;
        EventManager.OnChangePlayerAttribute += UpdatePlayAttributeText;
    }
    private void OnDestroy()
    {
        EventManager.OnChangeLevel -= UpdatePlayerLevel;
        EventManager.OnChangePlayerAttribute -= UpdatePlayAttributeText;
    }
    public void UpdatePlayerLevel()
    {
        player_level_text.text ="LV  " +ActorModel.Model.GetLevel();
    }
    public void Init()
    {
        var attrubutes = Enum.GetNames(typeof(PlayerAttribute));

        for (int i = 0; i < attrubutes.Length; i++)
        {
            player_attribute_text[i].text = attrubutes[i] + ": \n" + ActorModel.Model.GetPlayerAttribute((PlayerAttribute)Enum.Parse(typeof(PlayerAttribute), attrubutes[i]));
        }
    }
    public void UpdatePlayAttributeText(PlayerAttribute attribute,double value)
    {
        StopAllCoroutines();

        var start = double.Parse(player_attribute_text[(int)attribute].text.Split('\n')[1]);

        StartCoroutine(TextAnim(player_attribute_text[(int)attribute], start, ActorModel.Model.GetPlayerAttribute(attribute), attribute + ": \n"));
      
        player_attribute_text[(int)attribute].transform.GetChild(0).gameObject.SetActive(false);
        player_attribute_text[(int)attribute].transform.GetChild(0).gameObject.SetActive(true);
         
        player_attribute_text[(int)attribute].transform.GetChild(0).GetComponent<Text>().text =DreamerUtil.GetColorRichText("("+ (value > 0?"+":"")+value +")",(value>0?Color.green:Color.red));
    }
    IEnumerator TextAnim(Text t,double value,double end,string c)
    {

        
        while (value  != end)
        {

            if (value > end)
                value--;
            else
                value++;

            t.text = c + value;

            yield return null;
        }
    }

}
