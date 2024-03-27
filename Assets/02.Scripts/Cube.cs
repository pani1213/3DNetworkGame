using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Rigidbody rigidbody;
    private void OnEnable()
    {

        Invoke("off", 3f);
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
