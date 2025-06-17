using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialScreenManager : MonoBehaviour
{
    public GameObject[] tutorialTextBoxes; // Assign your text box GameObjects in the Inspector
    public GameObject inputFieldObject;    // Assign your TMP_InputField GameObject in the Inspector
    public int inputFieldDialogueIndex = 1; // The index at which the input field should appear (set in Inspector)
    public TMP_Text nameDisplayText;       // Assign your TextMeshPro text field in the Inspector
    public float typeSpeed = 0.03f;        // Time between letters

    public GameObject bgObjectToEnable;   // Background object to enable
    public GameObject bgObjectToDisable;  // Background object to disable
    public int switchBackGroundDialogue; // The dialogue index to switch objects at

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
        bool advancePressed = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);

        if (isTyping)
            return;

        if (currentIndex == inputFieldDialogueIndex)
        {
            if (inputFieldObject != null)
                inputFieldObject.SetActive(true);

            // Mouse click or spacebar advance (only if input is filled)
            if (advancePressed && inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
            {
                AdvanceFromInputField();
            }
        }
        else
        {
            if (advancePressed)
            {
                if (currentIndex < tutorialTextBoxes.Length)
                    tutorialTextBoxes[currentIndex].SetActive(false);

                currentIndex++;

                if (currentIndex < tutorialTextBoxes.Length)
                {
                    ShowDialogue(currentIndex);

                    if (currentIndex == inputFieldDialogueIndex && inputFieldObject != null)
                        inputFieldObject.SetActive(true);
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
