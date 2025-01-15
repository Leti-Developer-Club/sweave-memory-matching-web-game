using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    // since the game will have varying levels of difficulty - from 3 x 3 to 6 x 6 grids,
    // the sprites for the front of the cards will have to be loaded randomly and programmatically
    // collectively the front sprites will be an array or list of type Sprite
    // Each card will randomly set one of the front sprites from the list as its front sprite
    // the default size of the array will be 3 for a 3 x 3 grid
    [SerializeField] List<Sprite> frontSprites = new List<Sprite>();
    [SerializeField] GameObject frontSprite;
    [SerializeField] GameObject backSprite;

    private float startTime;

    // has the game started?
    private bool isRoundStarting;

    private bool isFrontVisible;

    // private bool isBackVisible;
    [SerializeField] float revealTime = 2.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // load sprites from the folder containing the sprites and add them to the frontSprites list
        frontSprites = Resources.LoadAll<Sprite>("Sprites/FrontSprites/").ToList();
        // Debug.Log("No of sprites: " + frontSprites.Count);

        startTime = Time.realtimeSinceStartup;
        // Debug.Log("Initial startTime: " + startTime);
        isRoundStarting = true;


        if (frontSprites == null || frontSprites.Count < 0)
        {
            // Debug.LogError($"No sprites found at path the specified path");
            return;
        }

        foreach (var sprite in frontSprites)
        {
            Debug.Log($"Loaded sprite: {sprite.name}");
        }

        // get the sprite on the SpriteRenderer component on the frontSprite gameobject
        // SpriteRenderer frontSpriteSpriteRenderer = frontSprite.GetComponent<SpriteRenderer>();
        // Sprite actualFrontSprite = frontSpriteSpriteRenderer.sprite;
        // actualFrontSprite = frontSprites[randomValue];

        // assign one of the items in the list as the frontsprite of the card, it should be random
        // the sprites should be assigned randomly
        int listLength = frontSprites.Count;
        // Debug.LogError("This is the length of the list: " + listLength);

        if (listLength > 0)
        {
            int randomValue = Random.Range(0, listLength);
            frontSprite.GetComponent<SpriteRenderer>().sprite = frontSprites[randomValue];

        }

        else
        {
            Debug.LogError("Cannot assign a sprite because the list is empty.");
        }

        // show the front sprite by default, after a specified time show only the back sprite of the card

        frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
        // Debug.Log("Z position of the front sprite: " + frontSprite.transform.position.z);

        backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
        // Debug.Log("Z position of the back sprite: " + backSprite.transform.position.z);

        isFrontVisible = true;
        // isBackVisible = false;

    }

    // Update is called once per frame
    void Update()
    {
        // get the actual start time of the game
        float elapsedTime = Time.realtimeSinceStartup - startTime;

        // Debug.Log($"Current time: {Time.realtimeSinceStartup}, startTime: {startTime}, elapsed: {elapsedTime}");


        // Debug.Log("The game has been running for: " + elapsedTime + "seconds");
        // Debug.Log("elapsedTime: " + elapsedTime);
        // Debug.Log("revealTime: " + revealTime);
        // Debug.Log("isRoundStarting: " + isRoundStarting);
        // Debug.Log("Condition check: " + (elapsedTime > revealTime && isRoundStarting));


        // hide the front sprite of the card after the specified reveal time
        if (isRoundStarting && elapsedTime > revealTime)
        {
            HideFront();
            isRoundStarting = false;
        }

        Debug.Log("Is the front sprite visible in the update method? : " + isFrontVisible);
        // Debug.Log("Is the back sprite visible in the update method? : " + isBackVisible);
    }

    public void HideFront()
    {
        if (isFrontVisible)
        {
            Debug.Log("Hide front!");

            frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
            Debug.Log("Z position of the front sprite: " + frontSprite.transform.position.z);

            backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
            Debug.Log("Z position of the back sprite: " + backSprite.transform.position.z);

            isFrontVisible = false;
            // isBackVisible = true;
            Debug.Log("Front sprite hidden");

        }

    }

    // to flip the card from the front to the back, the positions of the front and back sprites are swapped after some time has elapsed
    // to flip the card from the back to the front after the card has been clicked on and how the front sprite for a brief moment, check to see if the card has been clicked, minotor has how time has passed since the card has been clicked on, as long as the brief moment of time hasn't passed, swap then the positions of the front and back sprites, after that brief moment has passed, swap the front and back sprites again
    // when the card is clicked it should show the front sprite for a brief moment
    // this click should only be possible after the card has been flipped for the first time in each round of the game
    private void OnMouseDown()
    {

        Debug.Log("Clicking on the card...");
        if (!isFrontVisible)
        {
            // show the front sprite for a brief moment

            frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
            // Debug.Log("Z position of the front sprite: " + frontSprite.transform.position.z);

            backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
            // Debug.Log("Z position of the back sprite: " + backSprite.transform.position.z);

            isFrontVisible = true;
            // Debug.Log("Is the back sprite visible in the OnMouseDown method? : " + isBackVisible);
            Debug.Log("Front sprite revealed");

            StartCoroutine(HideAfterDelay(2.5f));

        }

    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideFront();
    }
}
