using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NewsAPIHandler : MonoBehaviour
{
    [Header("API Settings")]
    public string apiKey = "YOUR_API_KEY_HERE";
    public string country = "us";
    public int pageSize = 20;

    void Awake()
    {
        pageSize = 20;
        country = "us";
    }

    string BuildURL()
    {
        string timestamp = System.DateTime.Now.Ticks.ToString();
        return string.Format("https://newsapi.org/v2/top-headlines?country={0}&pageSize={1}&apiKey={2}&t={3}", 
                       country, pageSize, apiKey, timestamp);
    }

    string BuildURLWithCategory(string category)
    {
        string timestamp = System.DateTime.Now.Ticks.ToString();
        return string.Format("https://newsapi.org/v2/top-headlines?country={0}&category={1}&pageSize={2}&apiKey={3}&t={4}", 
                       country, category, pageSize, apiKey, timestamp);
    }

    public void FetchNewsByCategory(string category)
    {
        StartCoroutine(FetchNewsWithCategory(category));
    }

    IEnumerator FetchNewsWithCategory(string category)
    {
        string url = BuildURLWithCategory(category);
        Debug.Log("Fetching " + category + " news from: " + url);
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Cache-Control", "no-cache");
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                
                List<string> dummyHeadlines = new List<string>() 
                { 
                    "Unable to load " + category + " news", 
                    "Check internet connection", 
                    "Tap refresh to try again" 
                };
                
                NewsDisplay display = FindFirstObjectByType<NewsDisplay>();
                if (display != null)
                {
                    display.UpdateHeadlines(dummyHeadlines);
                }
            }
            else
            {
                List<Article> articles = ParseNewsToArticles(request.downloadHandler.text);
                
                if (articles.Count == 0)
                {
                    Debug.LogWarning("No articles found for " + category);
                    List<string> fallback = new List<string>() 
                    { 
                        "No " + category + " news available", 
                        "Please refresh", 
                        "Or try another category" 
                    };
                    
                    NewsDisplay display = FindFirstObjectByType<NewsDisplay>();
                    if (display != null)
                    {
                        display.UpdateHeadlines(fallback);
                    }
                }
                else
                {
                    NewsDisplay display = FindFirstObjectByType<NewsDisplay>();
                    if (display != null)
                    {
                        display.UpdateHeadlinesWithSources(articles);
                        display.TriggerFadeIn();  
                    }
                }
            }            
        }
    }

    public IEnumerator FetchNews(System.Action<List<string>> callback)
    {
        string url = BuildURL();
        Debug.Log("Fetching news from: " + url);
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Cache-Control", "no-cache");
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                
                List<string> dummyHeadlines = new List<string>() 
                { 
                    "Unable to load news", 
                    "Check internet connection", 
                    "Tap refresh to try again" 
                };
                callback?.Invoke(dummyHeadlines);
            }
            else
            {
                List<string> headlines = ParseNews(request.downloadHandler.text);
                
                if (headlines.Count == 0)
                {
                    Debug.LogWarning("No headlines found, using fallback");
                    headlines = new List<string>() 
                    { 
                        "No news available", 
                        "Please refresh", 
                        "Or check back later" 
                    };
                }
                
                callback?.Invoke(headlines);
            }
        }
    }

    List<string> ParseNews(string json)
    {
        List<string> headlines = new List<string>();
        
        try
        {
            NewsResponse response = JsonUtility.FromJson<NewsResponse>(json);
            
            if (response != null && response.articles != null && response.articles.Length > 0)
            {
                List<string> allHeadlines = new List<string>();
                
                foreach (Article article in response.articles)
                {
                    if (article != null && !string.IsNullOrEmpty(article.title))
                    {
                        allHeadlines.Add(article.title);
                    }
                }
                
                Debug.Log("Found " + allHeadlines.Count + " headlines from US news");
                
                if (allHeadlines.Count > 0)
                {
                    headlines = GetRandomHeadlines(allHeadlines, 3);
                    Debug.Log("✓ Selected 3 random headlines");
                }
            }
            else
            {
                Debug.LogWarning("API returned no articles");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
        }
        
        return headlines;
    }

    List<Article> ParseNewsToArticles(string json)
    {
        List<Article> articles = new List<Article>();
        
        try
        {
            NewsResponse response = JsonUtility.FromJson<NewsResponse>(json);
            
            if (response != null && response.articles != null && response.articles.Length > 0)
            {
                List<Article> allArticles = new List<Article>();
                
                foreach (Article article in response.articles)
                {
                    if (article != null && !string.IsNullOrEmpty(article.title))
                    {
                        allArticles.Add(article);
                    }
                }
                
                Debug.Log("Found " + allArticles.Count + " articles");
                
                if (allArticles.Count > 0)
                {
                    articles = GetRandomArticles(allArticles, 3);
                    Debug.Log("✓ Selected 3 random articles");
                }
            }
            else
            {
                Debug.LogWarning("API returned no articles");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Parse error: " + e.Message);
        }
        
        return articles;
    }

    List<string> GetRandomHeadlines(List<string> allHeadlines, int count)
    {
        List<string> shuffled = new List<string>(allHeadlines);
        
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            string temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }
        
        int takeCount = Mathf.Min(count, shuffled.Count);
        return shuffled.GetRange(0, takeCount);
    }

    List<Article> GetRandomArticles(List<Article> allArticles, int count)
    {
        List<Article> shuffled = new List<Article>(allArticles);
        
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Article temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }
        
        int takeCount = Mathf.Min(count, shuffled.Count);
        return shuffled.GetRange(0, takeCount);
    }
}

[System.Serializable]
public class NewsResponse
{
    public string status;
    public int totalResults;
    public Article[] articles;
}

[System.Serializable]
public class Article
{
    public Source source;
    public string title;
    public string description;
    public string url;
    public string urlToImage;
    public string publishedAt;
}

[System.Serializable]
public class Source
{
    public string id;
    public string name;
}