using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controlPanel;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void ControlsPanel()
    {
        controlPanel.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void CloseButton()
    {
        controlPanel.SetActive(false);

    }
}
