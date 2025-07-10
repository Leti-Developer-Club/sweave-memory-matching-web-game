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

    public int rows;
    public int cols;

    public int gridSize;

    public float scalingFactor = 2.0f;

    public List<Sprite> frontSprites = new List<Sprite>();
    public float revealTime;
    public GameObject memoryCard;

    public Card firstRevealed;
    public Card secondRevealed;
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

    public GameObject Spawner;

    public GameObject cardGridText;

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
            int randomIndex = Random.Range(0, i + 1);
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
        int pairCount = totalCards / 2;
        int[] numbers = new int[totalCards];

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


    // This method is called by a card when it's clicked.
    public void CardRevealed(Card card)
    {
        Debug.Log($"Card revealed: {card.name} with ID: {card.id}");
        if (!card.IsRevealed)
            return;

        if (firstRevealed == null)
        {
            firstRevealed = card;
            Debug.Log($"First card revealed: {firstRevealed.name} with ID: {firstRevealed.id}");
        }
        else if (secondRevealed == null && firstRevealed != card)
        {
            secondRevealed = card;

            // increment the move count after a pair of cards have been clicked on or revealed
            moves++;

            // then show the score
            ShowScore();

            canReveal = false;
            Debug.Log($"Second card revealed: {secondRevealed.name} with ID: {secondRevealed.id}");
            StartCoroutine(MatchCards(firstRevealed, secondRevealed));
        }
    }

    //     How do i use the MatchCards method, where will I be referencing the firstCard and secondCard from?

    public IEnumerator MatchCards(Card firstCard, Card secondCard)
    {
        if (firstCard.id == secondCard.id)
        {
            Debug.Log("Cards match!");

            // let those cards show their front sprites till the end of the game
            firstCard.GetComponent<Card>().IsMatched = true;
            secondCard.GetComponent<Card>().IsMatched = true;

            // let those cards show their front sprites till the end of the game
            firstCard.GetComponent<Card>().ShowFrontSprite();
            secondCard.GetComponent<Card>().ShowFrontSprite();

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
            firstCard.GetComponent<Card>().HideFront();
            secondCard.GetComponent<Card>().HideFront();
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



    public void CreateCardGrid(int rows, int cols, GameObject cardPrefab, List<Sprite> frontSprites)
    {
        Transform spawnerTransform = Spawner.GetComponent<Transform>();

        // Ensure cardPrefab has a SpriteRenderer component
        SpriteRenderer spriteRenderer = cardPrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Card prefab must have a SpriteRenderer component.");
            return;
        }

        void PositionCard(GameObject card, int row, int col)
        {
            // Get camera dimensions in world units
            float cameraHeight = gameCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * gameCamera.aspect;

            // Calculate card spacing based on available screen space
            float padding = 0.1f; // 10% padding from screen edges
            float availableWidth = cameraWidth * (1 - padding);
            float availableHeight = cameraHeight * (1 - padding);

            // Calculate spacing to fit all cards within available space
            float horizontalSpacing = availableWidth / cols;
            float verticalSpacing = availableHeight / rows;

            // Use the smaller spacing to maintain aspect ratio
            float spacing = Mathf.Min(horizontalSpacing, verticalSpacing);

            // Calculate total grid dimensions
            float totalGridWidth = (cols - 1) * spacing;
            float totalGridHeight = (rows - 1) * spacing;

            // Start from camera center and offset by half grid size to center the grid
            Vector3 cameraCenter = gameCamera.transform.position;
            float startX = cameraCenter.x - totalGridWidth / 2f;
            float startY = cameraCenter.y + totalGridHeight / 2f;

            // Calculate final card position
            float x = startX + col * spacing;
            float y = startY - row * spacing;

            Vector2 position = new Vector3(x, y);

            card.transform.position = position;
            card.transform.localScale = Vector3.one * scalingFactor;

            Debug.Log($"Card at ({row},{col}) positioned at: {position}");
        }

        // Debug.Log($"Grid start position: {startPosition}, Grid dimensions:{totalGridWidth}x{totalGridHeight}");

        int[] shuffledNumbers = CreatePairedNumbersArray(rows, cols);
        totalPairs = shuffledNumbers.Length / 2;

        // Instantiate the grid of cards.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject newCard = Instantiate(cardPrefab, spawnerTransform);

                Debug.Log($"Spawner position: {spawnerTransform.position}");

                Debug.Log($"Card grid text position: {cardGridText.transform.position}");

                Debug.Log($"Instantiated card at position: {newCard.transform.position}");

                // Position the card
                PositionCard(newCard, row, col);

                // Calculate an index based on the row and col (example using a numbers array)
                int index = row * cols + col;

                // Clamp or wrap the index if necessary – here we assume 'numbers' has enough values
                int id = shuffledNumbers[index];

                Card cardScript = newCard.GetComponent<Card>();

                // Assign a front sprite to the card.
                if (cardScript != null && id < frontSprites.Count)
                {
                    Transform frontTransform = cardScript.transform.Find("FrontSprite");
                    if (frontTransform != null)
                    {
                        // get the FrontSprite GameObject and assign a sprite to it
                        GameObject frontSpriteGO = frontTransform.gameObject;
                        // Now you can access the SpriteRenderer component on frontSpriteGO:
                        SpriteRenderer sr = frontSpriteGO.GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            cardScript.id = id;
                            sr.sprite = frontSprites[id];
                            // Now call SetFrontSprite on the Card component.
                            cardScript.SetFrontSprite(sr.sprite);
                        }
                        else
                        {
                            Debug.LogError("SpriteRenderer not found on FrontSprite.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Child GameObject 'FrontSprite' not found.");
                    }
                }
                else
                {
                    Debug.LogError(
                        "Either Card component not found or front sprite index is out of range."
                    );
                }
            }
        }
    }
}
