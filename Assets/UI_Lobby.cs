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
        PhotonNetwork.JoinOrCreateRoom(roomID, null, TypedLobby.Default);
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

}