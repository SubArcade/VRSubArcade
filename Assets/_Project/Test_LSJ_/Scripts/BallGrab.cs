using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BallGrab : MonoBehaviour
{
    public XRBaseInteractable interactable;
    public float grabOffset = 0.5f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool shouldMove = false;
    void Start()
    {
        if (interactable == null)
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        if (interactable != null)
        {
            // selectEntered 이벤트에 OnGrabbed 함수를 연결합니다.
            interactable.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // 1. OnGrabbed는 Update()와 같은 프레임에서 호출됩니다.
        //    이동할 목표 위치만 계산하여 저장합니다.
        Transform interactor = args.interactorObject.transform;
        Vector3 forwardDirection = interactor.forward;
        targetPosition = interactor.position + forwardDirection * grabOffset;
        shouldMove = true;
        
        Debug.Log("계산된 목표 위치: " + targetPosition);
    }
    
    void FixedUpdate()
    {
        if (shouldMove)
        {
            // 2. FixedUpdate()에서 Rigidbody를 사용하여 위치를 업데이트합니다.
            //    이렇게 해야 물리 엔진과 동기화됩니다.
            if (rb != null)
            {
                rb.MovePosition(targetPosition);
                // rb.position = targetPosition; 도 사용 가능합니다.
            }

            // 한 번 이동했으니 더 이상 움직이지 않도록 플래그를 비활성화합니다.
            shouldMove = false;
        }
    }

    void OnDestroy()
    {
        // 메모리 누수를 방지하기 위해 이벤트를 해제합니다.
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}
