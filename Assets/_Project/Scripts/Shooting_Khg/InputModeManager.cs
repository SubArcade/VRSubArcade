using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 여러 VR 컨트롤러/조작법 세트를 관리하고 전환합니다.
/// 각 조작법은 컨트롤러, 인터랙터 등을 포함하는 부모 게임 오브젝트여야 합니다.
/// </summary>
public class InputModeManager : MonoBehaviour
{
    /// <summary>
    /// 사용하려는 조작법 모드를 정의합니다. (예: Teleport, GrabOnly, Drive)
    /// </summary>
    public enum ControlMode
    {
        Control_01,
        Control_02,
        Control_03,
        Control_04,
        Control_05
        // 필요에 따라 이름을 변경하거나 추가하세요.
    }
     
    [Tooltip("컨트롤러 세트 게임 오브젝트 목록. ControlMode Enum 순서 일치해야 함.")]
    public List<GameObject> controlSetups;

    [Tooltip("장면이 시작될 때 활성화할 기본 컨트롤러.")]
    public ControlMode startingMode;

    void Start()
    {
        // 시작 모드로 설정
        SwitchMode(startingMode);
    }

    /// <summary>
    /// 활성화할 조작법을 전환합니다.
    /// </summary>
    /// <param name="mode">활성화할 모드</param>
    public void SwitchMode(ControlMode mode)
    {
        int modeIndex = (int)mode;

        if (modeIndex >= controlSetups.Count || controlSetups[modeIndex] == null)
        {
            Debug.LogWarning($"InputModeManager: '{mode}'에 할당된 컨트롤러 세트가 없거나 유효하지 않습니다.");
            return;
        }

        // 모든 컨트롤러 세트를 비활성화
        for (int i = 0; i < controlSetups.Count; i++)
        {
            if (controlSetups[i] != null)
            {
                SafeDisableGroupsUnder(controlSetups[i]);
                controlSetups[i].SetActive(false);
            }
        }

        // 선택된 컨트롤러 세트만 활성화
        controlSetups[modeIndex].SetActive(true);
        RebuildGroupsUnder(controlSetups[modeIndex]);
        Debug.Log($"조작법 모드가 '{mode}'(으)로 변경되었습니다.");
    }

    // --- 테스트용 코드 ---
    // 에디터에서 숫자 키 1, 2, 3을 눌러 모드를 전환.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchMode(ControlMode.Control_01);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchMode(ControlMode.Control_02);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchMode(ControlMode.Control_03);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchMode(ControlMode.Control_04);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchMode(ControlMode.Control_05);
        }
    }
    
    // XRInteractionGroup 비우고 비활성화
    void SafeDisableGroupsUnder(GameObject rigRoot)
    {
        if (!rigRoot) return;
        var groups = rigRoot.GetComponentsInChildren<XRInteractionGroup>(true);

        foreach (var g in groups)
        {
            g.ClearGroupMembers();
            g.enabled = false;
            g.ClearGroupMembers();
        }
    }
    
    // XRInteractionGroup 재구성
    void RebuildGroupsUnder(GameObject rigRoot)
    {
        if (!rigRoot) return;
        
        var groups = rigRoot.GetComponentsInChildren<XRInteractionGroup>(true);
        if (groups.Length == 0) return; // 그룹이 하나도 없으면 아무 것도 안 함
        
        var interactors = rigRoot.GetComponentsInChildren<XRBaseInteractor>(true);

        foreach (var g in groups)
        {
            g.ClearGroupMembers();
            g.enabled = true;
            
            foreach (var it in interactors)
            {
                if (it && it.enabled && it.gameObject.activeInHierarchy)
                    g.AddGroupMember(it);
            }
        }
    }

}
