using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderButtonManager : MonoBehaviour
{
    public List<Button> orderButtons;
    public Color activeColor = new Color(0.3608f, 0.4941f, 0.6196f);
    public Color inactiveColor = new Color(0.1686f, 0.2392f, 0.3215f);

    private Button currentActive;

    private void Start()
    {
        foreach (Button button in orderButtons)
        {
            ApplyColor(button, inactiveColor);
        }

        if (orderButtons.Count > 0)
        {
            SetActive(orderButtons[0]);
        }
    }

    public void SetActive(Button clickedButton)
    {
        if (currentActive != null)
        {
            ApplyColor(currentActive, inactiveColor);
        }

        currentActive = clickedButton;
        ApplyColor(currentActive, activeColor);
    }

    private void ApplyColor(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        colors.selectedColor = color;
        colors.disabledColor = color;
        colors.colorMultiplier = 1f;
        button.colors = colors;
    }
}