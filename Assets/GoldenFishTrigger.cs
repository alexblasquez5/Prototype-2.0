using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldenFishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the specified scene
            SceneManager.LoadSceneAsync(2);
        }
    }
}
