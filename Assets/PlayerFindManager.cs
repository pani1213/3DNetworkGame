using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFindManager : Singleton<PlayerFindManager>
{
    public GameObject character;
    public Character playerCharacter;
    public CinemachineFreeLook freeLookCamera;
    public void InIt()
    {
        playerCharacter = character.GetComponent<Character>();
        freeLookCamera.Follow = character.transform;
        freeLookCamera.LookAt = character.transform;
        CharacterStateUI.Instance.InIt();
    }
}
