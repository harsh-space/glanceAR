using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonPressEffect : MonoBehaviour 
{
    private Vector3 originalScale;
    
    void Start() 
    {
        originalScale = transform.localScale;
        
        
        Button btn = GetComponent<Button>();
        if (btn != null) 
        {
            btn.onClick.AddListener(OnButtonPress);
        }
    }
    
    void OnButtonPress() 
    {
        StopAllCoroutines(); 
        StartCoroutine(PressAnimation());
    }
    
    IEnumerator PressAnimation() 
    {
        
        float duration = 0.1f;
        float elapsed = 0f;
        
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.05f, elapsed / duration);
            transform.localScale = originalScale * scale;
            yield return null;
        }
        
        
        elapsed = 0f;
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1.05f, 1f, elapsed / duration);
            transform.localScale = originalScale * scale;
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
}