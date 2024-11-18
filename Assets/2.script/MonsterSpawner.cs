using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // 몬스터 프리팹
    public GameObject monsterPrefab;

    // 몬스터 소환 주기 (초 단위)
    public float spawnInterval = 5f;

    // 몬스터 소환 위치
    public Transform spawnPoint;

    void Start()
    {
        // 주기적으로 몬스터 소환 시작
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while (true)
        {
            // 몬스터 생성
            Spawn();

            // 소환 주기만큼 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Spawn()
    {
        if (monsterPrefab != null && spawnPoint != null)
        {
            // 몬스터 생성 (소환 위치와 회전 정보 사용)
            Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("MonsterPrefab or SpawnPoint is not assigned!");
        }
    }
}
