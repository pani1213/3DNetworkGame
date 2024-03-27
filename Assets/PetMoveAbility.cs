using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PetMoveAbility : MonoBehaviour
{
    public float gravity = -9.81f;
    private CharacterController controller;
    private Animator mAnimator;
    private Vector3 velocity;

    public int stopDistance = 3;
    public int speed = 5;
    public float rotationSpeed = 5.0f;

    private bool isFly;
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Gravity();
        FollowPlayer();
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

    private void FollowPlayer()
    {
        if (PlayerFindManager.Instance.character == null)
            return;


        float distance = Vector3.Distance(transform.position, PlayerFindManager.Instance.character.transform.position);

        Vector3 direction = (PlayerFindManager.Instance.character.transform.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero) // 방향 벡터가 0이 아닐 때만 회전 처리
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        isFly = false;
        mAnimator.SetBool("isFly", isFly);
        if (distance > stopDistance)
        {
            isFly= true;
            mAnimator.SetBool("isFly", isFly);
            // 지형을 따라 이동하려면, 수평 방향의 움직임에만 집중하고, Y 축은 변경하지 않습니다.
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
