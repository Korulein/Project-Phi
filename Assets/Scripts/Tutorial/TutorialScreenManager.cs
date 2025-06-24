using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialScreenManager : MonoBehaviour
{
    public GameObject[] tutorialTextBoxes; // List of all dialogue boxes
    public GameObject inputFieldObject; // Input for name under contract
    public int inputFieldDialogueIndex = 1; //Dialogue index where input should appear
    public TMP_Text nameDisplayText; // Text display of name under contract
    public float typeSpeed = 0.03f;

    public GameObject bgObjectToEnable; // Second background, for slideshow
    public GameObject bgObjectToDisable; //First background, for introduction
    public int switchBackGroundDialogue; //Dialogue index on where to switch backgrounds

    private int currentIndex = 0;
    private TMP_InputField inputField;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        foreach (var box in tutorialTextBoxes)
            box.SetActive(false);

        if (tutorialTextBoxes.Length > 0)
            ShowDialogue(currentIndex);

        if (inputFieldObject != null)
        {
            inputField = inputFieldObject.GetComponent<TMP_InputField>();
            inputFieldObject.SetActive(false);
            inputField.onSubmit.RemoveAllListeners();
            inputField.onSubmit.AddListener(OnInputFieldSubmit);
        }

        if (nameDisplayText != null)
            nameDisplayText.gameObject.SetActive(false);
    }

    void Update()
    {
        bool advancePressed = Input.GetMouseButtonDown(0);

        // --- Skip typing animation if in progress ---
        if (advancePressed && isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            // Reveal all text in the current dialogue box
            if (currentIndex < tutorialTextBoxes.Length)
            {
                var tmp = tutorialTextBoxes[currentIndex].GetComponentInChildren<TMP_Text>();
                if (tmp != null)
                    tmp.maxVisibleCharacters = tmp.text.Length;
            }
            isTyping = false;
            return;
        }

        // --- Normal advance logic ---
        if (currentIndex == inputFieldDialogueIndex)
        {
            if (inputFieldObject != null)
                inputFieldObject.SetActive(true);

            // Mouse click advance (only if input is filled)
            if (advancePressed && inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
            {
                AdvanceFromInputField();
            }
        }
        else
        {
            if (advancePressed)
            {
                //Disable the previous dialogue box before advancing
                if (currentIndex < tutorialTextBoxes.Length)
                    tutorialTextBoxes[currentIndex].SetActive(false);

                currentIndex++;

                //Enable the next dialogue box
                if (currentIndex < tutorialTextBoxes.Length)
                {
                    ShowDialogue(currentIndex);

                    if (currentIndex == inputFieldDialogueIndex && inputFieldObject != null)
                        inputFieldObject.SetActive(true);
                }
                // If you reach the end of the tutorial, change scene
                if (currentIndex >= tutorialTextBoxes.Length)
                {
                    // After all dialogue, load the how-to-play 
                    SceneManager.LoadScene("Scene_How_To_Play_01");
                }
            }
        }
    }

    // Called when Enter is pressed in the input field
    private void OnInputFieldSubmit(string value)
    {
        if (isTyping)
            return;

        if (currentIndex == inputFieldDialogueIndex && !string.IsNullOrWhiteSpace(value))
        {
            AdvanceFromInputField();
        }
    }

    private void AdvanceFromInputField()
    {
        tutorialTextBoxes[currentIndex].SetActive(false);
        inputFieldObject.SetActive(false);

        if (nameDisplayText != null)
        {
            nameDisplayText.text = inputField.text;
            nameDisplayText.gameObject.SetActive(true);
        }

        currentIndex++;
        if (currentIndex < tutorialTextBoxes.Length)
        {
            // Replace {insert name} in the next dialogue, if present
            var tmp = tutorialTextBoxes[currentIndex].GetComponentInChildren<TMPro.TMP_Text>();
            if (tmp != null)
            {
                tmp.text = tmp.text.Replace("{insert name}", inputField.text);
            }
            ShowDialogue(currentIndex);
        }
       
    }

    private void ShowDialogue(int index)
    {
        for (int i = 0; i < tutorialTextBoxes.Length; i++)
            tutorialTextBoxes[i].SetActive(i == index);

        // Switch objects at the specified dialogue index
        if (index == switchBackGroundDialogue)
        {
            if (bgObjectToEnable != null)
                bgObjectToEnable.SetActive(true);
            if (bgObjectToDisable != null)
                bgObjectToDisable.SetActive(false);
        }

        var tmp = tutorialTextBoxes[index].GetComponentInChildren<TMPro.TMP_Text>();
        if (tmp != null)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(tmp, tmp.text));
        }
    }

    private IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        isTyping = true;
        textComponent.maxVisibleCharacters = 0;
        int totalChars = fullText.Length;
        for (int i = 0; i <= totalChars; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeSpeed);
        }
        textComponent.maxVisibleCharacters = totalChars;
        isTyping = false;
    }
}
