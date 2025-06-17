/*
A script that takes care of the starting configuration of the game 
*/

using UnityEngine;

public class InitialVisibilityController : MonoBehaviour
{
    [Header("Parents or Individual Objects to Hide Visually On Start")]
    public GameObject[] objectsToHide;

    void Start()
    {
        foreach (var obj in objectsToHide)
        {
            if (obj == null) continue;

            // Get all Renderer components in the object and its children
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);

            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
    }
}
