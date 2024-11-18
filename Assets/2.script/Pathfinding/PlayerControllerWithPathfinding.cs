using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWithPathfinding : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�
    public Transform target;    // ��ǥ ���� Transform
    public LayerMask obstacleLayer; // ��ֹ� LayerMask
    public Animator animator;   // �ִϸ����� ������Ʈ

    private Pathfinding pathfinding;
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int currentGridPos;

    // �ʱ� ��ġ ���� ����
    private Vector3 initialPosition;

    // �̵� �� ���θ� Ȯ���ϴ� �÷���
    private bool isMoving = false;

    // �� ī��Ʈ ����
    private int gemCount = 0;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>(); // Pathfinding ��ũ��Ʈ ����
        currentGridPos = Vector2Int.FloorToInt(transform.position); // ���� ��ġ

        // �ʱ� ��ġ ����
        initialPosition = transform.position;
    }

    void Update()
    {
        // �̵� ���� ��� Ŭ���� ����
        if (isMoving)
            return;

        // ���콺 Ŭ������ ��ǥ ����
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int goalGridPos = Vector2Int.FloorToInt(mousePosition);

            // ��ֹ� ���� �� ��� ã��
            HashSet<Vector2Int> obstacles = GetObstacles();
            path = pathfinding.FindPath(currentGridPos, goalGridPos, obstacles);

            if (path.Count > 0) // ��ȿ�� ��ΰ� ���� ��츸 �̵� ����
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator FollowPath()
    {
        isMoving = true; // �̵� ����

        foreach (var position in path)
        {
            Vector3 targetPos = new Vector3(position.x, position.y, 0);
            Vector2 direction = (targetPos - transform.position).normalized; // �̵� ���� ���

            while ((targetPos - transform.position).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                // �ִϸ��̼� ���� ������Ʈ
                UpdateAnimationDirection(direction);

                yield return null;
            }

            currentGridPos = position; // ���� ��ġ ������Ʈ
        }

        // ���� ���� �ִϸ��̼� ó��
        UpdateAnimationDirection(Vector2.zero);

        isMoving = false; // �̵� ����
    }

    void UpdateAnimationDirection(Vector2 direction)
    {
        // �̵� ���⿡ ���� �ִϸ��̼� ����
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
            GetComponent<SpriteRenderer>().flipX = false; // ������ �̵� �� �ø� ��Ȱ��ȭ
        }
        else if (direction.x < 0)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true; // ���� �̵� �� �ø� Ȱ��ȭ
        }
        else
        {
            animator.Play("idle"); // ���� ����
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
        // Enemy �±װ� ���� ������Ʈ�� �浹�� ���
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy! Returning to initial position.");
            ReturnToInitialPosition();
        }

        // Gem �±װ� ���� ������Ʈ�� �浹�� ���
        if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject); // �� ������Ʈ ����
            gemCount++; // �� ���� ����
        }
    }

    void ReturnToInitialPosition()
    {
        StopAllCoroutines(); // ���� ���� ���� �̵� �ߴ�
        transform.position = initialPosition; // �ʱ� ��ġ�� �̵�
        currentGridPos = Vector2Int.FloorToInt(initialPosition); // ���� ��ġ�� �ʱ� ��ġ�� ������Ʈ
        isMoving = false; // �̵� ���� �ʱ�ȭ
    }

    // �� ������ ��ȯ�ϴ� �޼���
    public int GetGemCount()
    {
        return gemCount;
    }
}
