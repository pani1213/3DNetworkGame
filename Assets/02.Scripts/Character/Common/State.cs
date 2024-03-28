using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State 
{
    public int MaxHealth;
    public float Health;
    public int Damage;
    public float MaxStamina;
    public float Stamina;
    public float MoveSpeed;
    public float RunSpeed;
    public float RotationSpeed;
    public float AttackCoolTime;
    public float StaminaRecovery;
    public bool isDed;

    public void InIt()
    {
        Health = MaxHealth;
        Stamina = MaxStamina;
        isDed = false;
    }

}

