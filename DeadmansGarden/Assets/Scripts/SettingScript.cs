using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using Photon.Pun;
public class SettingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioMixer audio;
    [SerializeReference] private PlayerAimController playerAimController;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider aimSensitivity, normalSensitivity;
    Resolution[] resolutions;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].height == Screen.currentResolution.height
                    && resolutions[i].width == Screen.currentResolution.width)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(resolutions.Length - 1);
         
        }
    }

    private void Update()
    {
        if(playerAimController != null)
        {
            return;
           
        }
        else
        {
            if (photonView.IsMine)
            {
              
                playerAimController = FindObjectOfType<PlayerAimController>();
            }

        }
       
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audio.SetFloat("volume", volume);

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetNormalSensitivity()
    {
        playerAimController.SetNormalSensitivity(normalSensitivity.value);
    }
    public void SetAimSensitivity()
    {
        playerAimController.SetAimSensitivity(aimSensitivity.value);
    }
}
