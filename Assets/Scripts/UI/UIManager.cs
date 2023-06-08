using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsPanel;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public Slider hpBar;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        PausePanel();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject[] closeableUIs = GameObject.FindGameObjectsWithTag("ClosableUI");
            if (closeableUIs.Length > 0)
            {
                foreach (var item in closeableUIs)
                {
                    item.SetActive(false);
                }
            }
        }
    }
    #region Panels
    public void PausePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.FindGameObjectsWithTag("ClosableUI").Length <= 0)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
        }
    }
    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void SettingsClose()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
    #region InGameUI

    public IEnumerator UpdateHpBar(float potionHealthPercent, float health, bool increase)
    {
        float time = 0;
        float increaseType = increase == true ? 1 : -1;
        while (time < potionHealthPercent)
        {

            time += Time.deltaTime * 15;
            health = health + (increaseType * Time.deltaTime);
            hpBar.value = hpBar.value + (increaseType * Time.deltaTime * 15);
            yield return null;
        }
    }
    public void MarketOpen(GameObject marketUI)
    {
        marketUI.SetActive(true);
    }

    #endregion


    public void GameStartUpdateUI(float health)
    {
        hpBar.value = health;
    }
}
