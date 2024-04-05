using Photon.Pun;
using UnityEngine;

public class CharacterItemGetAbility : CharacterAbility
{
    public ParticleSystem ParticleSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (!_owner.PhotonView.IsMine || _owner.state.isDed)
                return;
            IncreaseStat increaseStat = other.GetComponent<IItemUse>().UseItem();

            _owner.PhotonView.RPC(nameof(PlayParticle), RpcTarget.All);

            switch (increaseStat.ItemType)
            {
                case ItemType.hp:
                    _owner.state.Health += increaseStat.Value;
                    if (_owner.state.Health > _owner.state.MaxHealth)
                        _owner.state.Health = _owner.state.MaxHealth;
                        Debug.Log(_owner.state.Health);
                    break;
                case ItemType.stamina:
                    _owner.state.Stamina += increaseStat.Value;
                    if(_owner.state.Stamina  > _owner.state.MaxStamina)
                        _owner.state.Stamina = _owner.state.MaxStamina;
                        Debug.Log(_owner.state.Stamina);
                    break;
                case ItemType.coin:
                    _owner.AddScore((int)increaseStat.Value);
                    //_owner.state.Score += increaseStat.Value;
                
                    break;
            }
        }
    }

    [PunRPC]
    public void PlayParticle()
    {

        ParticleSystem.Play();
    }

}
