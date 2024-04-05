using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class MonsterController : MonoBehaviour, IDamaged 
{
    private enum EnemeyState
    {
        Idle,
        Patrol,
        Trace,
        Attack,
        Return,
        Damaged,
        Die
    }
    public int hp = 15;


    private EnemeyState _currentState;
    public float FindDistance = 8f;
    public Transform _playerTransform;
    // 공격 가능 범위
    public float AttackDistance = 2f; 
    public float MoveSpeed = 5f;
    private CharacterController controller;

    private float _currentTime;
    private float _attackDelay = 2f;

    private Vector3 _originPos;		// 초기 위치 저장용 변수
    public float MoveDistance = 20f;    // 이동 가능 범위
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        // 최초의 에너미 상태는 '대기'
        _currentState = EnemeyState.Idle;
        _originPos = transform.position; // 자신의 초기 위치 저장하기
        //_playerTransform = PlayerFindManager.Instance.character.transform;


    }

    private void Update()
    {
        if (_playerTransform == null)
            return;
        // 현재 상태를 체크해 해당 상태별로 정해진 기능을 수행하게 하고 싶다.
        switch (_currentState)
        {
            case EnemeyState.Idle:
                Idle();
                break;
            case EnemeyState.Patrol:
                Patrol();
                break;
            case EnemeyState.Trace:
                Trace();
                break;
            case EnemeyState.Attack:
                Attack();
                break;
            case EnemeyState.Return:
                Return();
                break;
            case EnemeyState.Damaged:
                Damaged();
                break;
            case EnemeyState.Die:
                Die();
                break;
        }
    }

    public void Idle()
    {
    

        if (Vector3.Distance(transform.position, _playerTransform.position) < FindDistance)
        {
            _currentState = EnemeyState.Trace;
            Debug.Log("상태 전환: Idle -> Trace");
        }
    }
    public void Patrol()
    {
        
    }
    public void Trace()
    {
        if (Vector3.Distance(transform.position, _originPos) > MoveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환한다.
            _currentState = EnemeyState.Return;
        }
        else if (Vector3.Distance(transform.position, _playerTransform.position) > AttackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (_playerTransform.position - transform.position).normalized;

            // 캐릭터 콘트롤러를 이용해 이동하기
            controller.Move(dir * MoveSpeed * Time.deltaTime);
        }
        else
        {
            _currentState = EnemeyState.Attack;
        }
    } 
    public void Attack()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) < AttackDistance)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _attackDelay)
            {
                print("공격");
                _playerTransform.GetComponent<IDamaged>().Dameged(10);
                _currentTime = 0;
            }
            
        }
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환한다(재추격 실시).
        else
        {
            _currentState = EnemeyState.Trace;
            Debug.Log("상태 전환: Attack -> Trace");
            _currentTime = _attackDelay;
        }
    }
    public void Return()
    {
        // 만일, 초기 위치에서의 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동한다.
        if (Vector3.Distance(transform.position, _originPos) > 0.1f)
        {
            Vector3 dir = (_originPos - transform.position).normalized;
            controller.Move(dir * MoveSpeed * Time.deltaTime);
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환한다.
        else
        {
            transform.position = _originPos;

            // hp를 다시 회복한다.
            // hp = maxHp;
            _currentState = EnemeyState.Idle;
            Debug.Log("상태 전환: Return -> Idle");
        }
    }  
    public void Damaged()
    {
        StartCoroutine(IHit());
    } 
    public void Die()
    {
        StopAllCoroutines();
        StartCoroutine(Die_Coroutine());
    }
    IEnumerator Die_Coroutine()
    {
        controller.enabled = false;

        // 2초 동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        Debug.Log("소멸!");
        Destroy(gameObject);
    }
    public void Dameged(int _damage)
    {
        if (_currentState == EnemeyState.Damaged || _currentState == EnemeyState.Die || _currentState == EnemeyState.Return)
        {
            return;
        }

        hp -= _damage;
        if (hp > 0)
        {
            _currentState = EnemeyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환한다.
        else
        {
            _currentState = EnemeyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }

    } 
    public IEnumerator IHit()
    {
        yield return new WaitForSeconds(0.5f);
        _currentState = EnemeyState.Trace;
    }

    public void Dameged(int _damage, int Actnum)
    {
        throw new System.NotImplementedException();
    }
}
