/*
A script to trigger the adding of the result icon in the progress bar of the Summer scene
*/

using UnityEngine;

public class GroupUIBinderSummer : MonoBehaviour
{
    public FeedbackUIControllerSummer feedbackUI;

    void Start()
    {
        var groups = Object.FindObjectsByType<ERPGroupControllerSummer>(FindObjectsSortMode.None);

        foreach (var group in groups)
        {
            group.OnGroupFinished += feedbackUI.AddResultIcon;
        }
    }
}
