using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameSettingUI : MonoBehaviour
{
    public InputField nickNameInput;

    public void ButtonAction_SetNickName()
    {
        if (!PhotonManager.instance.isJoindRoom)
            return;

        if (nickNameInput.text != "")
            PhotonNetwork.NickName = nickNameInput.text;
        else
            PhotonNetwork.NickName = $"검객_{UnityEngine.Random.Range(0, 100)}";

        PhotonManager.instance.CreatedCharacter();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        gameObject.SetActive(false);
    }
}
