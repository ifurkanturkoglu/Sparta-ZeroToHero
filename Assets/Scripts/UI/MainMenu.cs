using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool informationIsOpen;
    [SerializeField]GameObject informationPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void Information(){
        informationIsOpen = !informationIsOpen;
        informationPanel.SetActive(informationIsOpen);
    }
    public void ExitGame(){
        Application.Quit();
    }
}
