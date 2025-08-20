// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameSettingsScriptableObject gameSettings;
    public static GameManager Instance { get; private set; }

    public GameObject gameCanvas;

    public GameObject pauseCanvas;

    public Button pauseButton;

    public Button resumeButton;

    public Button closeButton;

    public Button settingsButton;

    public Button pauseQuitButton;

    public int rows;
    public int cols;

    public int gridSize;

    public float scalingFactor = 2.0f;

    public List<Sprite> frontSprites = new List<Sprite>();
    public float revealTime;
    public GameObject memoryCard;

    public ICard firstRevealed;
    public ICard secondRevealed;
    public bool canReveal = true;

    private int matchedPairs = 0;
    private int totalPairs;

    public GameObject WinScreenCanvas;

    public GameObject restartButton;
    public GameObject gameDifficultyButton;

    public GameObject quitButton;

    public Camera gameCamera;

    GameObject cardGridPanel;
    RectTransform panelRectTransform;

    // public GameObject Spawner;

    public GameObject ScoreCounterText;

    private TextMeshProUGUI moveTextGO;

    private int moves = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("Pause screen canvas not found");
        }
    }

    void Start()
    {
        // Reset game state
        matchedPairs = 0;
        totalPairs = 0;
        firstRevealed = null;
        secondRevealed = null;
        canReveal = true;

        if (gameCamera == null)
        {
            gameCamera = Camera.main;
            Debug.Log("GameScene camera assigned");
        }
        else
        {
            Debug.Log("GameScene camera not assigned");
        }

        gameCanvas = GameObject.FindWithTag("GameCanvas");

        // find the score counter text game object
        ScoreCounterText = GameObject.Find("ScoreCounterText");
        if (ScoreCounterText != null)
        {
            // get the text component
            moveTextGO = ScoreCounterText.GetComponent<TextMeshProUGUI>();

            //log the actual text
            Debug.Log("ScoreCounterText text: " + moveTextGO.text);
        }
        else
        {
            Debug.Log("ScoreCounterText game object not found");
        }

        // Find the Card Grid Panel in the current scene (make sure it has the proper tag)
        cardGridPanel = GameObject.FindWithTag("CardGridPanel");
        if (cardGridPanel == null)
        {
            Debug.LogError("Panel with tag 'CardGridPanel' not found in the current scene.");
            return;
        }

        // Get the Panel's RectTransform so we know its actual dimensions.
        panelRectTransform = cardGridPanel.GetComponent<RectTransform>();
        if (panelRectTransform == null)
        {
            Debug.LogError("Panel does not have a RectTransform component.");
            return;
        }

        // load sprites
        frontSprites = LoadSprites();

        // access the game settings scriptable object
        if (gameSettings != null)
        {
            Debug.Log("Game settings loaded successfully!");
            Debug.Log(
                $"Rows:{gameSettings.rows}, Cols:{gameSettings.cols}, RevealTime: {gameSettings.revealTime}"
            );
            rows = gameSettings.rows;
            cols = gameSettings.cols;
            Debug.Log("Grid is " + rows + " by " + cols);
            revealTime = gameSettings.revealTime;
            CreateCardGrid(rows, cols, memoryCard, frontSprites);
            PauseGame();
            ResumeGame();
            QuitGame();
        }
        else
        {
            Debug.LogError("Game settings not found!");
        }

        // Find the WinScreenCanvas Panel in the GameScene
        WinScreenCanvas = GameObject.FindWithTag("WinScreenCanvas");

        if (WinScreenCanvas != null)
        {
            restartButton = GameObject.Find("RestartButton");
            gameDifficultyButton = GameObject.Find("GameDifficultyButton");
            quitButton = GameObject.Find("QuitButton");

            if (restartButton == null && gameDifficultyButton == null && quitButton == null)
            {
                Debug.LogError(
                    "Restart, gameDifficulty and quit buttons not found in the GameScene!"
                );
            }

            // Ensure the WinScreenCanvas is disabled initially
            WinScreenCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("WinScreenCanvas UI element not found in the GameScene!");
        }
    }

    // when the pause button is clicked, the pause screen should be enabled
    void PauseGame()
    {
        pauseButton.onClick.AddListener(() =>
        {
            Debug.Log("Pause Button Clicked");
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(true);
        });
    }

    void ResumeGame()
    {
        resumeButton.onClick.AddListener(() =>
        {
            Debug.Log("Resume Button Clicked");
            pauseCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        });

        closeButton.onClick.AddListener(() =>
        {
            Debug.Log("Close Button Clicked");
            pauseCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        });
    }

    void QuitGame()
    {
        pauseQuitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit Button Clicked");
            pauseCanvas.SetActive(false);
            SceneManager.LoadScene("WelcomeScene");
        });
    }

    // get all the front sprite assets. Each sprite can only be assigned to two cards(a pair of cards) in the card grid
    List<Sprite> LoadSprites()
    {
        // load sprites from the folder containing the sprites and add them to the frontSprites list
        frontSprites = Resources.LoadAll<Sprite>("Sprites/FrontSprites/").ToList();
        if (frontSprites == null || frontSprites.Count < 0)
        {
            Debug.LogError($"No sprites found at path the specified path");
        }

        Debug.Log("No of sprites: " + frontSprites.Count);

        foreach (var sprite in frontSprites)
        {
            Debug.Log($"Loaded sprite: {sprite.name}");
        }

        return frontSprites;
    }

    public void HideCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
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

    private int[] ShuffleCards(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int temp = newArray[i];
            newArray[i] = newArray[randomIndex];
            newArray[randomIndex] = temp;
        }
        Debug.Log("Shuffled array: " + string.Join(", ", newArray));
        return newArray;
    }

    public int[] CreatePairedNumbersArray(int rows, int cols)
    {
        int totalCards = rows * cols;
        int pairCount;
        int[] numbers;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total cards must be even to form pairs!");
            return null;
        }
        else
        {
            pairCount = totalCards / 2;
            numbers = new int[totalCards];
        }

        for (int i = 0; i < pairCount; i++)
        {
            numbers[i * 2] = i;
            numbers[i * 2 + 1] = i;
        }

        // Ensure to use the shuffled result!
        numbers = ShuffleCards(numbers);
        return numbers;
    }

    //     to implement a move counter
    // what is a move?
    // - a move is a pair of cards that have been clicked
    // - have a move counter variable, when a pair of cards are clicked, increase it by one
    public void ShowScore()
    {
        Debug.Log($"This is the current score: {moves}");
        // if score is less than 1, the text should read Move but Moves otherwise
        string movesWord = moves <= 1 ? "Move" : "Moves";
        // how do i access the values of a TextMeshPro component
        moveTextGO.text = $"{movesWord}: {moves}";
    }

    // This method is called by a card when it's clicked. Works with both Card and CardUI
    public void CardRevealed(ICard card)
    {
        MonoBehaviour cardMB = card as MonoBehaviour;
        Debug.Log($"Card revealed: {cardMB?.name} with ID: {card.id}");
        if (!card.IsRevealed)
            return;

        if (firstRevealed == null)
        {
            firstRevealed = card;
            Debug.Log($"First card revealed: {cardMB?.name} with ID: {firstRevealed.id}");
        }
        else if (secondRevealed == null && firstRevealed != card)
        {
            secondRevealed = card;

            // increment the move count after a pair of cards have been clicked on or revealed
            moves++;

            // then show the score
            ShowScore();

            canReveal = false;
            Debug.Log($"Second card revealed: {cardMB?.name} with ID: {secondRevealed.id}");
            StartCoroutine(MatchCards(firstRevealed, secondRevealed));
        }
    }

    //     How do i use the MatchCards method, where will I be referencing the firstCard and secondCard from?

    public IEnumerator MatchCards(ICard firstCard, ICard secondCard)
    {
        if (firstCard.id == secondCard.id)
        {
            Debug.Log("Cards match!");

            // let those cards show their front sprites till the end of the game
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;

            // let those cards show their front sprites till the end of the game
            firstCard.ShowFrontSprite();
            secondCard.ShowFrontSprite();

            matchedPairs++;
            if (matchedPairs == totalPairs)
            {
                Debug.Log("All cards matched! Player wins!");

                // Hide the Game Canvas
                if (gameCanvas == null)
                {
                    Debug.Log("GameCanvas can't be found");
                }
                else
                {
                    gameCanvas.SetActive(false);
                }

                // Trigger win sequence here (e.g., show win screen, play sound, etc.)
                WinGame();
            }
        }
        else
        {
            Debug.Log("Cards don't match!");
            // Wait a bit so the player can see the cards.
            yield return new WaitForSeconds(1.0f);
            firstCard.HideFront();
            secondCard.HideFront();
        }
        // Reset for the next pair.
        firstRevealed = null;
        secondRevealed = null;
        canReveal = true;
    }

    private void WinGame()
    {
        // Log the win event
        Debug.Log("Congratulations! You've won the game!");

        // Show a win screen
        ShowWinScreenCanvas();

        // Play a win sound
        // PlayWinSound();

        // Disable further input
        // DisablePlayerInput();

        // Optionally, show statistics or offer to restart the game
        // ShowGameStatistics();
    }

    public void ShowWinScreenCanvas()
    {
        if (WinScreenCanvas != null)
        {
            WinScreenCanvas.SetActive(true);

            // Find buttons again when win screen is shown, in case scene loading changed references
            restartButton = GameObject.Find("RestartButton");
            gameDifficultyButton = GameObject.Find("GameDifficultyButton");
            quitButton = GameObject.Find("QuitButton");

            if (restartButton != null)
            {
                // Remove existing listeners first to prevent duplicates
                restartButton.GetComponent<Button>().onClick.RemoveAllListeners();
                restartButton
                    .GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        Debug.Log("Restarting the game...");
                        WinScreenCanvas.SetActive(false);
                        SceneManager.LoadScene("GameScene");
                    });
            }

            if (gameDifficultyButton != null)
            {
                gameDifficultyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                gameDifficultyButton
                    .GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        Debug.Log("Choose a difficulty setting...");
                        WinScreenCanvas.SetActive(false);
                        SceneManager.LoadScene("SettingsScene");
                    });
            }

            if (quitButton != null)
            {
                quitButton.GetComponent<Button>().onClick.RemoveAllListeners();
                quitButton
                    .GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        Debug.Log("Quitting game...");
                        SceneManager.LoadScene("WelcomeScene");
                    });
            }
        }
        else
        {
            Debug.LogError("WinScreenCanvas UI element not found!");
        }
    }

    private float availableWidth;
    private float availableHeight;
    private Vector2 cellSize;

    RectTransform CGRectTransform;
    int[] shuffledNumbers;

    public void CreateCardGrid(int rows, int cols, GameObject cardPrefab, List<Sprite> frontSprites)
    {

        // Get layout and set grid size
        FlexibleGridLayoutGroup layoutGroup = cardGridPanel.GetComponent<FlexibleGridLayoutGroup>();
        layoutGroup.SetGridSize(rows, cols);

        // Remove old cards
        foreach (Transform child in cardGridPanel.transform)
            Destroy(child.gameObject);

        // Create shuffled pairs
        shuffledNumbers = CreatePairedNumbersArray(rows, cols);
        totalPairs = shuffledNumbers.Length / 2;

        // Instantiate cards
        for (int i = 0; i < shuffledNumbers.Length; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardGridPanel.transform);

            int id = shuffledNumbers[i];

            // Try Card first (legacy sprite-based)
            Card cardScript = newCard.GetComponent<Card>();
            if (cardScript != null)
            {
                cardScript.id = id;

                if (id < frontSprites.Count)
                {
                    // Card uses Vector2 parameter for scaling
                    cardScript.SetFrontSprite(Vector2.zero, frontSprites[id]);
                    cardScript.SetBackSprite(Vector2.zero);
                }
                else
                {
                    Debug.LogError($"Invalid sprite ID {id}");
                }
            }
            else
            {
                // Try CardUI (UI-based)
                CardUI cardUIScript = newCard.GetComponent<CardUI>();
                if (cardUIScript != null)
                {
                    cardUIScript.id = id;

                    if (id < frontSprites.Count)
                    {
                        // CardUI only takes Sprite parameter
                        cardUIScript.SetFrontSprite(frontSprites[id]);
                        // You'll need to assign back sprite here
                        // cardUIScript.SetBackSprite(backSprite);
                    }
                    else
                    {
                        Debug.LogError($"Invalid sprite ID {id}");
                    }
                }
                else
                {
                    Debug.LogError("Card prefab must have either Card or CardUI component!");
                }
            }
        }
    }
}
