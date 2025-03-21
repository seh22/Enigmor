using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // 배경음 볼륨 값
    public float BackgroundVolume { get; private set; } = 1.0f;

    // 효과음 볼륨 값
    public float EffectVolume { get; private set; } = 1.0f;

    // 진동 활성화 여부를 저장하는 변수
    public bool VibrationEnabled { get; private set; } = true;

    // PlayerPrefs 키 이름 상수
    private const string BackgroundVolumeKey = "BackgroundVolume";
    private const string EffectVolumeKey = "EffectVolume";
    private const string VibrationKey = "VibrationEnabled";

    public override void Awake()
    {
        base.Awake(); // Singleton Awake 호출
        LoadSettings(); // 저장된 설정 로드
    }

    private void LoadSettings()
    {
        BackgroundVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, 1.0f); // 배경음 볼륨 로드 (기본값 1.0)
        EffectVolume = PlayerPrefs.GetFloat(EffectVolumeKey, 1.0f);         // 효과음 볼륨 로드 (기본값 1.0)
        VibrationEnabled = PlayerPrefs.GetInt(VibrationKey, 1) == 1;       // 진동 설정 로드 (기본값 true)
    }

    // 배경음 볼륨

    /// <summary>
    /// 배경음 볼륨 저장
    /// </summary>
    public void SaveBackgroundVolume(float volume)
    {
        BackgroundVolume = volume; // 변수 업데이트
        PlayerPrefs.SetFloat(BackgroundVolumeKey, volume); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 저장 적용
    }

    // 효과음 볼륨

    /// <summary>
    /// 효과음 볼륨 저장
    /// </summary>
    public void SaveEffectVolume(float volume)
    {
        EffectVolume = volume; // 변수 업데이트
        PlayerPrefs.SetFloat(EffectVolumeKey, volume); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 저장 적용
    }

    // 진동

    /// <summary>
    /// 진동 활성화 여부 저장
    /// </summary>
    public void ToggleVibration(bool enabled)
    {
        VibrationEnabled = enabled; // 변수 업데이트
        PlayerPrefs.SetInt(VibrationKey, enabled ? 1 : 0); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 저장 적용
    }
}
