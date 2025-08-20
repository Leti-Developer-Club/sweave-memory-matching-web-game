using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    public GameObject frontSprites;
    public GameObject centerSprite; // Child GameObject that displays the front image

    //other positioned sprites
    public GameObject topLeftSprite;
    public GameObject topRightSprite;
    public GameObject bottomLeftSprite;
    public GameObject bottomRightSprite;

    [SerializeField]
    public GameObject backSprite; // Child GameObject of frontSprite that displays the back sprite or image
    private float startTime;
    private bool isRoundStarting;
    private bool isFrontVisible;
    float revealTime;

    public int id { get; set; }

    public bool IsMatched { get; set; } // Property to track if the card is matched

    void Awake()
    {
        if (GameManager.Instance != null)
        {
            revealTime = GameManager.Instance.revealTime;
        }
        else
        {
            Debug.LogError("GameManager script is not available");
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
            // IsRevealed = false;
            SetSpritePositions(false);
            isFrontVisible = false;
            Debug.Log("Front sprite hidden");
        }
    }

    // / <summary>
    // / Sets the front sprite to a new sprite and scales it to fit the card's target size.
    // / </summary>
    // / <param name="newSprite">The sprite to assign.</param>
    public void SetFrontSprite(Vector2 cellSize, Sprite newSprite)
    {
        // the size of the card should be based on the grid cell size
        float scaleX = cellSize.x / newSprite.bounds.size.x;
        float scaleY = cellSize.y / newSprite.bounds.size.y;
        float scaleFactor = Mathf.Min(scaleX, scaleY);

        // Assign the sprite to the center sprite renderer
        if (centerSprite != null && centerSprite.GetComponent<SpriteRenderer>() != null)
        {
            centerSprite.GetComponent<SpriteRenderer>().sprite = newSprite;
            centerSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }

        // Assign the sprite to the top left sprite renderer
        if (topLeftSprite != null && topLeftSprite.GetComponent<SpriteRenderer>() != null)
        {
            topLeftSprite.GetComponent<SpriteRenderer>().sprite = newSprite;
            topLeftSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }

        // Assign the sprite to the top right sprite renderer
        if (topRightSprite != null && topRightSprite.GetComponent<SpriteRenderer>() != null)
        {
            topRightSprite.GetComponent<SpriteRenderer>().sprite = newSprite;
            topRightSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }

        // Assign the sprite to the bottom right sprite renderer
        if (bottomRightSprite != null && bottomRightSprite.GetComponent<SpriteRenderer>() != null)
        {
            bottomRightSprite.GetComponent<SpriteRenderer>().sprite = newSprite;
            bottomRightSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }

        // Assign the sprite to the bottom left sprite renderer
        if (bottomLeftSprite != null && bottomLeftSprite.GetComponent<SpriteRenderer>() != null)
        {
            bottomLeftSprite.GetComponent<SpriteRenderer>().sprite = newSprite;
            bottomLeftSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
    }

    public void SetBackSprite(Vector2 cellSize)
    {
        Sprite newSprite = backSprite.GetComponent<SpriteRenderer>().sprite;

        // calculate scaling for the backsprite based on the grid cell
        // the size of the card should be based on the grid cell size
        float scaleX = cellSize.x / newSprite.bounds.size.x;
        float scaleY = cellSize.y / newSprite.bounds.size.y;
        float scaleFactor = Mathf.Min(scaleX, scaleY);

        // Set the local scale of the backSprite child accordingly.
        backSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // Scale the FrontSprites container to match the back sprite size
        // SetFrontSpritesContainerScale(scaleFactor);
    }

    /// <summary>
    /// Sets the scale of the FrontSprites container to match the back sprite scale.
    /// This ensures the front sprite container (mask) has the same size as the back sprite.
    /// </summary>
    /// <param name="scaleFactor">The scale factor to apply to the FrontSprites container.</param>
    public void SetFrontSpritesContainerScale(float scaleFactor)
    {
        if (frontSprites != null)
        {
            frontSprites.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            Debug.Log($"FrontSprites container scaled to: {scaleFactor}");
        }
        else
        {
            Debug.LogError("FrontSprites GameObject is not assigned!");
        }
    }

    /// <summary>
    /// Sets the positions of the front and back sprites relative to the card container.
    /// </summary>
    /// <param name="showFront">If true, the front sprite is shown; if false, it is hidden.</param>
    private void SetSpritePositions(bool showFront)
    {
        // The container (this GameObject) maintains a fixed position and scale.
        // We just adjust the z-order of the front and back sprites.
        if (showFront)
        {
            frontSprites.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                -4.0f
            );
            backSprite.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                4.0f
            );
        }
        else
        {
            frontSprites.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                4.0f
            );
            backSprite.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                -4.0f
            );
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideFront();
    }

    private void OnMouseDown()
    {
        Debug.Log($"Card {id} clicked - Current state: front visible = {isFrontVisible}");

        // Check if cards can currently be revealed
        if (!GameManager.Instance.canReveal)
        {
            Debug.Log("Cards cannot be revealed right now");
            return;
        }

        // only show the front sprite, if it is hidden
        if (!isFrontVisible)
        {
            ShowFrontSprite();
            StartCoroutine(HideAfterDelay(revealTime));
            // let the GameManager know the front sprite is visible
            GameManager.Instance.CardRevealed(this);
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
    /// Updates the sprite sizes based on the calculated cell size from the layout group.
    /// </summary>
    /// <param name="cellSize">The calculated cell size from FlexibleGridLayoutGroup</param>
    public void UpdateSpriteSize(Vector2 cellSize)
    {
        // Get the current sprites to recalculate their sizes
        Sprite centerSpriteImage = centerSprite?.GetComponent<SpriteRenderer>()?.sprite;

        if (centerSpriteImage != null)
        {
            // Recalculate scale based on new cell size
            float scaleX = cellSize.x / centerSpriteImage.bounds.size.x;
            float scaleY = cellSize.y / centerSpriteImage.bounds.size.y;
            float scaleFactor = Mathf.Min(scaleX, scaleY);

            // Update all front sprites
            if (centerSprite != null)
                centerSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            if (topLeftSprite != null)
                topLeftSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            if (topRightSprite != null)
                topRightSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            if (bottomLeftSprite != null)
                bottomLeftSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            if (bottomRightSprite != null)
                bottomRightSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

            // Update back sprite scale
            if (backSprite != null)
                backSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

            // Update front sprites container scale
            SetFrontSpritesContainerScale(scaleFactor);

            Debug.Log($"Card {id} updated to cell size: {cellSize}, scale factor: {scaleFactor}");
        }
    }
}
