using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CategoryManager : MonoBehaviour
{
    [Header("Button References")]
    public Button allButton;
    public Button techButton;
    public Button businessButton;
    public Button sportsButton;

    [Header("Button Images (for color changes)")]
    public Image allButtonImage;
    public Image techButtonImage;
    public Image businessButtonImage;
    public Image sportsButtonImage;

    [Header("Button Texts (for bold/regular)")]
    public TextMeshProUGUI allButtonText;
    public TextMeshProUGUI techButtonText;
    public TextMeshProUGUI businessButtonText;
    public TextMeshProUGUI sportsButtonText;

    [Header("News API Handler")]
    public NewsAPIHandler newsAPIHandler;

    
    private Color activeColor = new Color(0f, 0.851f, 1f, 1f); 
    private Color inactiveColor = new Color(0.749f, 0.965f, 1f, 1f); 

    private string currentCategory = "general";

    void Start()
    {
       
        allButton.onClick.AddListener(() => SwitchCategory("general", allButtonImage, allButtonText));
        techButton.onClick.AddListener(() => SwitchCategory("technology", techButtonImage, techButtonText));
        businessButton.onClick.AddListener(() => SwitchCategory("business", businessButtonImage, businessButtonText));
        sportsButton.onClick.AddListener(() => SwitchCategory("sports", sportsButtonImage, sportsButtonText));

        
        SetActiveButton(allButtonImage, allButtonText);
        SetInactiveButton(techButtonImage, techButtonText);
        SetInactiveButton(businessButtonImage, businessButtonText);
        SetInactiveButton(sportsButtonImage, sportsButtonText);
    }

    void SwitchCategory(string category, Image clickedButtonImage, TextMeshProUGUI clickedButtonText)
    {
       
        if (currentCategory == category) return;

        currentCategory = category;

       
        SetInactiveButton(allButtonImage, allButtonText);
        SetInactiveButton(techButtonImage, techButtonText);
        SetInactiveButton(businessButtonImage, businessButtonText);
        SetInactiveButton(sportsButtonImage, sportsButtonText);

        
        SetActiveButton(clickedButtonImage, clickedButtonText);

        
        if (newsAPIHandler != null)
        {
            newsAPIHandler.FetchNewsByCategory(category);
        }
        else
        {
            Debug.LogError("NewsAPIHandler reference is missing in CategoryManager!");
        }
    }

    void SetActiveButton(Image img, TextMeshProUGUI txt)
    {
        img.color = activeColor; 
        txt.fontStyle = FontStyles.Bold;
    }

    void SetInactiveButton(Image img, TextMeshProUGUI txt)
    {
        img.color = inactiveColor; 
        txt.fontStyle = FontStyles.Normal;
    }
}