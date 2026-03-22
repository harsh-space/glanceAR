using System.Collections;
using UnityEngine;

public class RefreshButtonController : MonoBehaviour
{
    public RectTransform iconTransform;
    public float spinDuration = 0.5f;
    
    private bool isSpinning = false;

    public void SpinAndRefresh()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinIcon());
        }
    }

    IEnumerator SpinIcon()
    {
        isSpinning = true;
        
        float elapsed = 0f;
        float startRotation = iconTransform.localEulerAngles.z;
        float targetRotation = startRotation + 360f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / spinDuration);
            
            float currentRotation = Mathf.Lerp(startRotation, targetRotation, t);
            iconTransform.localEulerAngles = new Vector3(0, 0, currentRotation);
            
            yield return null;
        }

        iconTransform.localEulerAngles = new Vector3(0, 0, targetRotation % 360f);
        isSpinning = false;
    }
}