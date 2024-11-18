using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI Text�� ����ϱ� ���� �ʿ�

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�
    public Animator animator;   // �ִϸ����� ������Ʈ
    private Rigidbody2D rb;     // Rigidbody2D ������Ʈ
    private Vector2 movement;   // �÷��̾��� �̵� ����

    // Gem ī��Ʈ ����
    private int gemCount = 0;

    // UI �ؽ�Ʈ ������Ʈ
    public Text gemCountText;  // �� �ʵ忡 Text UI�� �����Ͽ� ī��Ʈ�� ȭ�鿡 ǥ��

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        UpdateGemCountText();  // ���� �� ī��Ʈ �ؽ�Ʈ ������Ʈ
    }

    void Update()
    {
        // ����Ű �Է� ó��
        movement = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") != 0) // ���� �Ǵ� ������ �Է�
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = 0; // �밢�� ����: ���� �Է��� ���� ��� ���� �Է� ����
        }
        else if (Input.GetAxisRaw("Vertical") != 0) // �� �Ǵ� �Ʒ� �Է�
        {
            movement.y = Input.GetAxisRaw("Vertical");
            movement.x = 0; // �밢�� ����: ���� �Է��� ���� ��� ���� �Է� ����
        }

        // �ִϸ��̼� ������Ʈ
        if (movement != Vector2.zero)
        {
            UpdateAnimationDirection(movement);
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D�� ����� �̵� ó��
        rb.velocity = movement * moveSpeed;
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
        else if (direction == Vector2.right)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = false; // ������ �̵� �� �ø� ��Ȱ��ȭ
        }
        else if (direction == Vector2.left)
        {
            animator.Play("walk");
            GetComponent<SpriteRenderer>().flipX = true; // ���� �̵� �� �ø� Ȱ��ȭ
        }
    }

    // Gem�� �浹���� ��
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Gem �±װ� �޸� ������Ʈ�� �浹�ϸ�
        if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);  // Gem ������Ʈ ����
            gemCount++;  // Gem ī��Ʈ ����
            UpdateGemCountText();  // UI�� ī��Ʈ ������Ʈ
        }
    }

    // UI �ؽ�Ʈ�� Gem ī��Ʈ ������Ʈ
    void UpdateGemCountText()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems: " + gemCount.ToString();  // UI �ؽ�Ʈ�� ī��Ʈ ǥ��
        }
    }
}
