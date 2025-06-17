/*
A script to control what is added in the progress bar for the Winter scene: 
positive icon snowflake for sucess
red X for 3 times faulty selection 
*/

using UnityEngine;
using System;

public class FeedbackUIController : MonoBehaviour
{
    public GameObject snowflakePrefab;
    public GameObject xPrefab;
    public Transform container; // where icons are added, i.e. progress bar 

    // public variables to be used for failure threshold handling 
    public int TotalExpectedGroups = 8; 
    public int TotalResults => container.childCount;

    // Expose new event
    public event Action<bool> OnResultAdded;

    public void AddResultIcon(bool isSuccess)
    {
        GameObject icon = Instantiate(isSuccess ? snowflakePrefab : xPrefab, container);
        OnResultAdded?.Invoke(isSuccess);
    }
}
