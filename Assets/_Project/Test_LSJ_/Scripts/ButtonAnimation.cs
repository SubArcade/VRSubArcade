using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonAnimation : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private BasketballManager manager;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        manager = GameObject.FindGameObjectWithTag("BasketballManager").GetComponent<BasketballManager>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnButtonPressed);
        }
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        manager.InitializeGame();
        transform.DOLocalMoveY(0.98f, 0.3f);
        DOVirtual.DelayedCall(2f, () => transform.DOLocalMoveY(1.02f, 0.3f));
        
    }
}
