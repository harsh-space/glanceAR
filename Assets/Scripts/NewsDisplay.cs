using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewsDisplay : MonoBehaviour
{
    [Header("Headline Text Components")]
    public TextMeshProUGUI headline1;
    public TextMeshProUGUI headline2;
    public TextMeshProUGUI headline3;

    [Header("Source Text Components (optional)")]
    public TextMeshProUGUI source1; 
    public TextMeshProUGUI source2;
    public TextMeshProUGUI source3;

    [Header("News Card GameObjects")]
    public GameObject newsCard1;
    public GameObject newsCard2;
    public GameObject newsCard3;

    public NewsAPIHandler api;

    [Header("Text Settings")]
    public bool showFullHeadlines = true;
    
    [Range(0.5f, 1.5f)]
    public float lineSpacing = 0.8f;
    
    private string currentCategory = "general"; 
    private List<Article> currentArticles = new List<Article>();

    void Start()
    {
        SetupTextComponent(headline1);
        SetupTextComponent(headline2);
        SetupTextComponent(headline3);
        
       
        api.FetchNewsByCategory(currentCategory);
    }

    void SetupTextComponent(TextMeshProUGUI textComponent)
    {
        if (textComponent == null) return;
        
        textComponent.textWrappingMode = TextWrappingModes.Normal;
        textComponent.enableAutoSizing = false;
        textComponent.fontSize = 40;
        textComponent.lineSpacing = lineSpacing;
        textComponent.overflowMode = TextOverflowModes.Truncate;
    }

   
    public void RefreshNews()
    {
        Debug.Log("RefreshNews called for category: " + currentCategory);
        RefreshCardsWithAnimation();
    }

    
    public void SetCategory(string category)
    {
        currentCategory = category;
        Debug.Log("Category changed to: " + category);
        RefreshCardsWithAnimation();
    }

   
    public void RefreshCardsWithAnimation() 
    {
        StartCoroutine(AnimateCardsRefresh());
    }

    IEnumerator AnimateCardsRefresh() 
    {
      
        yield return StartCoroutine(FadeOutAllCards(0.2f));
        
        
        api.FetchNewsByCategory(currentCategory);
        
       
    }

   
    public void TriggerFadeIn()
    {
        StartCoroutine(FadeInCardsWithSlide());
    }

    IEnumerator FadeOutAllCards(float duration) 
    {
        CanvasGroup[] cardGroups = new CanvasGroup[3];
        cardGroups[0] = newsCard1.GetComponent<CanvasGroup>();
        cardGroups[1] = newsCard2.GetComponent<CanvasGroup>();
        cardGroups[2] = newsCard3.GetComponent<CanvasGroup>();
        
        float elapsed = 0f;
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            
            foreach (var group in cardGroups) 
            {
                if (group != null) group.alpha = alpha;
            }
            
            yield return null;
        }
    }

    IEnumerator FadeInCardsWithSlide() 
    {
        GameObject[] cards = { newsCard1, newsCard2, newsCard3 };
        
        for (int i = 0; i < cards.Length; i++) 
        {
            StartCoroutine(SlideAndFadeCard(cards[i]));
            yield return new WaitForSeconds(0.1f); 
        }
    }

    IEnumerator SlideAndFadeCard(GameObject card) 
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        CanvasGroup group = card.GetComponent<CanvasGroup>();
        
        if (group == null) group = card.AddComponent<CanvasGroup>();
        
        Vector2 endPos = rect.anchoredPosition;
        Vector2 startPos = endPos + new Vector2(0, -10);
        
        group.alpha = 0f;
        rect.anchoredPosition = startPos;
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            group.alpha = t;
            
            yield return null;
        }
        
        rect.anchoredPosition = endPos;
        group.alpha = 1f;
    }

    public void UpdateHeadlinesWithSources(List<Article> articles)
    {
        currentArticles = articles;
        Debug.Log("Received " + articles.Count + " articles with sources");
      
        if (articles.Count > 0)
        {
            headline1.text = FormatHeadline(articles[0].title);
            if (source1 != null) source1.text = GetSourceName(articles[0]);
        }
        else
        {
            headline1.text = "No headline available";
            if (source1 != null) source1.text = "";
        }

        if (articles.Count > 1)
        {
            headline2.text = FormatHeadline(articles[1].title);
            if (source2 != null) source2.text = GetSourceName(articles[1]);
        }
        else
        {
            headline2.text = "No headline available";
            if (source2 != null) source2.text = "";
        }

        if (articles.Count > 2)
        {
            headline3.text = FormatHeadline(articles[2].title);
            if (source3 != null) source3.text = GetSourceName(articles[2]);
        }
        else
        {
            headline3.text = "No headline available";
            if (source3 != null) source3.text = "";
        }

        Debug.Log("Headlines and sources updated!");
    }

    
    public void UpdateHeadlines(List<string> h)
    {
        Debug.Log("Received " + h.Count + " headlines");
        
        headline1.text = h.Count > 0 ? FormatHeadline(h[0]) : "No headline available";
        headline2.text = h.Count > 1 ? FormatHeadline(h[1]) : "No headline available";
        headline3.text = h.Count > 2 ? FormatHeadline(h[2]) : "No headline available";
    
        if (source1 != null) source1.text = "";
        if (source2 != null) source2.text = "";
        if (source3 != null) source3.text = "";
        
        Debug.Log("Headlines updated!");
    }

    string GetSourceName(Article article)
    {
        if (article != null && article.source != null && !string.IsNullOrEmpty(article.source.name))
        {
            return article.source.name;
        }
        return "News";
    }

    string FormatHeadline(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        int dashIndex = text.LastIndexOf(" - ");
        if (dashIndex > 0 && dashIndex > text.Length - 30)
        {
            text = text.Substring(0, dashIndex).Trim();
        }

        if (!showFullHeadlines && text.Length > 100)
        {
            text = text.Substring(0, 100).Trim() + "...";
        }

        return text;
    }
}