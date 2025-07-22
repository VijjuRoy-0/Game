using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI popupText;
    public TMP_Dropdown dropdown;

    public void Start()
    {
       // menu.SetActive(true);
        popupText.enabled = false;
    }
    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll(); // clear saved data
        
       switch(dropdown.value)
        {
            case 0: PlayerPrefs.SetInt("Rows", 2); PlayerPrefs.SetInt("Cols", 2);
                break;
            case 1: PlayerPrefs.SetInt("Rows", 4); PlayerPrefs.SetInt("Cols", 4);
                break;
            case 2: PlayerPrefs.SetInt("Rows", 6); PlayerPrefs.SetInt("Cols", 6); 
                break;
        }
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        if(PlayerPrefs.HasKey("SaveData")) 
        { 
            SceneManager.LoadScene("Game");
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
