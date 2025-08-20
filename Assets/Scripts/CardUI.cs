using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler, ICard
{
    [Header("UI Components")]
    public GameObject frontSprites;
    public Image centerSprite; // UI Image that displays the front image

    //other positioned sprites
    public Image topLeftSprite;
    public Image topRightSprite;
    public Image bottomLeftSprite;
    public Image bottomRightSprite;

    [SerializeField]
    public Image backSprite; // UI Image that displays the back sprite or image

    private float startTime;
    private bool isRoundStarting;
    private bool isFrontVisible;
    float revealTime;

    public int id { get; set; }

    public bool IsMatched { get; set; } // Property to track if the card is matched

    void Awake()
    {
        if (GameUIManager.Instance != null)
        {
            revealTime = GameUIManager.Instance.revealTime;
        }
        else
        {
            Debug.LogError("GameUIManager script is not available");
        }
    }

    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        isRoundStarting = true;
        isFrontVisible = true;

        // Set initial positions for front and back sprites.
        SetSpritePositions(true);
    }

    void Update()
    {
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        if (isRoundStarting && elapsedTime > revealTime)
        {
            HideFront();
            isRoundStarting = false;
        }
    }

    public void HideFront()
    {
        if (isFrontVisible && !IsMatched)
        {
            // Logic to hide the front sprite
            SetSpritePositions(false);
            isFrontVisible = false;
            Debug.Log("Front sprite hidden");
        }
    }

    /// <summary>
    /// Sets the front sprite to a new sprite for UI Image components.
    /// </summary>
    /// <param name="newSprite">The sprite to assign.</param>
    public void SetFrontSprite(Sprite newSprite)
    {
        // Assign the sprite to the center sprite Image
        if (centerSprite != null)
        {
            centerSprite.sprite = newSprite;
            centerSprite.preserveAspect = true;
        }

        // Assign the sprite to the top left sprite Image
        if (topLeftSprite != null)
        {
            topLeftSprite.sprite = newSprite;
            topLeftSprite.preserveAspect = true;
        }

        // Assign the sprite to the top right sprite Image
        if (topRightSprite != null)
        {
            topRightSprite.sprite = newSprite;
            topRightSprite.preserveAspect = true;
        }

        // Assign the sprite to the bottom right sprite Image
        if (bottomRightSprite != null)
        {
            bottomRightSprite.sprite = newSprite;
            bottomRightSprite.preserveAspect = true;
        }

        // Assign the sprite to the bottom left sprite Image
        if (bottomLeftSprite != null)
        {
            bottomLeftSprite.sprite = newSprite;
            bottomLeftSprite.preserveAspect = true;
        }
    }

    public void SetBackSprite(Sprite newSprite)
    {
        if (backSprite != null)
        {
            backSprite.sprite = newSprite;
            backSprite.preserveAspect = true;
        }
    }

    /// <summary>
    /// Sets the visibility of the front and back sprites for UI components.
    /// </summary>
    /// <param name="showFront">If true, the front sprite is shown; if false, it is hidden.</param>
    private void SetSpritePositions(bool showFront)
    {
        if (frontSprites != null)
            frontSprites.SetActive(showFront);

        if (backSprite != null)
            backSprite.gameObject.SetActive(!showFront);
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideFront();
    }

    // UI click handler - replaces OnMouseDown for UI elements
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Card {id} clicked - Current state: front visible = {isFrontVisible}");

        // Check if cards can currently be revealed
        if (!GameUIManager.Instance.canReveal)
        {
            Debug.Log("Cards cannot be revealed right now");
            return;
        }

        // only show the front sprite, if it is hidden
        if (!isFrontVisible)
        {
            ShowFrontSprite();
            StartCoroutine(HideAfterDelay(revealTime));
            // let the GameUIManager know the front sprite is visible
            GameUIManager.Instance.CardRevealed(this);
        }
    }

    public void ShowFrontSprite()
    {
        SetSpritePositions(true);
        isFrontVisible = true;
        Debug.Log("Front sprite revealed");
    }

    public bool IsRevealed
    {
        get { return isFrontVisible; }
    }

    /// <summary>
    /// Updates the card size for UI layout - uses RectTransform instead of sprite scaling
    /// </summary>
    /// <param name="cellSize">The calculated cell size from FlexibleGridLayoutGroup</param>
    public void UpdateSpriteSize(Vector2 cellSize)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = cellSize;
            Debug.Log($"Card {id} updated to cell size: {cellSize}");
        }
        else
        {
            Debug.LogError($"Card {id} does not have a RectTransform component!");
        }
    }
}
