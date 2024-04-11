using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_Room : MonoBehaviour
{
    public Text RoomNameTextUI, NicknameTextUI, PlayerCountTextUI;

    RoomInfo roomInfo;
    public void Set(RoomInfo room)
    {
        roomInfo = room;
        RoomNameTextUI.text = room.Name;// "방제목";

        if(null!=room.CustomProperties["MasterNickName"])
        NicknameTextUI.text = room.CustomProperties["MasterNickName"].ToString();//PhotonNetwork.GetPhotonView(room.masterClientId).name;// "방장닉";
        PlayerCountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";// "1/20";
    }
    public void OnClickRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
