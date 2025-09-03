using UnityEngine;
using UnityEngine.UI;


public class BagHealth : MonoBehaviour
{
    public float maxHealth = 20f;
    public GameObject BoomParticle;

    public float moveSpeed = 2f;           
    public float moveRange = 8f;
    public float moveInterval = 1f;

    [Header("UI Text 연결")]
    public Text healthText;

    [Header("폭발 사운드 클립")]
    public AudioClip boomSound;

    private float currentHealth;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float timer;



    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        targetPosition = startPosition;
        UpdateHealthUI();
    }
    void FixedUpdate()
    { //일정시간마다 움직임
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            PickNewTarget();
        }

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    void PickNewTarget()
    { //랜덤으로 상하, 좌우 이동거리를 정함
        float offsetX = Random.Range(-moveRange, moveRange);
        float offsetY = Random.Range(-moveRange, moveRange);
        targetPosition = startPosition + new Vector3(offsetX, offsetY, 0f);
        //startPosition은 오브젝트가 처음 시작한 위치
        //offsetX, offsetY를 더해서 새로운 목표 위치를 계산

    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"맞춰! {currentHealth}";
        }
    }

    void Die()
    {
        if (BoomParticle != null)
        {
            GameObject spawnedBoom = Instantiate(BoomParticle, transform.position, Quaternion.identity);
            Destroy(spawnedBoom, 0.3f);
        }
        // 폭발 사운드 재생
        if (boomSound != null)
        {
            AudioSource.PlayClipAtPoint(boomSound, transform.position);
        }

        gameObject.SetActive(false); // 비활성화
        Invoke(nameof(Respawn), 3f);  //  다시 활성화
    }
    void Respawn()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        transform.position = startPosition;
        targetPosition = startPosition;
        timer = 0f;
        gameObject.SetActive(true);
    }

}

