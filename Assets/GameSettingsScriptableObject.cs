using TMPro;
using UnityEngine;

[CreateAssetMenu(
    fileName = "GameSettingsScriptableObject",
    menuName = "ScriptableObjects/GameSettingsScriptableObject"
)]
public class GameSettingsScriptableObject : ScriptableObject
{
    public int rows;
    public int cols;
    public float revealTime;

    public void ChooseDifficulty(TMP_Text difficultyText)
    {
        if (difficultyText != null)
        {
            string buttonText = difficultyText.text.Trim().ToString();
            Debug.Log("The card game's difficulty is set to: " + buttonText);

            switch (buttonText)
            {
                case "Easy":
                    rows = 2;
                    cols = 4;
                    revealTime = 4.0f;
                    break;
                case "Medium":
                    rows = 3;
                    cols = 6;
                    revealTime = 3.5f;
                    break;
                case "Hard":
                    rows = 4;
                    cols = 8;
                    revealTime = 3.0f;
                    break;
                default:
                    Debug.LogError("Unexpected button text: " + buttonText);
                    break;
            }
        }
    }
}
