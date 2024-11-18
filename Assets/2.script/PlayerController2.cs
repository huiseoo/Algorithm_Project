using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI Text를 사용하기 위해 필요

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도
    public Animator animator;   // 애니메이터 컴포넌트
    private Rigidbody2D rb;     // Rigidbody2D 컴포넌트
    private Vector2 movement;   // 플레이어의 이동 방향

    // Gem 카운트 변수
    private int gemCount = 0;

    // UI 텍스트 컴포넌트
    public Text gemCountText;  // 이 필드에 Text UI를 연결하여 카운트를 화면에 표시

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        UpdateGemCountText();  // 시작 시 카운트 텍스트 업데이트
    }

    void Update()
    {
        // 방향키 입력 처리
        movement = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") != 0) // 왼쪽 또는 오른쪽 입력
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = 0; // 대각선 방지: 수평 입력이 있을 경우 수직 입력 무시
        }
        else if (Input.GetAxisRaw("Vertical") != 0) // 위 또는 아래 입력
        {
            movement.y = Input.GetAxisRaw("Vertical");
            movement.x = 0; // 대각선 방지: 수직 입력이 있을 경우 수평 입력 무시
        }

        // 애니메이션 업데이트
        if (movement != Vector2.zero)
        {
            UpdateAnimationDirection(movement);
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 사용해 이동 처리
        rb.velocity = movement * moveSpeed;
    }

    void UpdateAnimationDirection(Vector2 direction)
    {
        // 이동 방향에 따라 애니메이션 설정
        if (direction == Vector2.up)
        {
            animator.Play("up");
        }
        else if (direction == Vector2.down)
        {
            animator.Play("down");
        }
        else if (direction == Vector2.right)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽 이동 시 플립 비활성화
        }
        else if (direction == Vector2.left)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true; // 왼쪽 이동 시 플립 활성화
        }
    }

    // Gem과 충돌했을 때
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Gem 태그가 달린 오브젝트와 충돌하면
        if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);  // Gem 오브젝트 삭제
            gemCount++;  // Gem 카운트 증가
            UpdateGemCountText();  // UI에 카운트 업데이트
        }
    }

    // UI 텍스트에 Gem 카운트 업데이트
    void UpdateGemCountText()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems: " + gemCount.ToString();  // UI 텍스트에 카운트 표시
        }
    }
}
