using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAttackAbility : CharacterAbility
{
    public Animator mAnimator;
    float coolTime =1;
    private int mAttackStamina =20;
    public Collider wearponCollider;

    List<IDamaged> damageds = new List<IDamaged>();
    // Update is called once per frame
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
            return;
        coolTime += Time.deltaTime;
        if (coolTime > _owner.state.AttackCoolTime && Input.GetMouseButtonDown(0) && _owner.state.Stamina > mAttackStamina)
        {
            _owner.state.Stamina -= mAttackStamina;
            coolTime = 0;
            _owner.PhotonView.RPC(nameof(PlayAttackAnimation),RpcTarget.All,UnityEngine.Random.Range(1,4));       
        }
    }
    public void ActiveCollder()
    {
        wearponCollider.enabled = true;
    }
    public void InActiveCollider()
    {
        wearponCollider.enabled = false;
        damageds.Clear();
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.transform == transform || !_owner.PhotonView.IsMine)
            return;

        IDamaged obj;
        if (other.TryGetComponent<IDamaged>(out obj))
        {
            if (damageds.Contains(obj))
                return;

            damageds.Add(obj);

            GameObject vfxObj = PhotonNetwork.Instantiate("HitVFX", Vector3.zero,Quaternion.identity);
            vfxObj.transform.position = ((other.transform.position + transform.position) / 2f) + (Vector3.up * 0.7f);
            vfxObj.GetComponent<ParticleSystem>().Play();


            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null)
            {
                photonView.RPC("Dameged", RpcTarget.All, _owner.state.Damage);
            }
        }
    }
    [PunRPC]
    public void PlayAttackAnimation(int index)
    {
        mAnimator.SetTrigger($"Attack0{index}");
    }
}
