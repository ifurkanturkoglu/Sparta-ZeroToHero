using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    

    public int gold = 0;
    public int score = 0;
    public int wave = 1;

    private void Awake()
    {
        Instance = this;
        gold = 1000;
        print("Selam");
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