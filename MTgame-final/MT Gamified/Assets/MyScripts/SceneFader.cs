/*
A script that takes care of smooth scene transitions 
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1.5f;

    [Header("Objects to Deactivate After Fade")]
    public GameObject[] objectsToDeactivate;

    [Header("Objects to Activate After Fade")]
    public GameObject[] objectsToActivate;

    [Header("Invisible Objects to Make Visible After Fade")]
    public GameObject[] objectsToMakeVisible;


    public void TriggerFade()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Tried to trigger fade on inactive GameObject.");
            Debug.Log("TriggerFade called from " + gameObject.name);
            return;
        }

        StartCoroutine(FadeSequence());
    }


    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeToBlack());

        yield return new WaitForSeconds(0.5f); // Optional pause after fade

        // Perform changes, but delay deactivation to end of frame
        ActivateObjects();
        MakeObjectsVisible();

        yield return new WaitForEndOfFrame(); // Delay deactivation safely
        DeactivateObjects();

    }

    private IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color startColor = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }

    private IEnumerator FadeToClear()
    {
        float t = 0f;
        Color startColor = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadePanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    private void ActivateObjects()
    {
        foreach (var obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    private void DeactivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private void MakeObjectsVisible()
    {
        foreach (var obj in objectsToMakeVisible)
        {
            if (obj == null) continue;

            foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = true;
            }
        }
    }
}
