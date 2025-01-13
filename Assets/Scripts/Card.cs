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

    [SerializeField] float revealTime = 2.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        Debug.Log("Initial startTime: " + startTime);
        isRoundStarting = true;


        // load sprites from the folder containing the sprites and add them to the frontSprites list
        frontSprites = Resources.LoadAll<Sprite>("Resources/Sprites/FrontSprites/").ToList();
        Debug.Log("No of sprites: " + frontSprites.Count);


        if (frontSprites == null || frontSprites.Count < 0)
        {
            Debug.LogError($"No sprites found at path the specified path");
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
        Debug.LogError("This is the length of the list: " + listLength);

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
        backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
        frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // get the actual start time of the game 
        float elapsedTime = Time.realtimeSinceStartup - startTime;

        Debug.Log($"Current time: {Time.realtimeSinceStartup}, startTime: {startTime}, elapsed: {elapsedTime}");


        // Debug.Log("The game has been running for: " + elapsedTime + "seconds");
        // Debug.Log("elapsedTime: " + elapsedTime);
        Debug.Log("revealTime: " + revealTime);
        Debug.Log("isRoundStarting: " + isRoundStarting);
        Debug.Log("Condition check: " + (elapsedTime > revealTime && isRoundStarting));


        // hide the cards after the specified reveal time
        FlipCard(elapsedTime, revealTime);

    }

    public void FlipCard(float elapsedTime, float revealTime)
    {
        if (elapsedTime > revealTime)
        {
            Debug.Log("Flipping card!");

            // hide the front sprites after a brief moment, then flip the card to show the back

            backSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -2.0f);
            Debug.Log("Z position of the back sprite: " + backSprite.transform.position.z);

            frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, 4.0f);
            Debug.Log("Z position of the front sprite: " + frontSprite.transform.position.z);

            // isRoundStarting = false;

        }

    }

    // when i click on a card is should show the front sprite for a brief moment
    // if the card the card has been clicked on, 
    // flip card for a specified period to show the front sprite
    // disable back sprite, then enable front sprite
    // add animation to create illusion of card flipping

    // public void OnMouseDown()
    // {   
    //     // the back sprite should be closer to the camera, and 
    //     // the z position of the back sprite is -0.2
    //     // the front sprite should be behind the back sprite
    //     // the z position of the front sprite is 0.1
    //     // when the back sprite is disabled the front sprite should be rendered
    //     Debug.Log("This is the game card");
    //     if (backSprite.activeSelf)
    //     {
    //         backSprite.SetActive(false);
    //         frontSprite.transform.position = new Vector3(transform.position.x, transform.position.y, -0.02f);
    //         // at the start of the game, the front of the card(s) should be shown to the player for a few seconds, 
    //         // then the card flip over to show the back of the card or hide the front of the card 
    //         // the front of the spr

    //     }

    // }
}