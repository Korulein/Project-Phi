using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{
    public GameObject[] dialogueBoxes; // List of all dialogue boxes
    public Button[] advanceButtons; // List of specific buttons needed to advance if necessary
    public float typeSpeed = 0.03f;
    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    [SerializeField] int loadSceneIndex = 1;

    void Start()
    {
        // Disable all dialogue boxes except the first one
        for (int i = 0; i < dialogueBoxes.Length; i++)
            dialogueBoxes[i].SetActive(i == 0);

        if (dialogueBoxes.Length > 0)
            StartTypewriter(dialogueBoxes[0]);

        SetupAdvanceButton();
    }

    void Update()
    {
        if (!isTyping &&
            currentIndex < dialogueBoxes.Length &&
            (advanceButtons == null || currentIndex >= advanceButtons.Length || advanceButtons[currentIndex] == null) &&
            Input.GetMouseButtonDown(0))
        {
            EnableNextDialogueBox();
        }
    }

    public void EnableNextDialogueBox()
    {
        // If dialogue is still being typed out, disable advancing with click
        if (isTyping)
            return;

        // In order to make sure every button works once, remove all listeners before advancing
        if (advanceButtons != null && currentIndex < advanceButtons.Length && advanceButtons[currentIndex] != null)
        {
            advanceButtons[currentIndex].onClick.RemoveListener(EnableNextDialogueBox);
        }

        if (currentIndex < dialogueBoxes.Length)
            dialogueBoxes[currentIndex].SetActive(false);

        currentIndex++;

        if (currentIndex < dialogueBoxes.Length)
        {
            dialogueBoxes[currentIndex].SetActive(true);
            StartTypewriter(dialogueBoxes[currentIndex]);
            SetupAdvanceButton();
        }

        // If all dialogues are finished, load the next scene
        if (currentIndex >= dialogueBoxes.Length)
        {
            StartGame(); // Replace with your actual scene name
            return;
        }

    }

    private void SetupAdvanceButton()
    {
        if (advanceButtons != null)
        {
            foreach (var btn in advanceButtons)
                if (btn != null) btn.onClick.RemoveAllListeners();
        }

        // Only add the listener to the correct button for this dialogue
        if (advanceButtons != null && currentIndex < advanceButtons.Length && advanceButtons[currentIndex] != null)
        {
            advanceButtons[currentIndex].onClick.AddListener(EnableNextDialogueBox);
        }
    }

    private void StartTypewriter(GameObject dialogueBox)
    {
        var tmp = dialogueBox.GetComponentInChildren<TMP_Text>();
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
    public void StartGame()
    {
        StartCoroutine(StartLoadingScene());
    }

    public IEnumerator StartLoadingScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(loadSceneIndex);
        yield return null;
    }
}
