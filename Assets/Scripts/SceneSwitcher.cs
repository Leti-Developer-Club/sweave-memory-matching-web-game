using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    private Button button;
    public string sceneName;

    void Start()
    {
        // Verify the button's GameObject name
        Debug.Log("Script attached to: " + gameObject.name);

        // Get the Button component and add a listener
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(() => LoadGameScene(sceneName));
        }
    }

    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("This is the " + sceneName);
    }
}


