using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpawn : MonoBehaviour
{
    [Header("생성할 프리팹들")]
    public GameObject[] throwablePrefabs;

    [Header("생성 위치 기준 상자")]
    public Transform spawnBox;

    [Header("최대 유지 개수")]
    public int spawnCount = 20;

    [Header("상자 크기")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);

    [Header("생성 간격 (초)")]
    public float checkInterval = 1f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        StartCoroutine(KeepSpawning());
    }

    IEnumerator KeepSpawning()
    {
        while (true)
        {
            // 리스트에서 파괴된 오브젝트 제거
            spawnedObjects.RemoveAll(obj => obj == null);

            int toSpawn = spawnCount - spawnedObjects.Count;//부족한 갯수 계산

            for (int i = 0; i < toSpawn; i++) //부족한 수만큼 반복생성
            {
                GameObject prefab = GetRandomNonBagPrefab(); //bag태그가 아닌 프리팹 중 하나를 랜덤선택
                if (prefab == null)
                {                  
                    break; //선택가능이 없으면 종료
                }

                Vector3 randomPos = spawnBox.position + new Vector3(
                    Random.Range(-boxSize.x / 2, boxSize.x / 2),
                    Random.Range(-boxSize.y / 2, boxSize.y / 2),
                    Random.Range(-boxSize.z / 2, boxSize.z / 2)
                    //랜덤위치 계산
                );

                GameObject newObj = Instantiate(prefab, randomPos, Quaternion.identity);
                spawnedObjects.Add(newObj); //프리팹을 생성하고 그 생성된 프리팹을 리스트에 추가
            }

            yield return new WaitForSeconds(checkInterval); //일정시간 대기후 루프반복
        }
    }

    GameObject GetRandomNonBagPrefab()
    {
        List<GameObject> filtered = new List<GameObject>(); //bag태그 프리팹 리스트

        foreach (GameObject prefab in throwablePrefabs)
        {
            if (prefab.tag != "bag")
            {
                filtered.Add(prefab);
            } //bag태그 아닌것만 필터링
        }

        if (filtered.Count == 0) return null;

        return filtered[Random.Range(0, filtered.Count)]; //필터링된 프리팹 랜덤으로 반환
    }
}
