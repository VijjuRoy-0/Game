using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI popupText;

    public void Start()
    {
       // menu.SetActive(true);
        popupText.enabled = false;
    }
    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll(); // clear saved data
        SceneManager.LoadScene(1);
       // menu.SetActive(false);
    }

    public void ContinueGame()
    {
        if(PlayerPrefs.HasKey("SaveData")) 
        { 
            SceneManager.LoadScene("GameScene");
        }

        else
        {
            popupText.enabled = true;
            Debug.Log("No datat");
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
