using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManament : MonoBehaviour
{
    private static ScoreManament Instance;
    public static ScoreManament instance {  get { return Instance; } }

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int score, int combo)
    {
       scoreText.text = $"Score:{score} Combo x{combo}";
    }
}
