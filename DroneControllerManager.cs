using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class DroneControllerManager : MonoBehaviour
{
    private InputDevice droneController;

    private void Start()
    {
        StartCoroutine(InitializeController());
    }

    private IEnumerator InitializeController()
    {
        // 컨트롤러 시스템에 추가될 때까지 대기
        while ((droneController = InputSystem.GetDevice("PengFei Model RC Simulator - XTR+G2+FMS Controller")) == null)
        {
            yield return null;
        }

        // 드론 컨트롤러 초기화
        droneController = InputSystem.GetDevice("PengFei Model RC Simulator - XTR+G2+FMS Controller");
        Debug.Log("Drone controller initialized.");
    }

    private void Update()
    {
        if (droneController != null)
        {
            DroneControllerSupport.ReadLeftRightStickHorizontal(droneController);
        }
    }
}