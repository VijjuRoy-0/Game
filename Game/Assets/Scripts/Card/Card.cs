using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private static Card instance;
    public static Card Instance {  get { return instance; } }
    public int cardId;

    public Image backImage;
    public Image frontImage;

    private bool isFlipped = false;
    private bool isMatched = false;
    private Button cardButton;

    public Transform visualRoot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(OnCardClicked);
    }
    public void Initialize(Sprite faceSprite, int id)
    {
        cardId = id;
        frontImage.sprite = faceSprite;
        isFlipped = false;
        isMatched = false;
        backImage.gameObject.SetActive(true);
        cardButton.interactable = true;
    }
    public void OnCardClicked()
    {
        Debug.Log("Card Clicked: ID " + cardId);
        if (GameManager.instance.isChecking || isFlipped || isMatched) return;

        FlipCard();
        GameManager.instance.OnCardFlipped(this);
    }
    public void FlipCard()
    {
        isFlipped = true;
        cardButton.interactable = false;

        backImage.gameObject.SetActive(false);
        StartCoroutine(FlipAnimation(true));
    }
    public void FlipBack()
    {
        isFlipped = false;
        
        backImage.gameObject.SetActive(true);
        StartCoroutine(FlipAnimation(false));

        cardButton.interactable = true;
    }
    public void SetMatched()
    {
        isMatched = true;
        cardButton.interactable = false;

        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        group.alpha = 0f;
    }

    IEnumerator FlipAnimation(bool showFront)
    {
        float time = 0f;
        float duration = 0.3f;
        Quaternion start = visualRoot.rotation;
        Quaternion mid = Quaternion.Euler(0, 90f, 0);
        Quaternion end = showFront ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180f, 0);

        while (time < duration / 2)
        {
            visualRoot.rotation = Quaternion.Slerp(start, mid, (time / (duration / 2)));
            time += Time.deltaTime;
            yield return null;
        }
        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);

        time = 0f;

        while (time < duration / 2)
        {
            visualRoot.rotation = Quaternion.Slerp(mid, end, (time / (duration / 2)));
            time += Time.deltaTime;
            yield return null;
        }
         visualRoot.rotation = end;

        if(!isMatched)
            cardButton.interactable=true;
    }
    public bool IsMatched() => isMatched;

    public void SetMatchedImmediately()
    {
        isMatched = true;
        backImage.gameObject.SetActive(false);
        frontImage.canvasRenderer.SetAlpha(1f);
        GetComponent<Button>().interactable = false;

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null) group.alpha = 0f;
    }
}