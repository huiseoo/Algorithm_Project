using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // ���� ������
    public GameObject monsterPrefab;

    // ���� ��ȯ �ֱ� (�� ����)
    public float spawnInterval = 5f;

    // ���� ��ȯ ��ġ
    public Transform spawnPoint;

    void Start()
    {
        // �ֱ������� ���� ��ȯ ����
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            // ���� ����
            Spawn();

            // ��ȯ �ֱ⸸ŭ ���
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Spawn()
    {
        if (monsterPrefab != null && spawnPoint != null)
        {
            // ���� ���� (��ȯ ��ġ�� ȸ�� ���� ���)
            Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("MonsterPrefab or SpawnPoint is not assigned!");
        }
    }
}
