/*
A script to control the transportation through the magical forest in the winter scene 
After a correct selection (or 3 incorrect ones) there is a fade to white (i.e. snowstorm) 
and the player is carried through the forest to the next clearing / intersection 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaypointPath
{
    public List<Transform> waypoints; // Waypoints for one segment (step)
}

public class BCISequenceController : MonoBehaviour
{
    public Camera mainCamera;
    public List<WaypointPath> pathSteps; // All step paths
    public float speed = 5f;
    public float rotationSpeed = 2f;

    public Image fadeOverlay;
    public float fadeDuration = 1f;

    private int currentStep = 0;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private bool isFading = false;

    private const float positionThreshold = 0.01f;
    private const float rotationThreshold = 0.1f;

    void Update()
    {
        if (!isMoving || mainCamera == null || currentStep >= pathSteps.Count)
            return;

        List<Transform> waypoints = pathSteps[currentStep].waypoints;

        if (currentWaypointIndex >= waypoints.Count)
        {
            // Done with current step
            currentStep++;
            currentWaypointIndex = 0;
            isMoving = false;
            Debug.Log($"[BCI] Step {currentStep} completed.");
            StartCoroutine(FadeToWhite(false)); // Fade back after movement
            return;
        }

        Transform target = waypoints[currentWaypointIndex];

        // Move toward current waypoint
        mainCamera.transform.position = Vector3.MoveTowards(
            mainCamera.transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Determine target rotation
        Quaternion targetRotation;

        bool isFinalWaypoint = currentWaypointIndex == waypoints.Count - 1;

        if (isFinalWaypoint)
        {
            // Final waypoint — use its own rotation
            targetRotation = target.rotation;
        }
        else
        {
            // Midway waypoint — look toward the next one
            Vector3 direction = waypoints[currentWaypointIndex + 1].position - mainCamera.transform.position;
            targetRotation = Quaternion.LookRotation(direction.normalized);
        }

        // Smooth rotation
        mainCamera.transform.rotation = Quaternion.RotateTowards(
            mainCamera.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime * 100f
        );

        // Check if we reached the current waypoint
        if (Vector3.Distance(mainCamera.transform.position, target.position) < positionThreshold &&
            Quaternion.Angle(mainCamera.transform.rotation, targetRotation) < rotationThreshold)
        {
            currentWaypointIndex++;
        }
    }

    public void HandleBCISelection(int stepIndex)
    {
        if (stepIndex == currentStep && !isMoving && !isFading)
        {
            Debug.Log($"[BCI] Correct ERP selected. Starting step {stepIndex}.");
            StartCoroutine(FadeAndStartMovement());
        }
        else
        {
            Debug.LogWarning($"[BCI] Incorrect ERP selection or already moving. Expected: {currentStep}, got: {stepIndex}.");
        }
    }

    private IEnumerator FadeAndStartMovement()
    {
        isFading = true;
        yield return StartCoroutine(FadeToWhite(true));
        isMoving = true;
        isFading = false;
    }

    private IEnumerator FadeToWhite(bool fadeIn)
    {
        float timer = 0f;
        Color startColor = fadeOverlay.color;
        Color endColor = fadeOverlay.color;

        startColor.a = fadeIn ? 0f : 1f;
        endColor.a = fadeIn ? 1f : 0f;

        fadeOverlay.color = startColor;

        while (timer < fadeDuration)
        {
            fadeOverlay.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = endColor;
    }

    public int GetCurrentStep() => currentStep;
    public bool IsMoving() => isMoving;
}
