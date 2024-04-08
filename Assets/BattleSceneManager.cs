using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    void Start()
    {
        RespawnerManager.Instance.RespawnCharacter();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
