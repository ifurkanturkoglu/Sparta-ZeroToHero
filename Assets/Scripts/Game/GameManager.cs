using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    

    public int gold;
    public int score;
    public int wave = 1;

    public Transform goldsParent;
    public GameObject goldPrefab;

    public int dieEnemyCount=-1;
    public bool waveComplete;

    private void Awake()
    {
        Instance = this;
        gold = 111110;
        score = 0;
        
    }
    void Start()
    {
        UIManager.Instance.InformationTextUpdate(UIManager.Instance.waveInfoText,Color.green);
    }
    void Update()
    {
        if(dieEnemyCount == GenerateEnemy.Instance.enemyCount){
            waveComplete = true;
            dieEnemyCount =0;
            UIManager.Instance.InformationTextUpdate(UIManager.Instance.waveInfoText,Color.green);
        }
        if(Input.GetKeyDown(KeyCode.F7)){
            SceneManager.LoadScene(1);
        }
    }
    public void UpdateScore(int enemyScore)
    {
        score += enemyScore;
        UIManager.Instance.scoreText.text = "Score: " + score;
    }
    public void UpdateGold(int enemyGold)
    {
        gold += enemyGold;
        UIManager.Instance.goldText.text = "Gold: " + gold;
    }

    public void IncreaseWave()
    {
        wave++;
        UIManager.Instance.waveText.text = "Wave: " + wave;
    }
}