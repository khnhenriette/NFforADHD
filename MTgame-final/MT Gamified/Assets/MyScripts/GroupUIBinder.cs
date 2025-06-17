/*
A script to trigger the adding of the result icon in the progress bar of the Winter scene
*/

using UnityEngine;

public class GroupUIBinder : MonoBehaviour
{
    public FeedbackUIController feedbackUI;

    void Start()
    {
        var groups = Object.FindObjectsByType<ERPGroupController>(FindObjectsSortMode.None);

        foreach (var group in groups)
        {
            group.OnGroupFinished += feedbackUI.AddResultIcon;
        }
    }
}
