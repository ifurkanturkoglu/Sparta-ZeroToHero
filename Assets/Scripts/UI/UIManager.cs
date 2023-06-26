using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject controlPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject finishUI;
    public Image skill1, skill2, skill3;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI healthPotCountText, staminaPotCountText;
    public TextMeshProUGUI informationText;
    public Slider hpBar, staminaBar;

    public string interactionInfoText = "Press the 'E' key to interact.";
    public string waveInfoText = "Press 'V' to start the wave";
    public bool textOrder;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Start()
    {
        goldText.text = "Gold: " + GameManager.Instance.gold.ToString();
        healthPotCountText.text = Pots.Instance.healthPotionCount.ToString();
        staminaPotCountText.text = Pots.Instance.staminaPotionCount.ToString();
        scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        healthPotCountText.color = Color.green;
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
        if (!Pots.Instance.canPotDrink)
        {
            staminaBar.value = Player.Instance.GetStamina();
            hpBar.value = Player.Instance.GetHealth();
        }

    }
    #region Panels
    public void PausePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.FindGameObjectsWithTag("ClosableUI").Length <= 0)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            Time.timeScale = pausePanel.activeSelf ? 0 : 1;
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
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void Controls()
    {
        controlPanel.SetActive(true);
    }
    public void CloseButton()
    {
        controlPanel.SetActive(false);

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
    #region InGameUI
    public void InformationTextUpdate(string text, Color color)
    {
        informationText.text = text;
        informationText.color = color;
    }
    public IEnumerator UpdateStaminaOrHPBar(Slider slider,float potionPercent,bool increase,GameObject? pot,float speed){
        float time = 0;
        float increaseType = increase == true ? 1 : -1;
        if (pot != null)
            pot.SetActive(true);
        while (time < potionPercent)
        {
            time += Time.deltaTime * speed;
            slider.value = slider.value + (increaseType * Time.deltaTime * speed);
            yield return null;
        }
        if (pot != null)
            pot.SetActive(false);
    }

    public void FinishUIOpen()
    {
        finishUI.SetActive(true);
    }

    public void MarketOpen(GameObject marketUI)
    {
        marketUI.SetActive(true);
    }

    #endregion


    public void GameStartUpdateUI(float health, float stamina)
    {
        hpBar.value = health;
        staminaBar.value = stamina;
    }

    

    public IEnumerator SkillIconUpdate(Image skillImage, float cooldown)
    {
        float timer = 0;
        skillImage.fillAmount = 0;

        while (timer <= cooldown)
        {
            timer += Time.deltaTime;
            skillImage.fillAmount += Time.deltaTime * 1 / cooldown;
            yield return null;
        }
    }


    public void menüyegit()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Destroy(GameObject.FindGameObjectWithTag("MainMusic"));
    }

}
