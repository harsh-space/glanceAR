using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashAnimator : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI logoText;
    public CanvasGroup taglineCanvasGroup;
    public CanvasGroup splashCanvasGroup;

    [Header("Animation Settings")]
    public float logoAnimDuration = 1.0f;
    public float glowPulseDuration = 0.8f;
    public float taglineFadeDuration = 0.7f;
    public float holdDuration = 1.5f;  
    public float fadeOutDuration = 0.5f;  

    [Header("Advanced Effects")]
    public bool enableGlowPulse = true;
    public bool enableTaglineSlide = true;

    private Vector3 logoStartScale = new Vector3(0.75f, 0.75f, 1f);
    private Vector3 logoEndScale = Vector3.one;
    private Color cyanBright = new Color(0f, 0.85f, 1f, 1f); 
    private Color cyanDark = new Color(0f, 0.55f, 0.75f, 1f);

    void Start()
    {
        
        logoText.transform.localScale = logoStartScale;
        logoText.color = cyanBright;
        taglineCanvasGroup.alpha = 0f;

        
        if (enableTaglineSlide)
        {
            RectTransform taglineRect = taglineCanvasGroup.GetComponent<RectTransform>();
            taglineRect.anchoredPosition = new Vector2(taglineRect.anchoredPosition.x, 
                                                       taglineRect.anchoredPosition.y - 30f);
        }

       
        StartCoroutine(AnimateSequence());
    }

    IEnumerator AnimateSequence()
    {
        
        float elapsed = 0f;
        while (elapsed < logoAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / logoAnimDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            logoText.transform.localScale = Vector3.Lerp(logoStartScale, logoEndScale, easedT);
            yield return null;
        }
        logoText.transform.localScale = logoEndScale;

       
        if (enableGlowPulse)
        {
            elapsed = 0f;
            while (elapsed < glowPulseDuration)
            {
                elapsed += Time.deltaTime;
                float pulse = (Mathf.Sin(elapsed * 4f) + 1f) / 2f;
                logoText.color = Color.Lerp(cyanDark, cyanBright, pulse);
                yield return null;
            }
            logoText.color = cyanBright;
        }

        
        yield return new WaitForSeconds(0.15f);

        RectTransform taglineRect = taglineCanvasGroup.GetComponent<RectTransform>();
        Vector2 taglineStartPos = taglineRect.anchoredPosition;
        Vector2 taglineEndPos = new Vector2(taglineStartPos.x, taglineStartPos.y + 30f);

        elapsed = 0f;
        while (elapsed < taglineFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / taglineFadeDuration;
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            
            taglineCanvasGroup.alpha = easedT;
            
            if (enableTaglineSlide)
            {
                taglineRect.anchoredPosition = Vector2.Lerp(taglineStartPos, taglineEndPos, easedT);
            }
            yield return null;
        }
        
        taglineCanvasGroup.alpha = 1f;
        if (enableTaglineSlide)
        {
            taglineRect.anchoredPosition = taglineEndPos;
        }

        
        float holdElapsed = 0f;
        while (holdElapsed < holdDuration)
        {
            holdElapsed += Time.deltaTime;
            float breathe = 1f + (Mathf.Sin(holdElapsed * 2f) * 0.02f);
            logoText.transform.localScale = logoEndScale * breathe;
            yield return null;
        }
        logoText.transform.localScale = logoEndScale;

       
        elapsed = 0f;
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            
            
            splashCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            
            yield return null;
        }

   
        splashCanvasGroup.alpha = 0f;
        splashCanvasGroup.gameObject.SetActive(false);
        
      
    }
}