using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    protected Character _owner;

    private void Awake()
    {   
        _owner = GetComponent<Character>();
        
    }
}
