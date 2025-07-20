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
        // Reset visibility
        frontImage.canvasRenderer.SetAlpha(0f);
        backImage.canvasRenderer.SetAlpha(1f);

        // Reset interaction
        GetComponent<Button>().interactable = true;

        // Reset rotation
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
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
        GetComponent<Button>().interactable = false;
        StartCoroutine(FlipAnimation(true));
        backImage.gameObject.SetActive(false);
      
    }
    public void FlipBack()
    {
        isFlipped=false;
        GetComponent<Button>().interactable = false;
        StartCoroutine(FlipAnimation(false));
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

    IEnumerator FlipAnimation(bool showFront, float duration = 0.3f)
    {
        float time = 0f;
       
        Quaternion start = transform.rotation;
        Quaternion mid = Quaternion.Euler(0, 90f, 0);
        Quaternion end = showFront? Quaternion.Euler(0,0,0) : Quaternion.Euler(0,180f,0);
       
        while (time < duration/2)
        {
            transform.rotation = Quaternion.Slerp(start, mid, (time/(duration/2)));
            time += Time.deltaTime;
            yield return null;
        }
        if (showFront)
        {
            frontImage.canvasRenderer.SetAlpha(1f);
            backImage.canvasRenderer.SetAlpha(0f);
        }
        else
        {
            frontImage.canvasRenderer.SetAlpha(0f);
            backImage.canvasRenderer.SetAlpha(1f);
        }

        time = 0f;

        while (time < duration / 2)
        {
            transform.rotation = Quaternion.Slerp(mid,end, (time/(duration/2)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = end;
       
        if (!isMatched)
            GetComponent<Button>().interactable = true;
    }
}
