using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviour
{
    public Text countdownText; // 카운트다운 숫자를 표시할 UI 텍스트
    public GameObject Drone; // 움직임을 제어할 플레이어(차량)

    private bool raceStarted = false;

    void Start()
    {
        Drone.GetComponent<SimpleDroneController>().enabled = false; // 초기에는 움직임 비활성화
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString(); // UI 텍스트에 숫자 표시
            yield return new WaitForSeconds(1f); // 1초 대기
            countdown--;
        }

        countdownText.text = "Go!"; // "Go!" 표시
        yield return new WaitForSeconds(1f); // 1초 대기
        countdownText.gameObject.SetActive(false); // UI 텍스트 숨기기

        Drone.GetComponent<SimpleDroneController>().enabled = true; // 차량 움직임 활성화
        raceStarted = true;
    }
}