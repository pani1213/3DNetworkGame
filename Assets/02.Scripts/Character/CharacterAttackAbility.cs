using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;


public class CharacterAttackAbility : CharacterAbility
{
    float coolTime =1;
    private int mAttackStamina =20;
    public Collider wearponCollider;

    List<IDamaged> damageds = new List<IDamaged>();
    // Update is called once per frame
    void Update()
    {
        if (!_owner.PhotonView.IsMine || _owner.state.isDed)
            return;
        coolTime += Time.deltaTime;
        if (coolTime > _owner.state.AttackCoolTime && Input.GetMouseButtonDown(0) && _owner.state.Stamina > mAttackStamina)
        {
            _owner.state.Stamina -= mAttackStamina;
            coolTime = 0;

            if(_owner.controller.isGrounded)
            _owner.PhotonView.RPC(nameof(PlayAttackAnimation),RpcTarget.All,UnityEngine.Random.Range(1,4));       
            else
            _owner.PhotonView.RPC(nameof(JumpAttack),RpcTarget.All);       
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
            GameObject vfxObj = PhotonNetwork.Instantiate("HitVFX",((other.transform.position + transform.position) / 2f) + (Vector3.up * 0.7f), Quaternion.identity);
            vfxObj.GetComponent<ParticleSystem>().Play();

            PhotonView photonView = other.GetComponent<PhotonView>();

         
            if (photonView != null)
            {
                Character otherChar;
                int score = 1;
                if (TryGetComponent<Character>(out otherChar))
                {
                    score = otherChar.myScore;
                }

                photonView.RPC("Dameged", RpcTarget.All, _owner.state.Damage);


                if (otherChar.state.Health <= 0)
                {
                    if (_owner.PhotonView.IsMine)
                    {
                       // SetProperties();
                        if (otherChar != null)
                        { 
                            ExitGames.Client.Photon.Hashtable myHashtable = PhotonNetwork.LocalPlayer.CustomProperties;                        
                            myHashtable["Score"] = (int)myHashtable["Score"] + score/2;
                            PhotonNetwork.SetPlayerCustomProperties(myHashtable);
                        }
                    }
                }
            }
        }
    }
    private void SetProperties()
    {
        ExitGames.Client.Photon.Hashtable myHashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        myHashtable["KillCount"] = (int)myHashtable["KillCount"] + 1;
        Debug.Log($"{myHashtable["KillCount"]} , killcount");
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHashtable);
    }
    [PunRPC]
    public void PlayAttackAnimation(int index)
    {
        _owner.mAnimator.SetTrigger($"Attack0{index}");
    }
    [PunRPC]
    public void JumpAttack()
    {
        _owner.mAnimator.SetTrigger($"Attack04");
    }
}
