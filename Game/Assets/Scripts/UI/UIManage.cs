using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManage : MonoBehaviour
{
    public GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
