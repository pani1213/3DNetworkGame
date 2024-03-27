using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CharacterAttackAbility _CharacterAttackAbility;
    private void OnTriggerEnter(Collider other)
    {
        _CharacterAttackAbility.OnTriggerEnter(other);
    }
}
