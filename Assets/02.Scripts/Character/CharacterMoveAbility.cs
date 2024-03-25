using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMoveAbility : CharacterAbility
{
    public float rotationSpeed = 10.0f; // 캐릭터 회전속도
    public float gravity = -9.81f;

    private CharacterController controller;
    public Animator mAnimator;
    private Vector3 velocity;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        CharacterMove();
        Gravity();
    }
    private void CharacterMove()
    {
        // WASD 키 입력 받기
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // 움직임 벡터 계산
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement.Normalize();
        mAnimator.SetFloat("Move", movement.magnitude);
        movement = Camera.main.transform.TransformDirection(movement);
        movement *= _owner.state.MoveSpeed;


        if (movement.magnitude >= 0.1f)
        {
      
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        

            controller.Move(movement * Time.deltaTime);
        }
        

        // 캐릭터 이동 적용
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
