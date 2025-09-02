using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballPlayer : MonoBehaviour
{
    public float mouseSensitivity = 100f; // 마우스 감도 조절
    public float verticalLookLimit = 90f; // 상하 시야각 제한 (90도 이상으로 설정하면 어색해질 수 있음)
    private float mouseX;
    private float rotationX = 0f; // X축(상하) 누적 회전 값
    private float rotationY = 0f; // Y축(좌우) 누적 회전 값
    
    public GameObject ball;
    public Transform ballSpawnPos;
    public float launchForce = 10f;
    public float launchAngle = 45f;
    
    public BasketballManager manager;
    void Update()
    {
        // 마우스의 X, Y축 이동 값을 가져옵니다.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Y축(좌우) 회전 값을 누적하여 계산합니다.
        rotationY += mouseX;

        // X축(상하) 회전 값을 누적하여 계산합니다.
        rotationX -= mouseY;
        
        // 상하 시야각을 제한하여 360도 회전하는 것을 방지합니다.
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);

        // 계산된 회전 값을 카메라에 적용합니다.
        // Y축 회전은 transform.localRotation을 사용해 현재 회전값에 더하고,
        // X축 회전은 새로운 Quaternion을 만들어 적용합니다.
        //transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // ShootTest();   
        }
        
        
    }
    private void ShootTest()
    {
        GameObject spawnedBall = Instantiate(ball, ballSpawnPos.position, ballSpawnPos.rotation);
        spawnedBall.GetComponent<BallScript>().SetObject(manager); // 농구 게임 매니저 넣어줌
        Rigidbody rb = spawnedBall.GetComponent<Rigidbody>();
      
        Vector3 forwardDirection = transform.forward;
    
        Vector3 upwardDirection = transform.up;

        float angleInRadians = launchAngle * Mathf.Deg2Rad;
        Vector3 launchDirection = (forwardDirection + upwardDirection * Mathf.Tan(angleInRadians)).normalized;

        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        //Destroy(spawnedBall, 4f);
      
    }
}
