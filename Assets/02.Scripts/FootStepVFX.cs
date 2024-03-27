using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepVFX : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("off", 1f);
    }
    void off()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        ObjectPooler.instance.ObjectInPool(this.gameObject);
    }
}
