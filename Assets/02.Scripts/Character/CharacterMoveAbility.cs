using Photon.Pun;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMoveAbility : CharacterAbility
{
    public float rotationSpeed = 10.0f; // 캐릭터 방향 회전속도
    public float gravity = -9.81f;
    private int staminaDepletionTate = 10;
    private bool isRunnig = false;

    private CharacterController controller;
    private Animator mAnimator;
    private Vector3 velocity;

    private float footStpeCoolTime;
    void Start()
    {
       
        mAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        if (!_owner.PhotonView.IsMine)
            return;

        CharacterMove();
        Gravity();
        HandleStamina();
    }
    private void CharacterMove()
    {
        

        // WASD 키 입력 받기
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement.Normalize();
        mAnimator.SetFloat("Move", movement.magnitude);
        movement = Camera.main.transform.TransformDirection(movement);

        if (Input.GetKey(KeyCode.LeftShift) && _owner.state.Stamina > 0)
        {
            isRunnig = true;
            movement *= _owner.state.RunSpeed;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                isRunnig = false;

            movement *= _owner.state.MoveSpeed;
        }

        // 움직임 벡터 계산

        if (movement.magnitude >= 0.1f)
        {
            footStpeCoolTime += Time.deltaTime;
            if (footStpeCoolTime > 0.5f)
            {
                GameObject VFX = ObjectPooler.instance.GetPoolObject("VFX_FootStep");
                VFX.SetActive(true);
                VFX.transform.position = transform.position;
                footStpeCoolTime = 0;
            }
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
            controller.Move(movement * Time.deltaTime);
        }
        

        // 캐릭터 이동 적용
    }
    private void HandleStamina()
    {
        if (isRunnig)
        {
            _owner.state.Stamina -= staminaDepletionTate * Time.deltaTime;
            _owner.state.Stamina = Mathf.Max(_owner.state.Stamina, 0);
        }
        else
        {
            _owner.state.Stamina += _owner.state.StaminaRecovery * Time.deltaTime;
            _owner.state.Stamina = Mathf.Min(_owner.state.Stamina, _owner.state.MaxStamina);
        }
    }
    private void Gravity()
    {
        if (controller.isGrounded)
            velocity.y = 0f; // 땅에 닿으면 수직 속도를 0으로 리셋
        else
            velocity.y += gravity * Time.deltaTime; // 땅에 닿아 있지 않으면 중력을 계속 적용
        // 중력에 의한 움직임 적용
        controller.Move(velocity * Time.deltaTime);
    }
}
