using UnityEngine;
using UnityEngine.UI;

public class VolumeControl2 : MonoBehaviour //stolen from old game 
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;

    [Header("Audio Groups")]
    public GameObject musicGroup;
    public GameObject soundEffectsGroup;

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float soundEffectsVolume = 1f;

    private void Start()
    {
       
        if (masterVolumeSlider != null) masterVolumeSlider.value = masterVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (soundEffectsVolumeSlider != null) soundEffectsVolumeSlider.value = soundEffectsVolume;

       
        if (masterVolumeSlider != null) masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicVolumeSlider != null) musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        if (soundEffectsVolumeSlider != null) soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
    }

    private void SetMasterVolume(float value)
    {
        masterVolume = value;
        UpdateVolumes();
    }

    private void SetMusicVolume(float value)
    {
        musicVolume = value;
        UpdateVolumes();
    }

    private void SetSoundEffectsVolume(float value)
    {
        soundEffectsVolume = value;
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        // Update music volume
        if (musicGroup != null)
        {
            foreach (AudioSource audioSource in musicGroup.GetComponentsInChildren<AudioSource>())
            {
                audioSource.volume = masterVolume * musicVolume;
            }
        }

        // Update sound effects volume
        if (soundEffectsGroup != null)
        {
            foreach (AudioSource audioSource in soundEffectsGroup.GetComponentsInChildren<AudioSource>())
            {
                audioSource.volume = masterVolume * soundEffectsVolume;
            }
        }
    }
}
