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
        StartCoroutine(FlipAnimation(true));
    }
    public void FlipBack()
    {
        isFlipped=false;
        backImage.gameObject.SetActive(true);
        StartCoroutine(FlipAnimation(false));
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

    IEnumerator FlipAnimation(bool showFront)
    {
        float time = 0f;
        float duration = 0.3f;
        Quaternion start = transform.rotation;
        Quaternion mid = Quaternion.Euler(0, 90f, 0);
        Quaternion end = showFront? Quaternion.Euler(0,0,0) : Quaternion.Euler(0,180f,0);
       
        while (time < duration/2)
        {
            transform.rotation = Quaternion.Slerp(start, mid, (time/(duration/2)));
            time += Time.deltaTime;
            yield return null;
        }
        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);

        time = 0f;

        while(time < duration / 2)
        {
            transform.rotation = Quaternion.Slerp(mid,end, (time/(duration/2)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = end;
    }
}
