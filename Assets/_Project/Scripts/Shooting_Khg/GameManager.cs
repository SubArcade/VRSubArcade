using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 코루틴 사용을 위해 추가

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("점수 설정")]
    [Tooltip("점수를 표시할 UI 텍스트")]
    public Text scoreText;
    public Text scoreText2;
    public int defaultScoreAmount = 10;

    [Header("타겟 관리")]
    [Tooltip("씬에 있는 모든 타겟들 (자동으로 찾아 할당)")]
    public HittableTarget[] allTargets;

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetScore();
        // 게임 시작 후 1프레임 뒤에 타겟을 찾도록 코루틴 실행
        StartCoroutine(SetupTargets());
    }

    /// <summary>
    /// 씬에 있는 모든 HittableTarget을 찾아 배열에 할당합니다.
    /// </summary>
    private IEnumerator SetupTargets()
    {
        // 1프레임 대기하여 씬의 모든 오브젝트가 확실히 로드된 후 실행
        yield return null; 
        allTargets = FindObjectsOfType<HittableTarget>();
    }

    /// <summary>
    /// 모든 타겟을 초기 상태로 리셋합니다.
    /// </summary>
    public void ResetAllTargets()
    {
        if (allTargets == null || allTargets.Length == 0)
        {
            Debug.LogWarning("리셋할 타겟이 없습니다. 타겟이 제대로 찾아졌는지 확인하세요.");
            return;
        }

        foreach (var target in allTargets)
        {
            target.ResetTarget();
            ResetScore();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
            scoreText2.text = "Score: " + currentScore;
        }
        else
        {
            Debug.LogWarning("Score Text가 GameManager에 연결되지 않았습니다.");
        }
    }
}