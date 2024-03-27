using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CharacterAttackAbility : CharacterAbility
{
    public Animator mAnimator;
    float coolTime =1;
    private int mAttackStamina =20;
    // Update is called once per frame
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
            return;

        coolTime += Time.deltaTime;
        if (coolTime > _owner.state.AttackCoolTime && Input.GetMouseButtonDown(0) && _owner.state.Stamina > mAttackStamina)
        {
            _owner.state.Stamina -= mAttackStamina;

            mAnimator.SetTrigger($"Attack0{UnityEngine.Random.RandomRange(1,4)}");
            coolTime = 0;
        }

    }
}
