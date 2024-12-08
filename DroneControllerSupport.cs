using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public static class DroneControllerSupport
{
    public static float leftHorizontalAxis = 0f;
    public static float rightHorizontalAxis = 0f;
    public static float leftVerticalAxis = 0f;
    public static float rightVerticalAxis = 0f;

    public static void ReadLeftRightStickHorizontal(InputDevice device)
    {
        DroneControllerState state;
        device.CopyState(out state);

        // 각 스틱의 rawValue를 받아 정규화
        float normalizedLeftHorizontalValue = NormalizeLeftStickHorizontal(state.leftStickHorizontal);
        float normalizedRightHorizontalValue = NormalizeRightStickHorizontal(state.rightStickHorizontal);

        float normalizedLeftVerticalValue = NormalizeLeftStickVertical(state.leftStickVertical);
        float normalizedRightVerticalValue = NormalizeRightStickVertical(state.rightStickVertical);

        // 정규화한 값을 공개 변수에 할당
        leftHorizontalAxis = normalizedLeftHorizontalValue;
        rightHorizontalAxis = normalizedRightHorizontalValue;
        leftVerticalAxis = normalizedLeftVerticalValue;
        rightVerticalAxis = normalizedRightVerticalValue;
    }

    // 왼쪽 스틱의 좌우 축 정규화 메서드
    // rawValue (0~255) 중 특정 기준값(midValue)으로부터 얼마나 벗어났는지 -1~1 범위로 변환
    private static float NormalizeLeftStickHorizontal(byte rawValue)
    {
        float leftMax = 4;    // 왼쪽으로 최대로 치우쳤을 때 예상 raw 값
        float midValue = 95;  // 중립 상태일 때의 raw 값
        float rightMax = 220; // 오른쪽으로 최대로 치우쳤을 때 예상 raw 값
        float normalizedValue;

        if (rawValue == midValue) return 0f;

        if (rawValue < midValue)
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        else
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);

        // 추가적인 오프셋 조정 (-0.26f)
        return normalizedValue - 0.26f;
    }

    // 오른쪽 스틱의 좌우 축 정규화 메서드
    private static float NormalizeRightStickHorizontal(byte rawValue)
    {
        float leftMax = 10;
        float midValue = 135;
        float rightMax = 253;
        float normalizedValue;

        if (rawValue == midValue) return 0f;

        if (rawValue < midValue)
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        else
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);

        // 추가 오프셋 +0.03f
        normalizedValue += 0.03f;

        // 범위 초과 시 클램프
        if (normalizedValue > 1) normalizedValue = 1;
        else if (normalizedValue < -1) normalizedValue = -1;

        return normalizedValue;
    }

    // 왼쪽 스틱의 상하 축 정규화 메서드
    // 0~255 범위를 -1~1로 맵핑하는 로직
    // midValue=128(중간값), maxValue=255(최대값) 가정
    private static float NormalizeLeftStickVertical(byte rawValue)
    {
        float midValue = 128f;
        float maxValue = 255f;
        float normalizedValue = (rawValue - midValue) / (maxValue - midValue);
        normalizedValue *= 2f; // -1~1 범위로 스케일링
        return normalizedValue / 2; // (결과적으로 -1~1 범위 유지)
    }

    // 오른쪽 스틱의 상하 축 정규화 메서드
    // 로직은 왼쪽 스틱과 동일하게 작동
    private static float NormalizeRightStickVertical(byte rawValue)
    {
        float midValue = 128f;
        float maxValue = 255f;
        float normalizedValue = (rawValue - midValue) / (maxValue - midValue);
        normalizedValue *= 2f; // -1~1 범위로 스케일링
        return normalizedValue / 2;
    }

}

[StructLayout(LayoutKind.Explicit, Size = 8)] // HID 포맷에 맞춘 구조체 크기 지정
internal struct DroneControllerState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('H', 'I', 'D');

    [FieldOffset(0)] public byte rawData0;             // 첫 번째 바이트(사용 여부에 따라 남겨둠)
    [FieldOffset(1)] public byte rightStickHorizontal; // 두 번째 바이트: 오른쪽 스틱 좌우
    [FieldOffset(2)] public byte leftStickHorizontal;  // 세 번째 바이트: 왼쪽 스틱 좌우
    [FieldOffset(3)] public byte rightStickVertical;   // 네 번째 바이트: 오른쪽 스틱 상하
    [FieldOffset(4)] public byte leftStickVertical;    // 다섯 번째 바이트: 왼쪽 스틱 상하
    [FieldOffset(5)] public byte rawData5;             // 여섯 번째 바이트
    [FieldOffset(6)] public byte rawData6;             // 일곱 번째 바이트
    [FieldOffset(7)] public byte rawData7;             // 여덟 번째 바이트(남은 바이트)
}
