﻿/*****************************
Created by 师鸿博
*****************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;
public class ArmConfig : ItemConfig<ArmConfig>
{
 
    [Button("保存", 50)]
    public override void Save()
    {
        
       
        TextAsset ta = Resources.Load<TextAsset>("all_config");
        JsonData jd = JsonMapper.ToObject(ta.text);
        if (jd["Arm"] == null)
            jd["Arm"] = new JsonData();
        JsonData data = new JsonData();
        data["物品ID"] = 物品ID;
        data["物品名字"] = 物品名字;
        data["物品描述"] = 物品描述;
        data["图标名字"] = 编辑器图标 ? AssetDatabase.GetAssetPath(编辑器图标).Substring(0, AssetDatabase.GetAssetPath(编辑器图标).Length-4).Substring(17) : "";
        data["物品阶级"] = (int)物品阶级;
        jd["Arm"][物品ID.ToString()] = data;
        using (StreamWriter sw = new StreamWriter(new FileStream("Assets/Resources/all_config.json", FileMode.Truncate)))
        {
            sw.Write(jd.ToJson());
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
        ItemEditorWindow._window.ForceMenuTreeRebuild();
        ItemEditorWindow._window.isCreate = false;
        ItemEditorWindow._window._tree.MenuItems[ItemEditorWindow._window._tree.MenuItems.Count - 1].Select();
#endif
    }
    public override Sprite GetSprite()
    {
        return Resources.Load<Sprite>(图标名字);
    }
    public   Sprite GetLsprite()
    {
        return Resources.Load<Sprite>(图标名字.Substring(0,图标名字.Length-1)+"L");
    }


}
