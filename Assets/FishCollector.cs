using UnityEngine;
using TMPro; // For updating the UI counter

public class FishCollector : MonoBehaviour
{
    public TextMeshProUGUI fishCounterText; // Reference to the UI text
    private int fishCount = 0; // Keeps track of the collected fish

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            // Increment the fish counter
            fishCount++;

            // Update the UI
            fishCounterText.text = "Fish: " + fishCount;

            // Destroy the collected fish
            Destroy(other.gameObject);
        }
    }
}
