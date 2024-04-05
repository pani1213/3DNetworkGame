using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvasAbility : CharacterAbility//, IPunObservable
{
    Canvas myCanvas;
    public Text NickNameTextUI;
    public Image HP_BarUI, Staminar_BarUI;
    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
        _owner = GetComponentInParent<Character>();
        NickNameTextUI.text = _owner.PhotonView.Controller.NickName;
    }
    private void Update()
    {
        Billboard();
        HP_BarUI.fillAmount = _owner.state.Health / _owner.state.MaxHealth;
        Staminar_BarUI.fillAmount = _owner.state.Stamina / _owner.state.MaxStamina;
    }
    private void Billboard()
    {
        transform.forward = Camera.main.transform.forward;
    }

}
