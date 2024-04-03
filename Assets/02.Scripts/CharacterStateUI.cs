using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateUI : Singleton<CharacterStateUI>
{

    public Image HpBar_UI, StaminaBar_UI;
    public Character _Character;

    public void InIt()
    {
        _Character = PlayerFindManager.Instance.playerCharacter;
    }
    public void Update()
    {
        if (_Character == null)
        {
            return;
        }
        HpBar_UI.fillAmount = _Character.state.Health/ _Character.state.MaxHealth;
        StaminaBar_UI.fillAmount = _Character.state.Stamina / _Character.state.MaxStamina;
    }
}
 