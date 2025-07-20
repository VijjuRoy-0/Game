using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GenerateBoard(2, 2);
    }
    public void GenerateBoard(int rows, int columns)
    {
        int totalCards = rows * columns;
        cardIds.Clear();

        for(int i = 0; i < totalCards/2; i++)
        {
            cardIds.Add(i);
            cardIds.Add(i);
        }

        Shuffle(cardIds);

        for(int i = 0;i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab,gridParent);
            Card card = cardObj.GetComponent<Card>();
            card.Initialize(cardSprite[cardIds[i]], cardIds[i]);
        }
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

        if (flippedCards.Count == 2)
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

    
}
