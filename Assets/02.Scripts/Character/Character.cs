using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterMoveAbility))]
[RequireComponent(typeof(CharacterRotateAbility))]
[RequireComponent(typeof(CharacterAttackAbility))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour , IPunObservable , IDamaged, IHitAction 
{
    public PhotonView PhotonView;
    public State state;
    [SerializeField] private CinemachineImpulseSource _source;
    public Animator mAnimator;
    public CharacterController controller;
    public Collider CharacterCollider;
    Vector3 recevedPosition;
    Quaternion recevedRotation;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            stream.SendNext(state.Health);
            stream.SendNext(state.Stamina);
        }
        else if (stream.IsReading)
        {
            //recevedPosition = (Vector3)stream.ReceiveNext();
            //recevedRotation = (Quaternion)stream.ReceiveNext();
            state.Health = (float)stream.ReceiveNext();
            state.Stamina = (float)stream.ReceiveNext();
        } 
    }
    private void Awake()
    { 
        PhotonView = GetComponent<PhotonView>();
        mAnimator = GetComponent<Animator>();
        CharacterCollider = GetComponent<Collider>();
    }
    public void Update()
    {
        if (transform.position.y <= -10f && PhotonView.IsMine && !state.isDed)
        {

            Died();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RespawnerManager.Instance.Respawn(GetComponent<CharacterController>(), gameObject);
        }
       //if (!PhotonView.IsMine)
       //{
       //    transform.position = Vector3.Lerp(transform.position, recevedPosition, Time.deltaTime * 20);
       //    transform.rotation = Quaternion.Slerp(transform.rotation, recevedRotation, Time.deltaTime * 20);
       //}
    }
    [PunRPC]
    public void Dameged(int _damage)
    {
        state.Health -= _damage;


        if (PhotonView.IsMine)
        { 
            _source.GenerateImpulse();
            StartCoroutine(HitUIManager.Instance.FadeImageCoroutine());
        }
        StartCoroutine(IHit());
        if (state.Health <= 0)
            Died();
        
    }
    public void Died()
    {
        mAnimator.SetBool("IsDed", true);

        state.isDed = true;
  
        StartCoroutine(DiedAction());
    }
    public IEnumerator DiedAction()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonView.RPC("ActiveObject", RpcTarget.All,false);

        if(PhotonView.IsMine)
        ItemObjectFactory.Instance.RequestCreate(transform.position);
        yield return new WaitForSeconds(1f); // 리스폰 시간
        //PhotonView.RPC("Respawn", RpcTarget.All,gameObject);
        RespawnerManager.Instance.Respawn(GetComponent<CharacterController>(),gameObject);
        PhotonView.RPC("OffDedAni", RpcTarget.All);
        state.InIt();
        PhotonView.RPC("ActiveObject", RpcTarget.All,true);
    }
    [PunRPC]
    private void ActiveObject(bool _onAndOff)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(_onAndOff);
        CharacterCollider.enabled = _onAndOff;
    }
    [PunRPC]
    private void OffDedAni()
    {
        mAnimator.SetBool("IsDed", false);
    }
    public IEnumerator IHit()
    {
        float shakeDuration = 0.5f;
        float shakeAmount = 0.01f;
        float elapsed = 0.0f;
        Vector3 originalPosition = transform.GetChild(0).localPosition;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            // 캐릭터 컨트롤러의 Move 함수를 사용하여 떨림 위치로 이동
            // Move 함수는 상대적 이동을 수행하므로, 현재 위치에서의 차이를 계산해 이동시킨다.
            transform.GetChild(0).localPosition = (randomPoint - transform.GetChild(0).localPosition + randomPoint);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.GetChild(0).localPosition = originalPosition;
    }


}
