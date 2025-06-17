/*
A script to control the behavior of the ERP groups (all with 4 ERP Tags)
Implements the logic of max 3 selections per group before moving on with the task 
*/

using System;
using UnityEngine;

public class ERPGroupController : MonoBehaviour
{
    public bool isGroupLocked { get; private set; } = false;
    private int attemptCount = 0;
    private const int maxAttempts = 3;

    public event Action<bool> OnGroupFinished; // UI feedback

    [Header("Triggers to simulate correct response")]
    public StepAdvance stepTrigger;
    public SceneFader sceneFader;

    public void HandleSelection(bool isCorrect)
    {
        if (isGroupLocked) return;

        attemptCount++;

        if (isCorrect)
        {
            isGroupLocked = true;
            Debug.Log($"Correct selection on attempt {attemptCount} for {gameObject.name}");
            OnGroupFinished?.Invoke(true);
            FireTriggers(); // trigger actions after success
        }
        else
        {
            Debug.LogWarning($"Incorrect selection on attempt {attemptCount} for {gameObject.name}");

            if (attemptCount >= maxAttempts)
            {
                isGroupLocked = true;
                Debug.Log($"Max attempts reached for {gameObject.name}. Simulating success outcome.");
                OnGroupFinished?.Invoke(false);
                FireTriggers(); // same outcome as correct
            }
        }
    }

    private void FireTriggers()
    {

        if (stepTrigger != null)
            stepTrigger.OnBCISelected();

        if (sceneFader != null)
            sceneFader.TriggerFade(); 
    }
}
