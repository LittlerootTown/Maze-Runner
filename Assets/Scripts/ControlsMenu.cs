using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Reference to the CanvasGroup
    public float fadeDuration = 1f; // Duration of the fade effect


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeCanvas(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeCanvas(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
