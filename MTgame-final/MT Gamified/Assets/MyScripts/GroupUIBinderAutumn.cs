/*
A script to trigger the adding of the result icon in the progress bar of the Autumn scene
*/

using UnityEngine;

public class GroupUIBinderAutumn : MonoBehaviour
{
    public FeedbackUIControllerAutumn feedbackUI;

    void Start()
    {
        var groups = Object.FindObjectsByType<ERPGroupControllerAutumn>(FindObjectsSortMode.None);

        foreach (var group in groups)
        {
            group.OnGroupFinished += feedbackUI.AddResultIcon;
        }
    }
}
