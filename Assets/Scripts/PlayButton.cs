using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private List<TMP_Text> difficultyTexts = new List<TMP_Text>();

    public GameSettingsScriptableObject gameSettings;

    void Start()
    {
        // Initialize the difficulty text objects once at the start
        CreateDifficultyTexts();

        if (gameObject.GetComponent<Button>() == null)
        {
            Debug.LogError("Button component missing on PlayButton");
            return;
        }

        gameObject
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                Debug.Log("Play Button Clicked");
                SetRandomDifficulty();
                SceneManager.LoadScene("GameScene");
            });
    }

    void CreateDifficultyTexts()
    {
        // Create parent object to hold our text objects (won't be visible)
        GameObject textParent = new GameObject("DifficultyTextObjects");
        textParent.transform.SetParent(transform);
        textParent.SetActive(false); // Make it inactive so it's not rendered

        // Create Easy text
        GameObject easyObj = new GameObject("EasyText");
        easyObj.transform.SetParent(textParent.transform);
        TMP_Text easyText = easyObj.AddComponent<TextMeshProUGUI>();
        easyText.text = "Easy";
        difficultyTexts.Add(easyText);

        // Create Medium text
        GameObject mediumObj = new GameObject("MediumText");
        mediumObj.transform.SetParent(textParent.transform);
        TMP_Text mediumText = mediumObj.AddComponent<TextMeshProUGUI>();
        mediumText.text = "Medium";
        difficultyTexts.Add(mediumText);

        // Create Hard text
        GameObject hardObj = new GameObject("HardText");
        hardObj.transform.SetParent(textParent.transform);
        TMP_Text hardText = hardObj.AddComponent<TextMeshProUGUI>();
        hardText.text = "Hard";
        difficultyTexts.Add(hardText);
    }

    void SetRandomDifficulty()
    {
        // Choose a random difficulty
        int randomNum = Random.Range(0, difficultyTexts.Count);

        Debug.Log("Passing difficulty: " + difficultyTexts[randomNum].text);
        Debug.Log(
            $"Rows:{gameSettings.rows}, Cols:{gameSettings.cols}, RevealTime: {gameSettings.revealTime}"
        );

        // Pass the randomly selected difficulty to GameManager
        gameSettings.ChooseDifficulty(difficultyTexts[randomNum]);
    }
}
