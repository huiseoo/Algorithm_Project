using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // ������ �̵� �ӵ�
    public float moveSpeed;

    // ������ �̵� ���� (1: ����, -1: �Ʒ���)
    public int moveDirection = 1;

    // ������ ��� �ð� (�� ����)
    public float respawnTime;

    // �ִϸ����� ������Ʈ
    public Animator animator;

    // �ʱ� ��ġ�� ����
    private Vector2 initialPosition;

    // Rigidbody2D ������Ʈ
    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody2D ������Ʈ ��������
        rb = GetComponent<Rigidbody2D>();

        // �ʱ� ��ġ ����
        initialPosition = transform.position;

        // ���� �̵� ����
        StartCoroutine(Move());

        // ���� �ð����� ���͸� ������
        //StartCoroutine(RespawnTimer());
    }

    IEnumerator Move()
    {
        while (true)
        {
            // �̵� ���⿡ ���� �ӵ��� ����
            rb.velocity = new Vector2(0, moveDirection * moveSpeed);

            // �̵� ���⿡ ���� �ִϸ��̼� ������Ʈ
            UpdateAnimationDirection();

            yield return null; // �� �����Ӹ��� ����
        }
    }

    void UpdateAnimationDirection()
    {
        // �̵� ���⿡ ���� �ִϸ��̼� ���
        if (moveDirection == 1)
        {
            animator.Play("up"); // ���� �̵� �� 'up' �ִϸ��̼�
        }
        else if (moveDirection == -1)
        {
            animator.Play("down"); // �Ʒ��� �̵� �� 'down' �ִϸ��̼�
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���Ͱ� Wall �±׿� �ε����� ��Ȱ��ȭ
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Monster hit the wall and will disappear.");
            gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        }
    }

    IEnumerator RespawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime); // ������ ��� �ð�
            Respawn(); // ������ ó��
        }
    }

    void Respawn()
    {
        // �ʱ� ��ġ�� ������
        Debug.Log("Monster respawned.");
        transform.position = initialPosition; // �ʱ� ��ġ�� �̵�
        gameObject.SetActive(true); // ���� Ȱ��ȭ
    }
}

