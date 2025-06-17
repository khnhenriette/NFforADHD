/*
The selection logging of the individual game objects 
Logs only once per ongoing selection (not every frame) & ensures there are no buggy selections 
*/

using Unity.VisualScripting;
using UnityEngine;

public class ERPSelectionLogger : MonoBehaviour
{
    private ERPGroupController groupController;
    private bool hasLoggedSelection = false; // Variable to prevent logging of the same ongoing selection
    private float lastLogTime = -Mathf.Infinity; // Time of last log attempt

    private const float minLogInterval = 1.0f; // 1 second interval

    void Awake()
    {
        // Find group controller in parents
        groupController = GetComponentInParent<ERPGroupController>();
    }

    public void LogSelection()
    {
        if (hasLoggedSelection)
            return; // Ignore selection repetitions that are due to ongoing selection over several frames
        
        
        // Prevent logging if called too soon after previous selection 
        if (Time.time - lastLogTime < minLogInterval)
            return;

        // store logging time     
        lastLogTime = Time.time;
        
        
        string fullName = GetFullName();

        // ignore selections that come from inactive or locked game objects and issue warning 
        if (!gameObject.activeInHierarchy || groupController == null || groupController.isGroupLocked)
        {
            Debug.LogWarning($"{fullName} tried to log while inactive or after correct selection!");
            return;
        }


        hasLoggedSelection = true;

        ERPGameLogger.Instance?.Log(fullName);

        // Pass selection result to group
        groupController.HandleSelection(isCorrectTag);
    }

    [SerializeField]
    private bool isCorrectTag = false;

    // Deselection resets logging ability
    public void ResetLogger()
    {
        hasLoggedSelection = false;
    }

    private string GetFullName()
    {
        Transform t = transform;
        string[] nameParts = new string[3];

        for (int i = 2; i >= 0; i--)
        {
            if (t != null)
            {
                nameParts[i] = t.name;
                t = t.parent;
            }
            else
            {
                nameParts[i] = "Unknown";
            }
        }

        return string.Join(" ", nameParts);
    }
}
