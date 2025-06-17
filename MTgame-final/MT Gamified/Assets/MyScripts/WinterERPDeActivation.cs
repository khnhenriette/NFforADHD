/*
A script that handles everything that needs to happen after a correct selection in the Winter scene
(or after the third faulty selection in a group)
Activate and deactivate all new and old game objects and progress the game flow within the Winter scene 
*/

using System.Collections;
using UnityEngine;

public class WinterERPDeActivation : MonoBehaviour
{
    public GameObject[] deactivateGroups;  // ERPFlashTag2D groups to turn off
    public GameObject[] activateGroups;    
    
    public GameObject[] laterActivationGroups;// ERPFlashTag2D groups to turn on (after short delay)
    public GameObject[] objectsToMakeVisible;

    private bool hasFired = false;

    public void OnBCISelected()
    {
        if (hasFired) return;
        hasFired = true;

        Debug.Log($"ERP tag selected: {name}");

        // Make objects visible after 2 second delay
        StartCoroutine(MakeObjectsVisibleAfterDelay(2f));


        foreach (var obj in activateGroups)
        {
            if (obj != null)
            {
                StartCoroutine(ActivateAfterDelay(obj, 3f));
            }
        }

        // Activate next ERP tag group with longer delay if necessary 
        foreach (var obj in laterActivationGroups)
        {
            if (obj != null)
            {
                StartCoroutine(ActivateAfterDelay(obj, 5f));
            }
        }

        // Hide old ERP group (though still visible group is already locked through selection logic)
        foreach (var obj in deactivateGroups)
        {
            if (obj != null)
            {
                StartCoroutine(DeactivateAfterDelay(obj, 5.1f));
            }
        }
    }

    private IEnumerator MakeObjectsVisibleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var obj in objectsToMakeVisible)
        {
            if (obj == null) continue;
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
                renderer.enabled = true;
        }

        Debug.Log("Made objects visible after delay.");
    }

    private IEnumerator ActivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
        Debug.Log($"Activated group after {delay} secs delay: {obj.name}");
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        Debug.Log($"Deactivated group after {delay} secs delay: {obj.name}");
    }
}
