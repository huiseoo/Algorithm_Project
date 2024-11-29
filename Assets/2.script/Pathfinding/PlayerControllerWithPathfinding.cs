using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* 경로찾기를 이용한 플레이어 캐릭터 컨트롤러
/// 마우스 클릭으로 이동하며 장애물을 피하고 보석을 수집하는 기능 구현
/// </summary>
public class PlayerControllerWithPathfinding : MonoBehaviour
{
    // 인스펙터에서 설정 가능한 public 변수들
    public float moveSpeed = 5f;         // 플레이어의 이동 속도
    public Transform target;             // 목표 지점의 Transform
    public LayerMask obstacleLayer;      // 장애물 감지를 위한 레이어 마스크
    public Animator animator;            // 캐릭터 애니메이션 제어용 컴포넌트

    // 경로찾기 관련 private 변수들
    private Pathfinding pathfinding;     // A* 경로찾기 알고리즘 컴포넌트
    private List<Vector2Int> path;       // 현재 이동 경로를 저장하는 리스트
    private Vector2Int currentGridPos;   // 현재 그리드 상의 위치

    private Vector3 initialPosition;      // 시작 위치 저장 (부활 지점)
    private bool isMoving = false;       // 현재 이동 중인지 나타내는 플래그
    private int gemCount = 0;            // 수집한 보석의 개수

    /// <summary>
    /// 컴포넌트 초기화 및 시작 위치 설정
    /// </summary>
    void Start()
    {
        // 필요한 컴포넌트 참조 가져오기
        pathfinding = GetComponent<Pathfinding>();
        
        // 현재 위치를 그리드 좌표로 변환하여 저장
        currentGridPos = Vector2Int.FloorToInt(transform.position);
        
        // 초기 위치 저장 (부활 지점으로 사용)
        initialPosition = transform.position;
    }

    /// <summary>
    /// 매 프레임 마우스 입력을 처리하고 이동 경로 계산
    /// </summary>
    void Update()
    {
        // 이동 중일 경우 추가 입력을 무시
        if (isMoving)
            return;

        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치를 월드 좌표로 변환
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int goalGridPos = Vector2Int.FloorToInt(mousePosition);

            // 장애물을 감지하고 경로 계산
            HashSet<Vector2Int> obstacles = GetObstacles();
            path = pathfinding.FindPath(currentGridPos, goalGridPos, obstacles);

            // 유효한 경로가 있을 경우에만 이동 시작
            if (path.Count > 0)
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    /// <summary>
    /// 계산된 경로를 따라 캐릭터를 이동시키는 코루틴
    /// </summary>
    IEnumerator FollowPath()
    {
        isMoving = true;     // 이동 시작 상태로 설정

        // 경로의 각 지점을 순회하며 이동
        foreach (var position in path)
        {
            // 현재 목표 지점과 이동 방향 계산
            Vector3 targetPos = new Vector3(position.x, position.y, 0);
            Vector2 direction = (targetPos - transform.position).normalized;

            // 목표 지점에 도달할 때까지 이동
            while ((targetPos - transform.position).sqrMagnitude > 0.01f)
            {
                // 부드러운 이동 처리
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    targetPos, 
                    moveSpeed * Time.deltaTime
                );

                // 현재 이동 방향에 맞는 애니메이션 재생
                UpdateAnimationDirection(direction);

                yield return null;
            }

            // 현재 그리드 위치 업데이트
            currentGridPos = position;
        }

        // 이동 완료 후 정지 상태 애니메이션으로 전환
        UpdateAnimationDirection(Vector2.zero);
        isMoving = false;    // 이동 종료 상태로 설정
    }

    /// <summary>
    /// 이동 방향에 따라 적절한 애니메이션을 재생하고 스프라이트 방향 조정
    /// </summary>
    /// <param name="direction">이동 방향 벡터</param>
    void UpdateAnimationDirection(Vector2 direction)
    {
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
            GetComponent<SpriteRenderer>().flipX = false;  // 오른쪽 방향
        }
        else if (direction.x < 0)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true;   // 왼쪽 방향
        }
        else
        {
            animator.Play("idle");  // 정지 상태
        }
    }

    /// <summary>
    /// 주변의 장애물을 감지하여 그리드 좌표로 변환
    /// </summary>
    /// <returns>장애물의 그리드 좌표 집합</returns>
    HashSet<Vector2Int> GetObstacles()
    {
        HashSet<Vector2Int> obstacles = new HashSet<Vector2Int>();
        
        // 주변 영역의 장애물 콜라이더 감지
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(50, 50),
            0,
            obstacleLayer
        );

        // 감지된 각 장애물의 위치를 그리드 좌표로 변환하여 저장
        foreach (var col in colliders)
        {
            obstacles.Add(Vector2Int.FloorToInt(col.transform.position));
        }

        return obstacles;
    }

    /// <summary>
    /// 다른 오브젝트와의 충돌 처리 (적, 보석 등)
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 적과 충돌한 경우
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy! Returning to initial position.");
            ReturnToInitialPosition();
        }

        // 보석과 충돌한 경우
        if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);  // 보석 오브젝트 제거
            gemCount++;                     // 보석 카운트 증가
        }
    }

    /// <summary>
    /// 플레이어를 초기 위치로 되돌리는 메서드
    /// </summary>
    void ReturnToInitialPosition()
    {
        StopAllCoroutines();                                      // 진행 중인 모든 코루틴 중지
        transform.position = initialPosition;                      // 위치 초기화
        currentGridPos = Vector2Int.FloorToInt(initialPosition);  // 그리드 위치 초기화
        isMoving = false;                                         // 이동 상태 초기화
    }

    /// <summary>
    /// 현재까지 수집한 보석의 개수를 반환
    /// </summary>
    public int GetGemCount()
    {
        return gemCount;
    }
}
