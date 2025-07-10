using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyLevel : MonoBehaviour
{
    public GameSettingsScriptableObject gameSettings;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    Button thisEasyButton;
    Button thisMediumButton;
    Button thisHardButton;

    void Awake()
    {
        thisEasyButton = easyButton;
        thisMediumButton = mediumButton;
        thisHardButton = hardButton;
    }

    void Start()
    {
        if (thisEasyButton != null && thisMediumButton != null && thisHardButton != null)
        {
            Debug.Log("Button script is running");
            thisEasyButton.onClick.AddListener(() =>
            {
                // gameSettings.ChooseDifficulty(thisEasyButton.GetComponentInChildren<TMP_Text>());
                // //load game scene
                // SceneManager.LoadScene("GameScene");
                SelectDifficulty(thisEasyButton.GetComponentInChildren<TMP_Text>());
            });
            thisMediumButton.onClick.AddListener(() =>
            {
                SelectDifficulty(thisMediumButton.GetComponentInChildren<TMP_Text>());
            });
            thisHardButton.onClick.AddListener(() =>
            {
                SelectDifficulty(thisHardButton.GetComponentInChildren<TMP_Text>());
            });
        }
    }

    void SelectDifficulty(TMP_Text difficulty)
    {
        // based on the button clicked set respective difficulty level
        gameSettings.ChooseDifficulty(difficulty);
        // log difficulty level set
        Debug.Log("Difficulty level chosen is: " + difficulty.text);
        // //load game scene
        SceneManager.LoadScene("GameScene");
    }
}
