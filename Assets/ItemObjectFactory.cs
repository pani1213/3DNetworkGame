using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))] 
public class ItemObjectFactory : Singleton<ItemObjectFactory>
{
    private PhotonView photonView;
    private void Awake()
    {
        photonView =GetComponent<PhotonView>();
    }
    public void RequestCreate(Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
            Create(position);
        else
        {
            photonView.RPC(nameof(Create), RpcTarget.MasterClient, position);
        }

    }
    public void RequestDelete(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
            Delete(viewID);
        else
        {
            photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewID);
        }

    }
    [PunRPC]
    private void Create( Vector3 position)
    {
        int random = Random.Range(0, 10);
        switch (random)
        {
            case 0:case 1:case 2:case 3:case 4:case 5:case 6:
                for (int i = 0; i < Random.Range(7,15); i++)
                PhotonNetwork.InstantiateRoomObject("Coin", position + Random.insideUnitSphere, Quaternion.identity);
                break;
            case 7:case 8:
                for (int i = 0; i < Random.Range(2,5); i++)
                PhotonNetwork.InstantiateRoomObject("HP", position + Random.insideUnitSphere, Quaternion.identity);
                break;
            case 9:
                for (int i = 0; i < Random.Range(2,5); i++)
                PhotonNetwork.InstantiateRoomObject("Stamina", position + Random.insideUnitSphere, Quaternion.identity);
                break;
        }

    }
    [PunRPC]
    private void Delete(int viewID)
    {
        GameObject delete = PhotonView.Find(viewID).gameObject;
        if (delete != null)
            PhotonNetwork.Destroy(delete);
    }
  // public void DropItem(Vector3 transform)
  // {
  //     Vector3 dropos = transform + new Vector3(0, 0.5f, 0) + Random.insideUnitSphere;
  //     Vector3 dropos2 = transform + new Vector3(0, 0.5f, 0) + Random.insideUnitSphere;
  //
  //     PhotonNetwork.InstantiateRoomObject("HP", dropos, Quaternion.identity);
  //     PhotonNetwork.InstantiateRoomObject("Stamina", dropos2, Quaternion.identity);
  // }
}
