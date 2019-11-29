﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Sword : MonoBehaviour
{
    private Animator _anim;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            Camera.main.DOShakePosition(0.1f, 1);
            
          
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale!=1)
        {
            timer += Time.unscaledDeltaTime;
            if(timer>=0.15f)
            {
               
                timer = 0;
                Time.timeScale = 1f;
            }
        }
    }
}
