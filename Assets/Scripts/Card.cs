using UnityEngine;

public class Card : MonoBehaviour
{
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