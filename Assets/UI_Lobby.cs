using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    public InputField NicknameInputFieldUI, RoomnameInputFieldUI;
    public GameObject[] ManObj;
    public GameObject[] WoManObj;
    public void OnClickMakeRoomButton()
    {
        string nickname = NicknameInputFieldUI.text;
        string roomID = RoomnameInputFieldUI.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomID))
        {
            Debug.Log("입력하세요.");
            return;
        }

        PhotonNetwork.NickName = nickname;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;   // 입장 가능한 최대 플레이어 수
        roomOptions.IsVisible = true; // 로비에서 방 목록에 노출할 것인가?
        roomOptions.IsOpen = true; // 방에 다른 플레이어가 들어올 수 있는가?
        roomOptions.EmptyRoomTtl = 1000 * 20;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "MasterNickName",nickname}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "MasterNickName" };

        PhotonNetwork.JoinOrCreateRoom(roomID, roomOptions, TypedLobby.Default);
    }
    public void ButtonActionCharacterTypeSet()
    {
        if (PhotonManager.instance.characterType == CharacterType.man)
        {
            for (int i = 0; i < ManObj.Length; i++)
            {
                ManObj[i].SetActive(false);
                WoManObj[i].SetActive(true);
            }
            Debug.Log(PhotonManager.instance.characterType);
            PhotonManager.instance.characterType = CharacterType.woman;
        }
        else
        {
            for (int i = 0; i < ManObj.Length; i++) 
            {
                ManObj[i].SetActive(true);
                WoManObj[i].SetActive(false);
            }
            Debug.Log(PhotonManager.instance.characterType);
            PhotonManager.instance.characterType = CharacterType.man;
        }
    }

    public void OnNickNameValueChanged(string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            return;

        }
        PhotonNetwork.NickName = newValue;
    }

}