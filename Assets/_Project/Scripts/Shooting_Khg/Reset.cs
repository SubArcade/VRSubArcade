using UnityEngine;

public class Reset : MonoBehaviour
{
    private TargetController[] targets;

    void Start()
    {
        // 씬 안의 모든 TargetController 자동 검색
        targets = FindObjectsOfType<TargetController>();
    }

    public void ResetAllTargets()
    {
        foreach (var target in targets)
        {
            target.ResetTarget();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        ResetAllTargets();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    ResetAllTargets();
    //}


}
