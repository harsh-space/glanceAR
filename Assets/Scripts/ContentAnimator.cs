using UnityEngine;
using System.Collections;

public class ContentAnimator : MonoBehaviour 
{
    public RectTransform contentPanel;     
    public CanvasGroup contentCanvasGroup;  
    public GameObject instructionText;      
    
    public GameObject[] newsCards;          
    
    private Vector2 hiddenPosition = new Vector2(0, -500);
    private Vector2 visiblePosition = new Vector2(0, 0);
    
    public void OnTargetFound() 
    {
        StartCoroutine(AnimateContentIn());
    }
    
    IEnumerator AnimateContentIn() 
    {
       
        if (instructionText != null) 
        {
            StartCoroutine(FadeOut(instructionText, 0.3f));
        }
        
      
        float duration = 0.4f;
        float elapsed = 0f;
        
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            
            float easeT = 1f - Mathf.Pow(1f - t, 3f);
            
            contentPanel.anchoredPosition = Vector2.Lerp(hiddenPosition, visiblePosition, easeT);
            contentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            yield return null;
        }
        
        contentPanel.anchoredPosition = visiblePosition;
        contentCanvasGroup.alpha = 1f;
        
       
        StartCoroutine(StaggeredCardFadeIn());
    }
    
    IEnumerator StaggeredCardFadeIn() 
    {
        for (int i = 0; i < newsCards.Length; i++) 
        {
            StartCoroutine(FadeInCard(newsCards[i]));
            yield return new WaitForSeconds(0.1f); 
        }
    }
    
    IEnumerator FadeInCard(GameObject card) 
    {
        CanvasGroup cardGroup = card.GetComponent<CanvasGroup>();
        if (cardGroup == null) 
        {
            cardGroup = card.AddComponent<CanvasGroup>();
        }
        
        cardGroup.alpha = 0f;
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            cardGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
        
        cardGroup.alpha = 1f;
    }
    
    IEnumerator FadeOut(GameObject obj, float duration) 
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        if (group == null) group = obj.AddComponent<CanvasGroup>();
        
        float elapsed = 0f;
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }
        
        obj.SetActive(false);
    }
}