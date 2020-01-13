﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorState : MonoBehaviour
{
    public bool isMoveable = true;
    public bool isInputable = true;
    public bool isGround = true;
    public bool isJump { get; set; }
    public bool isAttack { get; set; }
    public bool isDash { get; set; }
    public bool isMoveRight { get; set; }
    public bool isMoveLeft { get; set; }
    public bool isAttackUp { get; set; }
    // Start is called before the first frame update
}
