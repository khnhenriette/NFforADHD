/*
A script that calls the BCI sequence controller from the respctive game object
Provides the needed step index (i.e. where in the forest we currently are) so that the correct 
transition through the environment can be triggered 
*/

using UnityEngine;

public class BCIStepTrigger : MonoBehaviour
{
    public int stepIndex;
    public BCISequenceController controller;

    public void OnBCISelected()
    {
        controller.HandleBCISelection(stepIndex);
    }
}
