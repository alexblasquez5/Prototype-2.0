using System.Collections;
using UnityEngine;
using TMPro;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam; // Assign the virtual camera
    public TextMeshProUGUI roomText; // Assign the text UI element
    public string displayText; // Text to show when entering the camera
    public float displayDuration = 2f; // Duration to show the text

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true); // Activate the virtual camera
            if (roomText != null) // Ensure the text object is assigned
            {
                StartCoroutine(ShowText());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false); // Deactivate the virtual camera
        }
    }

    private IEnumerator ShowText()
    {
        roomText.text = displayText; // Set the text
        roomText.CrossFadeAlpha(1, 0.5f, false); // Fade in over 0.5 seconds
        yield return new WaitForSeconds(displayDuration); // Wait for the display duration
        roomText.CrossFadeAlpha(0, 0.5f, false); // Fade out over 0.5 seconds
    }
}
