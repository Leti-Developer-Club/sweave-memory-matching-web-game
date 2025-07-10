using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameSettingsScriptableObject gameSettings;
    public GameObject chooseDifficultyButton;
    public GameObject easyButton;
    public GameObject mediumButton;
    public GameObject hardButton;
    public GameObject closeButton;
    public GameObject difficultyModalCanvas;
    public GameObject settingsCanvas;

    void Start()
    {
        HideCanvas();

        Button thisEasyButton = easyButton.GetComponent<Button>();
        Button thisMediumButton = mediumButton.GetComponent<Button>();
        Button thisHardButton = hardButton.GetComponent<Button>();
        Button thisChooseDifficultyButton = chooseDifficultyButton.GetComponent<Button>();
        Button thisCloseButton = closeButton.GetComponent<Button>();

        onDifficultyButtonClick(thisChooseDifficultyButton, difficultyModalCanvas, settingsCanvas);

        if (thisEasyButton != null && thisMediumButton != null && thisHardButton != null)
        {
            thisEasyButton.onClick.AddListener(() =>
            {
                gameSettings.ChooseDifficulty(thisEasyButton.GetComponentInChildren<TMP_Text>());
                SceneManager.LoadScene("GameScene");
            });
            thisMediumButton.onClick.AddListener(() =>
            {
                gameSettings.ChooseDifficulty(thisMediumButton.GetComponentInChildren<TMP_Text>());
                SceneManager.LoadScene("GameScene");
            });
            thisHardButton.onClick.AddListener(() =>
            {
                gameSettings.ChooseDifficulty(thisHardButton.GetComponentInChildren<TMP_Text>());
                SceneManager.LoadScene("GameScene");
            });
        }

        if (thisCloseButton != null)
        {
            thisCloseButton.onClick.AddListener(() =>
            {
                Debug.Log("Close button clicked...");

                //hide the Difficulty options modal and show the settings scene
                HideCanvas();

                SceneManager.LoadScene("WelcomeScene");
            });
        }
        else
        {
            Debug.LogError("Close button is not assigned");
        }
    }

    public void HideCanvas()
    {
        if (difficultyModalCanvas != null)
        {
            difficultyModalCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Difficulty Modal Canvas is not assigned!");
        }
    }

    public void ShowCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Difficulty Modal Canvas is not assigned!");
        }
    }

    public void onDifficultyButtonClick(
        Button button,
        GameObject thisCanvas,
        GameObject thisSettingsCanvas
    )
    {
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                ShowCanvas(thisCanvas);
                thisSettingsCanvas.SetActive(false);
            });
        }
        else
        {
            Debug.LogError("Difficulty Button is not assigned!");
        }
    }
}
