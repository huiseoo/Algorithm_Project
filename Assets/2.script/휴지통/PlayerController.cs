using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    public Transform target;  // Ÿ�� ��ġ Transform
    private bool isMoving = false;

    // x��� y�����θ� �̵� (�밢�� �̵� ����)
    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���콺 ��Ŭ������ Ÿ�� ����
        if (Input.GetMouseButton(1)) // ��Ŭ�� ���¸� ����
        {
            Debug.Log("Right-click detected!"); // ��Ŭ�� ���� �޽���
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetTarget(targetPosition);
        }
    }

    void SetTarget(Vector2 position)
    {
        // Ÿ�� ������Ʈ ��ġ�� �����ϰ� �̵� ����
        target.position = position;
        isMoving = true;

        Debug.Log($"Target set at: {position}"); // Ÿ�� ��ġ ���� Ȯ�� �޽���

        // ��� Ž�� �� �̵� ����
        List<Vector2> path = FindPath(rb.position, target.position);
        if (path.Count > 0)
        {
            StopAllCoroutines();  // ���� �̵� ��� ����
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
            // �� ��������Ʈ�� �̵�
            while (Vector2.Distance(rb.position, waypoint) > 0.1f)
            {
                // ��ǥ ���������� ������ ���
                Vector2 direction = (waypoint - rb.position).normalized;

                // rb.velocity�� ����Ͽ� ����� �ӵ��� ����
                rb.velocity = direction * moveSpeed;

                // �̵� ���⿡ ���� �ִϸ��̼� ������Ʈ
                UpdateAnimationDirection(direction);

                yield return new WaitForFixedUpdate();
            }

            // ���� ��������Ʈ�� �����ϸ� �ӵ��� 0���� �����Ͽ� ����
            rb.velocity = Vector2.zero;
        }

        // ��� ��������Ʈ�� ������ �� �̵� ���� ����
        isMoving = false;
    }


    List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        // A* �˰����� ����Ͽ� ��ǥ ���������� �ִ� ��� ���
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
        // ����ư �Ÿ� ��� (�밢�� �̵��� �����ϱ� ���� ����)
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    bool IsWall(Vector2 position)
    {
        // �� ����: �ش� ��ġ�� "Wall" �±׸� ���� Collider�� ������ ������ �ν�
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        return hitCollider != null && hitCollider.CompareTag("Wall");
    }

    void UpdateAnimationDirection(Vector2 direction)
    {
        // �̵� ���⿡ ���� �ִϸ��̼� ����
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

    // Priority Queue Ŭ���� ����
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

