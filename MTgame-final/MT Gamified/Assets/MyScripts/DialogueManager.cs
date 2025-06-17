/*
This is a script to control the text scenes in between the scenes that are controlled via the BCI
Some information about the task is provided, the user meets some in-game characters
User control of the text via spacebar 
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)] public string line;
    }

    public DialogueLine[] dialogueLines;

    public GameObject dialogueBox;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public Image fadePanel;
    public float typeSpeed = 0.03f;
    public float delayBeforeFade = 1f;
    public float fadeDuration = 1.5f;
    //public string nextSceneName;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Lists of objects to manage 
    [Header("Scene Changes After Dialogue")]
    public GameObject[] objectsToDeactivate;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToMakeVisible;

    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLineIndex].line;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        dialogueBox.SetActive(true);
        isDialogueActive = true;
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            DialogueLine line = dialogueLines[currentLineIndex];
            speakerText.text = line.speaker;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(line.line));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        StartCoroutine(HandleSceneTransition());
    }

    IEnumerator HandleSceneTransition()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float t = 0f;
        Color fadeColor = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Apply object changes
        foreach (var obj in objectsToActivate)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objectsToMakeVisible)
        {
            if (obj == null) continue;
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
                renderer.enabled = true;
        }

        yield return new WaitForEndOfFrame(); // Wait to safely deactivate objects

        foreach (var obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(false);

    }
}
