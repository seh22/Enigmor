using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // ����� ���� ��
    public float BackgroundVolume { get; private set; } = 1.0f;

    // ȿ���� ���� ��
    public float EffectVolume { get; private set; } = 1.0f;

    // ���� Ȱ��ȭ ���θ� �����ϴ� ����
    public bool VibrationEnabled { get; private set; } = true;

    // PlayerPrefs Ű �̸� ���
    private const string BackgroundVolumeKey = "BackgroundVolume";
    private const string EffectVolumeKey = "EffectVolume";
    private const string VibrationKey = "VibrationEnabled";

    public override void Awake()
    {
        base.Awake(); // Singleton Awake ȣ��
        LoadSettings(); // ����� ���� �ε�
    }

    private void LoadSettings()
    {
        BackgroundVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, 1.0f); // ����� ���� �ε� (�⺻�� 1.0)
        EffectVolume = PlayerPrefs.GetFloat(EffectVolumeKey, 1.0f);         // ȿ���� ���� �ε� (�⺻�� 1.0)
        VibrationEnabled = PlayerPrefs.GetInt(VibrationKey, 1) == 1;       // ���� ���� �ε� (�⺻�� true)
    }

    // ����� ����

    /// <summary>
    /// ����� ���� ����
    /// </summary>
    public void SaveBackgroundVolume(float volume)
    {
        BackgroundVolume = volume; // ���� ������Ʈ
        PlayerPrefs.SetFloat(BackgroundVolumeKey, volume); // PlayerPrefs�� ����
        PlayerPrefs.Save(); // ���� ����
    }

    // ȿ���� ����

    /// <summary>
    /// ȿ���� ���� ����
    /// </summary>
    public void SaveEffectVolume(float volume)
    {
        EffectVolume = volume; // ���� ������Ʈ
        PlayerPrefs.SetFloat(EffectVolumeKey, volume); // PlayerPrefs�� ����
        PlayerPrefs.Save(); // ���� ����
    }

    // ����

    /// <summary>
    /// ���� Ȱ��ȭ ���� ����
    /// </summary>
    public void ToggleVibration(bool enabled)
    {
        VibrationEnabled = enabled; // ���� ������Ʈ
        PlayerPrefs.SetInt(VibrationKey, enabled ? 1 : 0); // PlayerPrefs�� ����
        PlayerPrefs.Save(); // ���� ����
    }
}
