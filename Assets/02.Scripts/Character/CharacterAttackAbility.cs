using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackAbility : CharacterAbility
{
    public Animator mAnimator;
    float coolTime =1;

    // Update is called once per frame
    void Update()
    {
        coolTime += Time.deltaTime;
        if (coolTime > _owner.state.AttackCoolTime && Input.GetMouseButtonDown(0))
        {
            mAnimator.SetTrigger($"Attack0{UnityEngine.Random.RandomRange(1,4)}");
            coolTime = 0;
        }

    }
}
