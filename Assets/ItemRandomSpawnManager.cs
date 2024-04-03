using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class ItemRandomSpawnManager : Singleton<ItemRandomSpawnManager>
{
    public Transform[] RandomPos;
    public Dictionary<int, bool> ChestPos;
    public ItemChest[] itemChests;

    public bool IsOn = false;

    PhotonView PhotonView;
    private void Awake()
    {
        PhotonView  =   GetComponent<PhotonView>();
    }
    private void Start()
    {
        ChestPos= new Dictionary<int, bool>();
        for (int i = 0; i < RandomPos.Length; i++)
        {
            ChestPos.Add(i,  false );
            itemChests[i].Index = i;
        }
    }
    private void Update()
    {
        if (!IsOn && PhotonNetwork.IsMasterClient)
        {
            //itemChests[UnityEngine.Random.Range(0, itemChests.Length)].gameObject.SetActive(true);
            //IsOn = true;
            Debug.Log("리스폰");
            int random= UnityEngine.Random.Range(0, itemChests.Length);
            PhotonView.RPC(nameof(ChestSetActive), RpcTarget.AllBuffered, true, random);
        }
    }
    [PunRPC]
    public void ChestSetActive(bool _onOff, int index)
    {
        itemChests[index].gameObject.SetActive(_onOff);
        IsOn = true;
    }
    public IEnumerator RandomSpawn(int index)
    {
        int count = 0;
        itemChests[index].GetComponent<Rigidbody>().isKinematic = true;
  
        while (count < 80)
        {
            yield return new WaitForSeconds(0.01f);
            ItemObjectFactory.Instance.RequestCreateOneType(itemChests[index].transform.position,"Coin");
            count++;
        }
        itemChests[index].GetComponent<Rigidbody>().isKinematic = false;
        PhotonView.RPC(nameof(SetActiveFalse), RpcTarget.AllBuffered, index);
       
        
    }
    [PunRPC]
    public void SetActiveFalse( int index)
    {
        itemChests[index].gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient && PhotonView.IsMine)
        {
            ChestPos[index] = false;
            IsOn = false;
        }
    }
}
public struct ChestPosData
{
    public bool isOn;
}
