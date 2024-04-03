using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ItemChest : MonoBehaviour , IHitAction , IDamaged
{
    public int Index;
    public PhotonView mPhotonView;
    Animator mAnimator;
    public float HP;
    public float maxHP;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
        mPhotonView = GetComponent<PhotonView>();
        
    }
    private void OnEnable()
    {
        mAnimator.SetBool("Open", false);
        HP = maxHP;
    }
    [PunRPC]
    public void Dameged(int _damage)
    {
        if (HP <= 0)
            return;

        HP -= _damage;
        if (HP <= 0)
        {
            mAnimator.SetBool("Open", true);
            StartCoroutine(ItemRandomSpawnManager.Instance.RandomSpawn(Index));
        
        }
    }

   
    public IEnumerator IHit()
    {
        float shakeDuration = 0.5f;
        float shakeAmount = 0.01f;
        float elapsed = 0.0f;
        Vector3 originalPosition = transform.localPosition;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            // 캐릭터 컨트롤러의 Move 함수를 사용하여 떨림 위치로 이동
            // Move 함수는 상대적 이동을 수행하므로, 현재 위치에서의 차이를 계산해 이동시킨다.
            transform.localPosition = (randomPoint - transform.localPosition + randomPoint);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }

}
