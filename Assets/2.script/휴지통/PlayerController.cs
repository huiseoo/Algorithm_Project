using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    public Transform target;  // 타깃 위치 Transform
    private bool isMoving = false;

    // x축과 y축으로만 이동 (대각선 이동 금지)
    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 마우스 우클릭으로 타깃 설정
        if (Input.GetMouseButton(1)) // 우클릭 상태를 감지
        {
            Debug.Log("Right-click detected!"); // 우클릭 감지 메시지
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetTarget(targetPosition);
        }
    }

    void SetTarget(Vector2 position)
    {
        // 타깃 오브젝트 위치를 설정하고 이동 시작
        target.position = position;
        isMoving = true;

        Debug.Log($"Target set at: {position}"); // 타깃 위치 설정 확인 메시지

        // 경로 탐색 후 이동 시작
        List<Vector2> path = FindPath(rb.position, target.position);
        if (path.Count > 0)
        {
            StopAllCoroutines();  // 이전 이동 경로 중지
            StartCoroutine(FollowPath(path));
        }
        else
        {
            Debug.Log("No path found to target.");
        }
    }

    IEnumerator FollowPath(List<Vector2> path)
    {
        foreach (var waypoint in path)
        {
            // 각 웨이포인트로 이동
            while (Vector2.Distance(rb.position, waypoint) > 0.1f)
            {
                // 목표 지점까지의 방향을 계산
                Vector2 direction = (waypoint - rb.position).normalized;

                // rb.velocity를 사용하여 방향과 속도를 적용
                rb.velocity = direction * moveSpeed;

                // 이동 방향에 따른 애니메이션 업데이트
                UpdateAnimationDirection(direction);

                yield return new WaitForFixedUpdate();
            }

            // 현재 웨이포인트에 도착하면 속도를 0으로 설정하여 정지
            rb.velocity = Vector2.zero;
        }

        // 모든 웨이포인트에 도달한 후 이동 상태 종료
        isMoving = false;
    }


    List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        // A* 알고리즘을 사용하여 목표 지점까지의 최단 경로 계산
        List<Vector2> path = new List<Vector2>();
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        PriorityQueue openSet = new PriorityQueue();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>();
        Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Vector2 current = openSet.Dequeue();

            if (current == goal)
            {
                while (cameFrom.ContainsKey(current))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }

            closedSet.Add(current);

            foreach (Vector2 dir in directions)
            {
                Vector2 neighbor = current + dir;

                if (IsWall(neighbor) || closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }

        return path;
    }

    float Heuristic(Vector2 a, Vector2 b)
    {
        // 맨해튼 거리 사용 (대각선 이동을 금지하기 위해 적합)
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    bool IsWall(Vector2 position)
    {
        // 벽 감지: 해당 위치에 "Wall" 태그를 가진 Collider가 있으면 벽으로 인식
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        return hitCollider != null && hitCollider.CompareTag("Wall");
    }

    void UpdateAnimationDirection(Vector2 direction)
    {
        // 이동 방향에 따른 애니메이션 설정
        if (direction == Vector2.up)
            animator.Play("up");
        else if (direction == Vector2.down)
            animator.Play("down");
        else if (direction == Vector2.right)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (direction == Vector2.left)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Priority Queue 클래스 통합
    private class PriorityQueue
    {
        private List<KeyValuePair<Vector2, float>> elements = new List<KeyValuePair<Vector2, float>>();

        public int Count => elements.Count;

        public void Enqueue(Vector2 item, float priority)
        {
            elements.Add(new KeyValuePair<Vector2, float>(item, priority));
        }

        public Vector2 Dequeue()
        {
            int bestIndex = 0;
            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].Value < elements[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }
            Vector2 bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        public bool Contains(Vector2 item)
        {
            foreach (var element in elements)
            {
                if (element.Key == item)
                    return true;
            }
            return false;
        }
    }
}

