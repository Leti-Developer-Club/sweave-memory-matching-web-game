using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject frontSprite; // Child GameObject that displays the front image
    [SerializeField] GameObject backSprite;  // Child GameObject of frontSprite that displays the back sprite or image
    private float startTime;
    private bool isRoundStarting;
    private bool isFrontVisible;
    float revealTime;

    // Desired target dimensions for the front sprite within the card container.
    // These values represent the size of the card's face regardless of the sprite's native size.
    public Vector2 targetSpriteSize = new Vector2(1.75f, 1.75f);

    public Vector2 targetBackSpriteSize = new Vector2(3.0f, 3.0f);    // For the back sprite.

    // Store the original prefab scale instead of trying to calculate new ones
    private Vector3 originalBackScale;

    public int id;

    public bool IsMatched { get; set; } // Property to track if the card is matched
    // public bool IsRevealed { get; set; } // Property to track if the card is revealed

    void Awake()
    {
        // Store the original scale from the prefab
        if (backSprite != null)
        {
            originalBackScale = backSprite.transform.localScale * 5.75f;
            Debug.Log($"Original back scale stored: {originalBackScale}");
        }
    }
    void Start()
    {
        if (GameManager.Instance != null)
        {
            revealTime = GameManager.Instance.revealTime;
        }
        else
        {
            Debug.LogError("GameManager script is not available");
        }

        startTime = Time.realtimeSinceStartup;
        isRoundStarting = true;
        isFrontVisible = true;

        // Ensure back sprite maintains its original scale
        if (backSprite != null)
        {
            backSprite.transform.localScale = originalBackScale;
        }

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

        // Ensure back sprite scale hasn't changed
        if (backSprite != null && backSprite.transform.localScale != originalBackScale)
        {
            backSprite.transform.localScale = originalBackScale;
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
    public void SetFrontSprite(Sprite newSprite)
    {
        // Calculate scaling factors so that the sprite fits within targetSpriteSize.
        // You might want to preserve the aspect ratio.
        float scaleX = targetSpriteSize.x / newSprite.bounds.size.x;
        float scaleY = targetSpriteSize.y / newSprite.bounds.size.y;
        // To preserve the aspect ratio, choose the smaller scale factor:
        float scaleFactor = Mathf.Min(scaleX, scaleY);

        // Set the local scale of the frontSprite child accordingly.
        frontSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

    }

    public void SetBackSprite(Sprite newSprite)
    {
        // Calculate scaling factors for the back sprite based on its own target size.
        float scaleX = targetBackSpriteSize.x / newSprite.bounds.size.x;
        float scaleY = targetBackSpriteSize.y / newSprite.bounds.size.y;
        float scaleFactor = Mathf.Min(scaleX, scaleY);

        // Set the local scale of the backSprite child accordingly.
        backSprite.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
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
            frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
            backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
        }
        else
        {
            frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
            backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
        }

        //set a scale for the back sprite 
        // Ensure the back sprite maintains its original scale
        backSprite.transform.localScale = originalBackScale;
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideFront();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicking on the card...: " + id);

        // Check if cards can currently be revealed
        if (!GameManager.Instance.canReveal)
        {
            return; // Exit if not allowed.
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
        get { return isFrontVisible; } // or some internal bool.
    }
}
