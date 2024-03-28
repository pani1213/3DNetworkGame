using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RespawnerManager : Singleton<RespawnerManager>
{
    public Transform[] respawnerPos;

    public void RespawnCharacter()
    {
        GameObject player = PhotonNetwork.Instantiate("Character", respawnerPos[UnityEngine.Random.Range(0, respawnerPos.Length)].transform.position, Quaternion.identity);
        PlayerFindManager.Instance.character = player;
        PlayerFindManager.Instance.InIt();
    }
    public void Respawn(CharacterController _player,GameObject _gameObject)
    {
        Debug.Log(UnityEngine.Random.Range(0, respawnerPos.Length));
        _player.enabled = false;
        _gameObject.transform.position = respawnerPos[UnityEngine.Random.Range(0, respawnerPos.Length)].transform.position;
        _player.enabled = true;
    }
}
