using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager instance { get { return Instance; } }

    public GameObject cardPrefab;
    public Transform gridParent;
    public Sprite[] cardSprite;

    private List<Card> flippedCards = new List<Card>();
    private List<int> cardIds = new List<int>();

    public bool isChecking = false;

    public int rows;
    public int cols;

    private int score = 0;
    private int combo = 0;

    private GridLayoutGroup gridLayoutGroup;
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
        gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("Rows") && PlayerPrefs.HasKey("Cols"))
        {
            rows = PlayerPrefs.GetInt("Rows");
            cols = PlayerPrefs.GetInt("Cols");
        }
        if (PlayerPrefs.HasKey("SaveData"))
        {
            LoadGame();
        }
        else
        {
            score = 0;
            combo = 0;
            GenerateBoard(rows, cols);
            ScoreManament.instance.UpdateScore(score, combo);
        }
       
    }
    public void GenerateBoard(int rows, int columns)
    {
        foreach(Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        int totalCards = rows * columns;
        cardIds.Clear();

        for(int i = 0; i < totalCards/2; i++)
        {
            cardIds.Add(i);
            cardIds.Add(i);
        }

        Shuffle(cardIds);

        // Wait 1 frame to calculate layout
        StartCoroutine(SetupBoardCoroutine(rows, columns, totalCards));
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void OnCardFlipped(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2 && !isChecking)
        {
            isChecking = true;
            StartCoroutine(CheckMatch());
           
        }
    }
    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (flippedCards[0].cardId == flippedCards[1].cardId)
        {
            flippedCards[0].SetMatched();
            flippedCards[1].SetMatched();
            combo++;
            score += 10+(combo - 1) * 5;
            SoundManager.Instance.MatchSound();
        }
        else
        {
            flippedCards[0].FlipBack();
            flippedCards[1].FlipBack();
            combo = 0;
            SoundManager.Instance.MisMatchSound();
        }
        if (AllCardMatched())
        {
            SoundManager.Instance.GameOverSound();
            UIManage.Instance.endPanel.SetActive(true);
        }
        ScoreManament.instance.UpdateScore(score,combo);
        
        flippedCards.Clear();
        isChecking = false;
        SaveGame();
    }
    private IEnumerator SetupBoardCoroutine(int numRows, int numCols, int totalCards, List<int> preMatched = null)
    {
        // Wait for one frame to ensure the container's size is correctly updated by the layout system
        yield return new WaitForEndOfFrame(); ; // Wait one frame for layout to be ready

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = numCols;

        // Correctly calculate the available space, including padding and spacing
        float panelWidth = ((RectTransform)gridParent.transform).rect.width;
        float totalHorizontalPadding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float totalHorizontalSpacing = gridLayoutGroup.spacing.x * (numCols - 1);
        float availableWidth = panelWidth - totalHorizontalPadding - totalHorizontalSpacing;
        float cellWidth = availableWidth / numCols;

        float panelHeight = ((RectTransform)gridParent.transform).rect.height;
        float totalVerticalPadding = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        float totalVerticalSpacing = gridLayoutGroup.spacing.y * (numRows - 1);
        float availableHeight = panelHeight - totalVerticalPadding - totalVerticalSpacing;
        float cellHeight = availableHeight / numRows;

        // Use the smaller dimension to make square cards that fit perfectly
        float cellSize = Mathf.Min(cellWidth, cellHeight);
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        // Now, create the cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridParent);
            Card card = cardObj.GetComponent<Card>();
            card.Initialize(cardSprite[cardIds[i]], cardIds[i]);

            if (preMatched != null && preMatched.Contains(card.cardId))
                card.SetMatchedImmediately();
        }
        StartCoroutine(RevealAndHideCards());
    }
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            rows = rows,
            cols = cols,
            cardIdOrder = new List<int>(cardIds),
            matchedCardIds = new List<int>(),
            _score = score,
            _comboCount = combo
        };

        foreach (Card card in FindObjectsOfType<Card>())
        {
            if (card.IsMatched())
                data.matchedCardIds.Add(card.cardId);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        rows = data.rows;
        cols = data.cols;
        cardIds = new List<int>(data.cardIdOrder);
        score = data._score;
        combo = data._comboCount;
        ScoreManament.instance.UpdateScore(score, combo);
        StartCoroutine(SetupBoardCoroutine(rows, cols, rows * cols, data.matchedCardIds));
    }
    bool AllCardMatched()
    {
        foreach (Card card in FindObjectsOfType<Card>())
        {
            if (!card.IsMatched())
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator RevealAndHideCards()
    {
        List<Card> allCards = new List<Card>(FindObjectsOfType<Card>());

        // Show all front sides
        foreach (Card card in allCards)
        {
            card.ForceShowFront();
        }

        yield return new WaitForSeconds(1.5f); // Wait for 1 second

        // Flip them all back
        foreach (Card card in allCards)
        {
            card.ForceHideFront(); // Custom instant back flip (not animation)
        }
    }

}
[System.Serializable]
public class SaveData
{
    public int rows;
    public int cols;
    public List<int> cardIdOrder;       // the shuffled list
    public List<int> matchedCardIds;    // cards that are already matched
    public int _score;
    public int _comboCount;
}
