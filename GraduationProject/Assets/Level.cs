﻿/*****************************
Created by 师鸿博
*****************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DreamerTool.UI;
public class Level : MonoBehaviour
{
    int index = 0;
    bool isRotate = false;
    float rotateY;
     
    public List<LevelIInfo> levelInfo = new List<LevelIInfo>();
 
    public void NextLevel()
    {
        isRotate = true;
        levelInfo[index].Leave();
        index++;
        levelInfo[index].Enter();
    }
    private void Update()
    {
        if (isRotate)
        {
            var value = Mathf.Lerp(0, 90, Time.deltaTime);
            rotateY += value;
            if (rotateY >= 90)
            {
                rotateY = 0;
                isRotate = false;
                return;
            }

            transform.RotateAround(ActorController.Controller.transform.position, transform.up, value);
        }
    }
}
[System.Serializable]
public class LevelIInfo
{
    public GameObject door;

    public void Leave()
    {
        door.SetActive(false);
    }
    public void Enter()
    {
        door.SetActive(true);
    }
}
