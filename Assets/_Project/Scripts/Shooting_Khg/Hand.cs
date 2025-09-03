using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

using ABC = UnityEngine.XR.Interaction.Toolkit.ActionBasedController;

public class Hand : MonoBehaviour
{
    public ABC abc; //햅틱 진종을 주기 위해 컨트롤로 컴포넌트 참조
    public Gun gun; //총에 트리거 효과를 주기 위해 참조

    public InputActionProperty triggerButton;


    private void Awake()
    {
        if(!abc) abc = GetComponent<ABC>();
        if(!gun) gun = GetComponent<Gun>();

        triggerButton.EnableDirectAction(); //확장메서드 
    }


    void OnEnable()
    {
        triggerButton.action.performed += OnTriggerInput;
    }

    void OnDisable()
    {
        triggerButton.action.performed -= OnTriggerInput;
    }

    void OnDestroy()
    {
        triggerButton.DisableDirectAction();    
    }
    
    void OnTriggerInput(InputAction.CallbackContext context)
    {
        isFire = context.ReadValueAsButton(); //버튼이 눌렸으면 true , 떨어졌으면 false를 반환해줌
        
        gun.PullTrigger( isFire );
    }

    public bool isFire; //트리거가 눌렸을때 true
    public float hapticInterval; //햅틱 피드백 간격
    [Range( 0, 1 )] public float hapticIntensity; //진동 강도
    public float hapticDuration; //진동 지속시간

    private float timeCache;
    private void Update()
    {
        if( !isFire ) return;
        if (Time.time < timeCache + hapticInterval) return;

        timeCache = Time.time;
        SendHaptic();
    }

    public void SendHaptic() //컨트롤러에 진동 보내는 함수
    {
        abc.SendHapticImpulse( hapticIntensity , hapticDuration );
    }
}
