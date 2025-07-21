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
        GenerateBoard(rows, cols);
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
             
        }
        else
        {
            flippedCards[0].FlipBack();
            flippedCards[1].FlipBack();
        }

        flippedCards.Clear();
        isChecking = false;
    }
    private IEnumerator SetupBoardCoroutine(int numRows, int numCols, int totalCards)
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
        }
    }


}
