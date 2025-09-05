using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BallGrab : MonoBehaviour
{
    public XRBaseInteractable interactable;

    private BallScript ballScript;
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
        ballScript = GetComponent<BallScript>();
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        ballScript.InitiateBall();
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
