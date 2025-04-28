using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmBuild : MonoBehaviour
{
    public Button confirmButton; 
    public string targetSortingLayer = "Component";
    public string lockedSortingLayer = "Locked";
    public GameObject successPopup;
    public GameObject warningPopup; 

    void Start()
    {
        // Instantiate the finish build button
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }
    }

    void OnConfirmButtonClicked()
    {
        // Check if any sprites with the target sorting layer still exist
        if (AreSpritesWithTargetLayerPresent())
        {
            ShowWarningPopup(); 
            return; 
        }

        // If no sprites with the target sorting layer exist, display the success popup
        ShowBuildSuccessPopup();
    }

    bool AreSpritesWithTargetLayerPresent()
    {
        SpriteRenderer[] sprites = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

        foreach (var sprite in sprites)
        {
            if (sprite.sortingLayerName == targetSortingLayer)
            {
                return true; 
            }
        }

        // No sprites with the target sorting layer found
        return false; 
    }

    void ShowBuildSuccessPopup()
    {
        // Instantiate the success popup prefab and display it
        if (successPopup != null)
        {
            Instantiate(successPopup, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Success popup prefab is not assigned!");
        }
    }

    void ShowWarningPopup()
    {
        // Instantiate the warning popup prefab and display it
        if (warningPopup != null)
        {
            GameObject newWarning = Instantiate(warningPopup, Vector3.zero, Quaternion.identity);

            // Add a close button functionality
            Button closeButton = newWarning.GetComponentInChildren<Button>();
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => Destroy(newWarning));
            }
        }
        else
        {
            Debug.LogWarning("Warning popup prefab is not assigned!");
        }
    }
}
