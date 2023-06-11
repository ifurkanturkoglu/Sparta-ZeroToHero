using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class FinishPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    void OnEnable()
    {
        scoreText.text = "Score: "+GameManager.Instance.score;
    }
    public void Again(){
        SceneManager.LoadScene(1);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }

}
