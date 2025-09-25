using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenuNavigation : MonoBehaviour
{
    [SerializeField] private Selectable[] menuItems; // Assign buttons and sliders in order
    private int currentIndex = 0;

    private void Start()
    {
        // Select the first menu item on start
        if (menuItems.Length > 0)
        {
            SelectMenuItem(currentIndex);
        }
    }

    private void Update()
    {
        if (menuItems.Length == 0) return;

        // Navigate up (previous item)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1);
        }
        // Navigate down (next item)
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);
        }

        // Handle slider adjustments
        if (menuItems[currentIndex] is Slider slider)
        {
            AdjustSlider(slider);
        }
        // Handle button activation
        else if (Input.GetKeyDown(KeyCode.RightArrow) && menuItems[currentIndex] is Button button)
        {
            button.onClick.Invoke();
        }
    }

    private void Navigate(int direction)
    {
        // Deselect the current item
        EventSystem.current.SetSelectedGameObject(null);

        // Update index, wrapping around if necessary
        currentIndex = (currentIndex + direction + menuItems.Length) % menuItems.Length;

        // Select the new item
        SelectMenuItem(currentIndex);
    }

    private void SelectMenuItem(int index)
    {
        // Highlight the item in the EventSystem
        EventSystem.current.SetSelectedGameObject(menuItems[index].gameObject);
    }

    private void AdjustSlider(Slider slider)
    {
        float adjustment = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            adjustment = slider.maxValue / 20f; // Increment the slider value
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            adjustment = -slider.maxValue / 20f; // Decrement the slider value
        }

        if (adjustment != 0)
        {
            slider.value = Mathf.Clamp(slider.value + adjustment, slider.minValue, slider.maxValue);
        }
    }
}
