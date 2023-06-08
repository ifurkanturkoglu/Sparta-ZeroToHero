using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Resolution[] resolutions;
    List<Resolution> resolutionList;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle vSyncToggle;
    int resolutionIndex = 0;
    string[] qualityLevels;
    private void Start()
    {
        SetupResolution();
        SetupQuality();
        fullscreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        fullscreenToggle.isOn = true;
        vSyncToggle.isOn = true;
    }
    private void SetupResolution()
    {
        resolutions = Screen.resolutions;
        resolutionList = new List<Resolution>();

        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionList.Add(resolutions[i]);
        }

        List<string> options = new List<string>();

        for (int i = 0; i < resolutionList.Count; i++)
        {
            string resolutionOption = resolutionList[i].width + "x" + resolutionList[i].height;
            options.Add(resolutionOption);

            if (resolutionList[i].width == Screen.width && resolutionList[i].height == Screen.height)
            {
                resolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    public void SetFullscreen()
    {
        Screen.fullScreenMode = fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void SetVSync()
    {
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
    }
    private void SetupQuality()
    {
        qualityLevels = QualitySettings.names;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(qualityLevels));

        int currentQualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }
}
