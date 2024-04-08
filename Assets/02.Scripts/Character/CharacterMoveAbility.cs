using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharacterMoveAbility : CharacterAbility
{
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float rotationSpeed = 10.0f; // 캐릭터 방향 회전속도

 

    public float _yVelocity;
    public float gravity = -9.81f;
    private int staminaDepletionTate = 10;
    private bool isRunnig = false;
    private bool groundedPlayer;

    private CharacterController controller;
    private float footStpeCoolTime;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!_owner.PhotonView.IsMine)
            return;

        if (!_owner.state.isDed)
        {
            Jump();
            CharacterMove();
        }
        HandleStamina();
    }
    private void CharacterMove()
    {
        // WASD 키 입력 받기
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        movement.Normalize();
        _owner.mAnimator.SetFloat("Move", movement.magnitude);
        movement = Camera.main.transform.TransformDirection(movement);

        //중력
        movement.y = _yVelocity;
        _yVelocity += gravity * Time.deltaTime;


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
        //인풋 있을때만 실행
        if (moveHorizontal != 0f || moveVertical!= 0f)
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
        }
        controller.Move(movement * (_owner.state.MoveSpeed * Time.deltaTime));
        // 캐릭터 이동 적용
    }
    public void Jump()
    {
        groundedPlayer = controller.isGrounded;
        if (Input.GetButtonDown("Jump") && groundedPlayer && _owner.state.Stamina > 10)
        {
            _owner.state.Stamina -= 10;
            _yVelocity = _owner.state.JumpPower;
        }
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
}