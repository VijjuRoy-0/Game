using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardId;

    public Image backImage;
    public Image frontImage;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void Initialize(Sprite faceSprite, int id)
    {
        cardId = id;
        frontImage.sprite = faceSprite;
        isFlipped = false;
        isMatched = false;
        backImage.gameObject.SetActive(true);
    }
    public void OnCardClicked()
    {
        if (isFlipped || isMatched) return;

        FlipCard();
        GameManager.instance.OnCardFlipped(this);
    }
    public void FlipCard()
    {
        isFlipped=true;
        backImage.gameObject.SetActive(false);
    }
    public void FlipBack()
    {
        isFlipped=false;
        backImage.gameObject.SetActive(true);
    }
    public void SetMatched()
    {
        isMatched=true;
        GetComponent<Button>().interactable = false;

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
}
