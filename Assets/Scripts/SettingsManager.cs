using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    public GameSettingsScriptableObject gameSettings;
    // public static SettingsManager Instance { get; private set; }
    public GameObject chooseDifficultyButton;
    public GameObject easyButton;
    public GameObject mediumButton;
    public GameObject hardButton;
    public GameObject closeButton;
    public GameObject difficultyModalCanvas;
    public GameObject settingsCanvas;

    // public GameObject gameCanvas;

    // public int rows;    // always 2
    // public int cols;    // varies based on difficulty

    // public int gridSize;

    // public List<Sprite> frontSprites = new List<Sprite>();
    // public float revealTime;
    // public GameObject memoryCard;

    // public Card firstRevealed;
    // public Card secondRevealed;
    // public bool canReveal = true;

    // private int matchedPairs = 0;
    // private int totalPairs;

    // public GameObject WinScreenCanvas;

    // public GameObject restartButton;
    // public GameObject gameDifficultyButton;

    // public GameObject quitButton;

    // public Transform cardParent; // Parent object to organize cards in the hierarchy
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //         SceneManager.sceneLoaded += OnSceneLoaded;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // private void OnDestroy()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }
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

            });
            thisMediumButton.onClick.AddListener(() =>
            {
                gameSettings.ChooseDifficulty(thisMediumButton.GetComponentInChildren<TMP_Text>());

            });
            thisHardButton.onClick.AddListener(() =>
            {
                gameSettings.ChooseDifficulty(thisHardButton.GetComponentInChildren<TMP_Text>());

            });

        }

        if (thisCloseButton != null)
        {

            thisCloseButton.onClick.AddListener(() =>
            {
                Debug.Log("Close button clicked...");

                //hide the Difficulty options modal and show the settings scene
                HideCanvas();

                SceneManager.LoadScene("GameScene");

                // settingsCanvas.SetActive(true);

            });

        }
        else
        {
            Debug.LogError("Close button is not assigned");
        }

    }

    // This method is called every time a new scene is loaded
    // void StartGame(Scene scene, LoadSceneMode mode)
    // {
    //     // Reset game state
    //     matchedPairs = 0;
    //     totalPairs = 0;
    //     firstRevealed = null;
    //     secondRevealed = null;
    //     canReveal = true;

    //     // Check if the loaded scene is the one you want
    //     if (scene.name == "GameScene")
    //     {
    //         // Place your code here that you want to run after the GameScene has loaded
    //         Debug.Log("GameScene has loaded!");
    //         // For example, you can call another method
    //         List<Sprite> frontSpriteList = LoadSprites();
    //         CreateCardGrid(rows, cols, memoryCard, frontSpriteList);

    //         gameCanvas = GameObject.FindWithTag("GameCanvas");


    //         // Find the WinScreenCanvas Panel in the GameScene
    //         WinScreenCanvas = GameObject.FindWithTag("WinScreenCanvas");

    //         if (WinScreenCanvas != null)
    //         {
    //             restartButton = GameObject.Find("RestartButton");
    //             gameDifficultyButton = GameObject.Find("GameDifficultyButton");
    //             quitButton = GameObject.Find("QuitButton");

    //             if (restartButton == null && gameDifficultyButton == null && quitButton == null)
    //             {
    //                 Debug.LogError("Restart, gameDifficulty and quit buttons not found in the GameScene!");
    //             }

    //             // Ensure the WinScreenCanvas is disabled initially
    //             WinScreenCanvas.SetActive(false);

    //         }
    //         else
    //         {
    //             Debug.LogError("WinScreenCanvas UI element not found in the GameScene!");

    //         }
    //     }


    //     if (scene.name == "SettingsScene")
    //     {
    //         GameObject difficultyOptionsCanvas = GameObject.FindWithTag("DifficultyOptionsCanvas");
    //         GameObject setdifficultySettingsButton = GameObject.FindWithTag("ChooseDifficultyButton");
    //         GameObject settingsCanvas = GameObject.FindWithTag("SettingsCanvas");

    //         Button _easyButton = GameObject.FindWithTag("EasyButton").GetComponent<Button>();
    //         Button _mediumButton = GameObject.FindWithTag("MediumButton").GetComponent<Button>();
    //         Button _hardButton = GameObject.FindWithTag("HardButton").GetComponent<Button>();
    //         Button _closeButton = GameObject.FindWithTag("CloseButton").GetComponent<Button>();
    //         Button _chooseDifficultyButton = GameObject.FindWithTag("ChooseDifficultyButton").GetComponent<Button>();

    //         if (difficultyOptionsCanvas != null)
    //         {
    //             difficultyOptionsCanvas.SetActive(false);
    //             onDifficultyButtonClick(setdifficultySettingsButton.GetComponent<Button>(), difficultyOptionsCanvas, settingsCanvas);
    //         }
    //         else
    //         {
    //             Debug.LogError("Difficulty Modal Canvas is not assigned!");
    //         }

    //         if (_easyButton != null && _mediumButton != null && _hardButton != null)
    //         {
    //             _easyButton.onClick.AddListener(() =>
    //             {
    //                 gameSettings.ChooseDifficulty(_easyButton.GetComponentInChildren<TMP_Text>());

    //             });
    //             _mediumButton.onClick.AddListener(() =>
    //             {
    //                 gameSettings.ChooseDifficulty(_mediumButton.GetComponentInChildren<TMP_Text>());

    //             });
    //             _hardButton.onClick.AddListener(() =>
    //             {
    //                 gameSettings.ChooseDifficulty(_hardButton.GetComponentInChildren<TMP_Text>());

    //             });

    //         }

    //         if (_closeButton != null)
    //         {

    //             _closeButton.onClick.AddListener(() =>
    //             {
    //                 Debug.Log("Close button clicked...");

    //                 //hide the Difficulty options modal and show the settings scene
    //                 HideCanvas();

    //                 settingsCanvas.SetActive(true);

    //             });

    //         }
    //         else
    //         {
    //             Debug.LogError("Close button is not assigned");
    //         }

    //     }
    // }

    // get all the front sprite assets. Each sprite can only be assigned to two cards(a pair of cards) in the card grid
    // List<Sprite> LoadSprites()
    // {
    //     // load sprites from the folder containing the sprites and add them to the frontSprites list
    //     frontSprites = Resources.LoadAll<Sprite>("Sprites/FrontSprites/").ToList();
    //     if (frontSprites == null || frontSprites.Count < 0)
    //     {
    //         Debug.LogError($"No sprites found at path the specified path");
    //     }

    //     Debug.Log("No of sprites: " + frontSprites.Count);

    //     foreach (var sprite in frontSprites)
    //     {
    //         Debug.Log($"Loaded sprite: {sprite.name}");
    //     }

    //     return frontSprites;
    // }

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

    public void onDifficultyButtonClick(Button button, GameObject thisCanvas, GameObject thisSettingsCanvas)
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




    // private int[] ShuffleCards(int[] numbers)
    // {
    //     int[] newArray = numbers.Clone() as int[];
    //     for (int i = newArray.Length - 1; i > 0; i--)
    //     {
    //         int randomIndex = Random.Range(0, i + 1);
    //         int temp = newArray[i];
    //         newArray[i] = newArray[randomIndex];
    //         newArray[randomIndex] = temp;
    //     }
    //     Debug.Log("Shuffled array: " + string.Join(", ", newArray));
    //     return newArray;
    // }

    // public int[] CreatePairedNumbersArray(int rows, int cols)
    // {
    //     int totalCards = rows * cols;
    //     int pairCount = totalCards / 2;
    //     int[] numbers = new int[totalCards];

    //     for (int i = 0; i < pairCount; i++)
    //     {
    //         numbers[i * 2] = i;
    //         numbers[i * 2 + 1] = i;
    //     }

    //     // Ensure to use the shuffled result!
    //     numbers = ShuffleCards(numbers);
    //     return numbers;
    // }

    // This method is called by a card when it's clicked.
    // public void CardRevealed(Card card)
    // {
    //     Debug.Log($"Card revealed: {card.name} with ID: {card.id}");
    //     if (!card.IsRevealed) return;

    //     if (firstRevealed == null)
    //     {
    //         firstRevealed = card;
    //         Debug.Log($"First card revealed: {firstRevealed.name} with ID: {firstRevealed.id}");
    //     }
    //     else if (secondRevealed == null && firstRevealed != card)
    //     {
    //         secondRevealed = card;
    //         canReveal = false;
    //         Debug.Log($"Second card revealed: {secondRevealed.name} with ID: {secondRevealed.id}");
    //         StartCoroutine(MatchCards(firstRevealed, secondRevealed));
    //     }
    // }



    //     How do i use the MatchCards method, where will I be referencing the firstCard and secondCard from?

    // public IEnumerator MatchCards(Card firstCard, Card secondCard)
    // {
    //     if (firstCard.id == secondCard.id)
    //     {
    //         Debug.Log("Cards match!");

    //         // let those cards show their front sprites till the end of the game
    //         firstCard.GetComponent<Card>().IsMatched = true;
    //         secondCard.GetComponent<Card>().IsMatched = true;

    //         // let those cards show their front sprites till the end of the game
    //         firstCard.GetComponent<Card>().ShowFrontSprite();
    //         secondCard.GetComponent<Card>().ShowFrontSprite();

    //         matchedPairs++;
    //         if (matchedPairs == totalPairs)
    //         {
    //             Debug.Log("All cards matched! Player wins!");

    //             // Hide the Game Canvas
    //             if (gameCanvas == null)
    //             {
    //                 Debug.Log("GameCanvas can't be found");
    //             }
    //             else
    //             {
    //                 gameCanvas.SetActive(false);
    //             }

    //             // Trigger win sequence here (e.g., show win screen, play sound, etc.)
    //             // WinGame();
    //         }

    //     }
    //     else
    //     {
    //         Debug.Log("Cards don't match!");
    //         // Wait a bit so the player can see the cards.
    //         yield return new WaitForSeconds(1.0f);
    //         firstCard.GetComponent<Card>().HideFront();
    //         secondCard.GetComponent<Card>().HideFront();
    //     }
    //     // Reset for the next pair.
    //     firstRevealed = null;
    //     secondRevealed = null;
    //     canReveal = true;
    // }

    // private void WinGame()
    // {
    //     // Log the win event
    //     Debug.Log("Congratulations! You've won the game!");

    //     // Show a win screen
    //     ShowWinScreenCanvas();

    //     //Show score: moves and time spent
    //     // ShowScore();

    //     // Play a win sound
    //     // PlayWinSound();

    //     // Disable further input
    //     // DisablePlayerInput();

    //     // Optionally, show statistics or offer to restart the game
    //     // ShowGameStatistics();
    // }

    // private void ShowWinScreenCanvas()
    // {
    //     if (WinScreenCanvas != null)
    //     {
    //         WinScreenCanvas.SetActive(true);

    //         if (restartButton != null && gameDifficultyButton != null && quitButton != null)
    //         {
    //             restartButton.GetComponent<Button>().onClick.RemoveAllListeners();
    //             restartButton.GetComponent<Button>().onClick.AddListener(() =>
    //             {
    //                 Debug.Log("Restarting the game...");
    //                 WinScreenCanvas.SetActive(false);
    //                 SceneManager.LoadScene("GameScene");
    //             });

    //             gameDifficultyButton.GetComponent<Button>().onClick.RemoveAllListeners();
    //             gameDifficultyButton.GetComponent<Button>().onClick.AddListener(() =>
    //             {
    //                 Debug.Log("Choose a difficulty setting...");
    //                 WinScreenCanvas.SetActive(false);
    //                 SceneManager.LoadScene("SettingsScene");
    //             });

    //             quitButton.GetComponent<Button>().onClick.RemoveAllListeners();
    //             quitButton.GetComponent<Button>().onClick.AddListener(() =>
    //             {
    //                 Debug.Log("Quitting game...");
    //                 //Clear everything and go to the first scene
    //                 // WinScreenCanvas.SetActive(false);
    //                 // HideCanvas();s
    //                 SceneManager.LoadScene("WelcomeScene");
    //             });
    //         }
    //         else
    //         {
    //             Debug.LogError("Restart button not found!");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("WinScreenCanvas UI element not found!");
    //     }
    // }


    // private void PlayWinSound()
    // {
    //     // Assuming you have an AudioSource component for playing sounds
    //     AudioSource audioSource = GetComponent<AudioSource>();
    //     if (audioSource != null)
    //     {
    //         audioSource.Play();
    //     }
    //     else
    //     {
    //         Debug.LogError("AudioSource component not found!");
    //     }
    // }

    // private void DisablePlayerInput()
    // {
    //     // Disable player input to prevent further interaction
    //     canReveal = false;
    // }

    // private void ShowGameStatistics()
    // {
    //     // Implement logic to display game statistics (e.g., time taken, moves made)
    //     Debug.Log("Game completed in X moves and Y seconds.");
    // }


    // how do i access the panel component from the GameScene if my GameManager script is attached to a gameobject is a different scene - the SettingsScene?
    // is it better if access the panel component from the Card prefab and assign panel's transform as the parent of the card prefab's transform?
    // The grid should be in the center of the scene
    // public void CreateCardGrid(int rows, int cols, GameObject cardPrefab, List<Sprite> frontSprites)
    // {
    //     // Find the Panel in the current scene (make sure it has the proper tag)
    //     GameObject panel = GameObject.FindWithTag("CardGridPanel");
    //     if (panel == null)
    //     {
    //         Debug.LogError("Panel with tag 'CardGridPanel' not found in the current scene.");
    //         return;
    //     }

    //     // Get the Panel's RectTransform so we know its actual dimensions.
    //     RectTransform panelTransform = panel.GetComponent<RectTransform>();
    //     if (panelTransform == null)
    //     {
    //         Debug.LogError("Panel does not have a RectTransform component.");
    //         return;
    //     }

    //     // Get the actual Rect (size and position) of the Panel.
    //     Rect panelRect = panelTransform.rect;
    //     // In local coordinates, the bottom left is:
    //     // Vector2 panelBottomLeft = new Vector2(panelRect.xMin, panelRect.yMin);

    //     // Get the center of the panel in local coordinates.
    //     Vector2 panelCenter = panelRect.center;

    //     // Ensure cardPrefab has a SpriteRenderer component
    //     SpriteRenderer spriteRenderer = cardPrefab.GetComponent<SpriteRenderer>();
    //     if (spriteRenderer == null)
    //     {
    //         Debug.LogError("Card prefab must have a SpriteRenderer component.");
    //         return;
    //     }

    //     // Get the size of the sprite (card) in world units.
    //     Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

    //     // Define the horizontal (and vertical) spacing between cards.
    //     float spacing = 3.35f;

    //     // Calculate total grid dimensions (accounting for spacing)
    //     float totalGridWidth = cols * spriteSize.x + (cols - 1) * spacing;
    //     float totalGridHeight = rows * spriteSize.y + (rows - 1) * spacing;
    //     // If we want the grid's center to be at the Panel's bottom left,
    //     // then the center of the grid should be exactly at panelBottomLeft.
    //     // To calculate the starting (top-left) position for the grid, we subtract half of the grid's width (and add half of the card width)
    //     // for x and add half of the grid's height (and subtract half of the card height) for y.
    //     // Vector2 startPosition = new Vector2(
    //     //      panelBottomLeft.x - totalGridWidth / 2 + spriteSize.x / 2,
    //     //      panelBottomLeft.y + totalGridHeight / 2 - spriteSize.y / 2
    //     // );

    //     // Calculate the starting position so that the grid is centered at the panel's center.
    //     Vector2 startPosition = new Vector2(
    //         panelCenter.x - totalGridWidth / 2 + spriteSize.x / 2,
    //         panelCenter.y + totalGridHeight / 2 - spriteSize.y / 2
    //     );

    //     int[] shuffledNumbers = CreatePairedNumbersArray(rows, cols);
    //     totalPairs = shuffledNumbers.Length / 2;


    //     // Instantiate the grid of cards.
    //     for (int row = 0; row < rows; row++)
    //     {
    //         for (int col = 0; col < cols; col++)
    //         {
    //             // Calculate each card's position using the starting point plus offsets for each row and column.
    //             float posX = col * (spriteSize.x + spacing) + startPosition.x;
    //             float posY = -row * (spriteSize.y + spacing) + startPosition.y;

    //             Vector2 position = new Vector2(posX, posY);

    //             // Instantiate the card and set its parent to the Panel
    //             GameObject newCard = Instantiate(cardPrefab, panel.transform);


    //             newCard.transform.localPosition = new Vector3(position.x, position.y, 0);

    //             // Assign a front sprite to the card.
    //             // Calculate an index based on the row and col (example using a numbers array)
    //             int index = row * cols + col;
    //             // Clamp or wrap the index if necessary – here we assume 'numbers' has enough values
    //             int id = shuffledNumbers[index];

    //             Card cardScript = newCard.GetComponent<Card>();

    //             // cardScript.LogCurrentScales();

    //             if (cardScript != null && id < frontSprites.Count)
    //             {

    //                 Transform frontTransform = cardScript.transform.Find("FrontSprite");
    //                 if (frontTransform != null)
    //                 {
    //                     // get the FrontSprite GameObject and assign a sprite to it
    //                     GameObject frontSpriteGO = frontTransform.gameObject;
    //                     // Now you can access the SpriteRenderer component on frontSpriteGO:
    //                     SpriteRenderer sr = frontSpriteGO.GetComponent<SpriteRenderer>();
    //                     if (sr != null)
    //                     {
    //                         cardScript.id = id;
    //                         sr.sprite = frontSprites[id];
    //                         // Now call SetFrontSprite on the Card component.
    //                         cardScript.SetFrontSprite(sr.sprite);
    //                     }
    //                     else
    //                     {
    //                         Debug.LogError("SpriteRenderer not found on FrontSprite.");
    //                     }
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError("Child GameObject 'FrontSprite' not found.");
    //                 }

    //             }
    //             else
    //             {
    //                 Debug.LogError("Either Card component not found or front sprite index is out of range.");
    //             }
    //         }
    //     }

    //     // StartCoroutine(MatchCards(firstRevealed, secondRevealed));
    // }

}
