using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    GameObject gameManager;
    CollisionManager collisionManager;

    // Activates when the object is instantiated
    private void Awake()
    {
        // Get instances of the PlayerController and CollisionManager
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("Game Manager");
        collisionManager = gameManager.GetComponent<CollisionManager>();

    }

    void Update()
    {
        // Check that the player exists (Has not already been destroyed)
        if (player)
        {
            // Check if the player is in the same position as the exit door, and has the requisite number of keys
            if (Vector3.Distance(transform.position, player.transform.position) == 0 && collisionManager.keyCount >= 4)
            {
                // Trigger the game to end, with the player having won
                playerController.PlayerDeath(true);
            }
        }
    }
}
