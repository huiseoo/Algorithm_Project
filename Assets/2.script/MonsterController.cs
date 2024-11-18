using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // 몬스터의 이동 속도
    public float moveSpeed;

    // 몬스터의 이동 방향 (1: 위로, -1: 아래로)
    public int moveDirection = 1;

    // 리스폰 대기 시간 (초 단위)
    public float respawnTime;

    // 애니메이터 컴포넌트
    public Animator animator;

    // 초기 위치를 저장
    private Vector2 initialPosition;

    // Rigidbody2D 컴포넌트
    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();

        // 초기 위치 저장
        initialPosition = transform.position;

        // 몬스터 이동 시작
        StartCoroutine(Move());

        // 일정 시간마다 몬스터를 리스폰
        //StartCoroutine(RespawnTimer());
    }

    IEnumerator Move()
    {
        while (true)
        {
            // 이동 방향에 따라 속도를 설정
            rb.velocity = new Vector2(0, moveDirection * moveSpeed);

            // 이동 방향에 따른 애니메이션 업데이트
            UpdateAnimationDirection();

            yield return null; // 매 프레임마다 실행
        }
    }

    void UpdateAnimationDirection()
    {
        // 이동 방향에 따라 애니메이션 재생
        if (moveDirection == 1)
        {
            animator.Play("up"); // 위로 이동 시 'up' 애니메이션
        }
        else if (moveDirection == -1)
        {
            animator.Play("down"); // 아래로 이동 시 'down' 애니메이션
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터가 Wall 태그에 부딪히면 비활성화
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Monster hit the wall and will disappear.");
            gameObject.SetActive(false); // 몬스터 비활성화
        }
    }

    IEnumerator RespawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime); // 리스폰 대기 시간
            Respawn(); // 리스폰 처리
        }
    }

    void Respawn()
    {
        // 초기 위치로 리스폰
        Debug.Log("Monster respawned.");
        transform.position = initialPosition; // 초기 위치로 이동
        gameObject.SetActive(true); // 몬스터 활성화
    }
}

