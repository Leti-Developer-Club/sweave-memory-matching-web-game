using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    private Button playButton;
    public string sceneName;

    void Start()
    {
        // Verify the button's GameObject name
        Debug.Log("Script attached to: " + gameObject.name);

        // Get the Button component and add a listener
        playButton = GetComponent<Button>();

        if (playButton != null)
        {
            playButton.onClick.AddListener(() => LoadGameScene(sceneName));
        }
    }

    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("This is the " + sceneName);
    }
}


