using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWithPathfinding : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도
    public Transform target;    // 목표 지점 Transform
    public LayerMask obstacleLayer; // 장애물 LayerMask
    public Animator animator;   // 애니메이터 컴포넌트

    private Pathfinding pathfinding;
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int currentGridPos;

    // 초기 위치 저장 변수
    private Vector3 initialPosition;

    // 이동 중 여부를 확인하는 플래그
    private bool isMoving = false;

    // 잼 카운트 변수
    private int gemCount = 0;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>(); // Pathfinding 스크립트 참조
        currentGridPos = Vector2Int.FloorToInt(transform.position); // 시작 위치

        // 초기 위치 저장
        initialPosition = transform.position;
    }

    void Update()
    {
        // 이동 중일 경우 클릭을 무시
        if (isMoving)
            return;

        // 마우스 클릭으로 목표 설정
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int goalGridPos = Vector2Int.FloorToInt(mousePosition);

            // 장애물 검출 및 경로 찾기
            HashSet<Vector2Int> obstacles = GetObstacles();
            path = pathfinding.FindPath(currentGridPos, goalGridPos, obstacles);

            if (path.Count > 0) // 유효한 경로가 있을 경우만 이동 시작
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator FollowPath()
    {
        isMoving = true; // 이동 시작

        foreach (var position in path)
        {
            Vector3 targetPos = new Vector3(position.x, position.y, 0);
            Vector2 direction = (targetPos - transform.position).normalized; // 이동 방향 계산

            while ((targetPos - transform.position).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                // 애니메이션 방향 업데이트
                UpdateAnimationDirection(direction);

                yield return null;
            }

            currentGridPos = position; // 현재 위치 업데이트
        }

        // 정지 상태 애니메이션 처리
        UpdateAnimationDirection(Vector2.zero);

        isMoving = false; // 이동 종료
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
        else if (direction.x > 0)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽 이동 시 플립 비활성화
        }
        else if (direction.x < 0)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true; // 왼쪽 이동 시 플립 활성화
        }
        else
        {
            animator.Play("idle"); // 정지 상태
        }
    }

    HashSet<Vector2Int> GetObstacles()
    {
        HashSet<Vector2Int> obstacles = new HashSet<Vector2Int>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(50, 50), 0, obstacleLayer);
        foreach (var col in colliders)
        {
            obstacles.Add(Vector2Int.FloorToInt(col.transform.position));
        }
        return obstacles;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy 태그가 붙은 오브젝트와 충돌한 경우
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy! Returning to initial position.");
            ReturnToInitialPosition();
        }

        // Gem 태그가 붙은 오브젝트와 충돌한 경우
        if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject); // 잼 오브젝트 제거
            gemCount++; // 잼 개수 증가
        }
    }

    void ReturnToInitialPosition()
    {
        StopAllCoroutines(); // 현재 진행 중인 이동 중단
        transform.position = initialPosition; // 초기 위치로 이동
        currentGridPos = Vector2Int.FloorToInt(initialPosition); // 현재 위치를 초기 위치로 업데이트
        isMoving = false; // 이동 상태 초기화
    }

    // 잼 개수를 반환하는 메서드
    public int GetGemCount()
    {
        return gemCount;
    }
}
