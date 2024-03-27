using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(CharacterMoveAbility))]
[RequireComponent(typeof(CharacterRotateAbility))]
[RequireComponent(typeof(CharacterAttackAbility))]
public class Character : MonoBehaviour , IPunObservable
{
    public PhotonView PhotonView;
    public State state;

    Vector3 recevedPosition;
    Quaternion recevedRotation;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(state.Health);
            stream.SendNext(state.Stamina);
        }
        else if (stream.IsReading)
        {
            recevedPosition = (Vector3)stream.ReceiveNext();
            recevedRotation = (Quaternion)stream.ReceiveNext();
            state.Health = (int)stream.ReceiveNext();
            state.Stamina = (float)stream.ReceiveNext();
        }
      
    }

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();

    }

    public void Update()
    {
        if (!PhotonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, recevedPosition, Time.deltaTime * 20);
            transform.rotation = Quaternion.Slerp(transform.rotation, recevedRotation, Time.deltaTime * 20);
        }


    }
}
