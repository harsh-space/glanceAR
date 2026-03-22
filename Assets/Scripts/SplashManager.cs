using System.Collections;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    public CanvasGroup splashCanvas;
    public GameObject newsCanvas;

    public float splashTime = 2f;
    public float fadeTime = 1f;

    void Start()
    {
        
        splashCanvas.alpha = 1f;
        splashCanvas.gameObject.SetActive(true);
        newsCanvas.SetActive(false);

        StartCoroutine(RunSplash());
    }

    IEnumerator RunSplash()
    {
        
        yield return new WaitForSeconds(splashTime);

        
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            splashCanvas.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        splashCanvas.gameObject.SetActive(false);
        newsCanvas.SetActive(true);
    }
}
