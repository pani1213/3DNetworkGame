using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.NickName = $"96년생_정성훈_{UnityEngine.Random.Range(0,100)}";
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 30;


    }
    public override void OnConnected()
    {
        //Debug.Log("서버 접속");
        //Rehion
        //PhotonNetwork.ConnectToRegion("kr");
        //Debug.Log(PhotonNetwork.CloudRegion);
    }
  
    public override void OnConnectedToMaster()
    {
        //Debug.Log("마스터 서버 접속");
        //Debug.Log($"InLobby?: {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public override void OnJoinedLobby()
    {
        //Debug.Log("로비 입장");

        PhotonNetwork.JoinOrCreateRoom("test", null, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        //Debug.Log("방 생성");
        //Debug.Log(PhotonNetwork.CurrentRoom.Name);   
    }
    public override void OnJoinedRoom()
    {
        GameObject player =  PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity);
        PlayerFindManager.Instance.character = player;
        PlayerFindManager.Instance.InIt();
        
    }


}
