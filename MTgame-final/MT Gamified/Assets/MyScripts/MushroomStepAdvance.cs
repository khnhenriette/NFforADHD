/*
A script that handles everything that needs to happen after a correct selection in the Autumn scene
(or after the third faulty selection in a group)
Activate and deactivate all new and old game objects and progress the game flow within the Autumn scene 
*/

using System.Collections;
using UnityEngine;

public class MushroomStepAdvance : MonoBehaviour
{
    public GameObject[] deactivateGroups;  // ERPFlashTag2D groups to turn off
    public GameObject[] activateGroups;    // ERPFlashTag2D groups to turn on (after short delay)
    public GameObject[] hideSprites;       // Sprites to hide
    public GameObject[] showSprites;       // Sprites to show

    private bool hasFired = false;

    public void OnBCISelected()
    {
        if (hasFired) return;
        hasFired = true;

        Debug.Log($"ERP tag selected: {name}");

    
        // Hide old sprite
        foreach (var obj in hideSprites)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Show new sprite
        foreach (var obj in showSprites)
        {
            if (obj != null)
            {
                StartCoroutine(ShowAfterDelay(obj, 0.5f));
            }
        }

        // Activate next ERP tag group with delay
        foreach (var obj in activateGroups)
        {
            if (obj != null)
            {
                StartCoroutine(ActivateAfterDelay(obj, 3.5f)); // some delay for activation after correct selection 
            }
        }

        // Hide old ERP group (though still visible group is already locked through selection logic)
        foreach (var obj in deactivateGroups)
        {
            if (obj != null)
            {
                StartCoroutine(DeactivateAfterDelay(obj, 3.6f)); // some longer delay for deactivation after correct selection
            }
        }
    }

    private IEnumerator ShowAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
        Debug.Log($"Show sprite after delay: {obj.name}");
    }

    private IEnumerator ActivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
        Debug.Log($"Activated group after delay: {obj.name}");
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        Debug.Log($"Deactivated group after delay: {obj.name}");
    }
}
