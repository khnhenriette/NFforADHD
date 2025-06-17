/*
A script to handle the overall pass or fail decision for the Winter scene 
Depending on the amount of correct selections made the next cut-scene will either be success & reward item or failure and no reward item 
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FailureThresholdHandler : MonoBehaviour
{
    [Header("UI Tracking")]
    public FeedbackUIController feedbackUI;

    [Header("Threshold Settings")]
    public int maxFailuresAllowed = 4;

    [Header("Events")]
    public UnityEvent OnPass;
    public UnityEvent OnFail;

    private int currentFailures = 0;
    private bool outcomeTriggered = false;

    void OnEnable()
    {
        feedbackUI.OnResultAdded += HandleResult;
    }


    void OnDisable()
    {
        if (feedbackUI != null)
            feedbackUI.OnResultAdded -= HandleResult;
    }


    private void HandleResult(bool isSuccess)
    {
        if (outcomeTriggered) return;

        if (!isSuccess)
            currentFailures++;

        if (feedbackUI.TotalResults >= feedbackUI.TotalExpectedGroups)
        {
            outcomeTriggered = true; 
            StartCoroutine(TriggerOutcomeWithDelay(5f));
        }
    }

    private IEnumerator TriggerOutcomeWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentFailures > maxFailuresAllowed)
        {
            Debug.Log($"More than {maxFailuresAllowed} failures. Triggering fail path.");
            OnFail?.Invoke();
        }
        else
        {
            Debug.Log($"Within failure threshold. Triggering pass path.");
            OnPass?.Invoke();
        }
    }
}
