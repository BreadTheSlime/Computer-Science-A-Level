using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController playerController;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI trapText;

    public int healthCount;
    public int scoreCount;
    public int keyCount;
    public int trapCount;

    [SerializeField] GameObject trap;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        // Define initial states for the variables and GUI
        healthCount = 3;
        scoreCount = 0;
        keyCount = 0;
        trapCount = 0;
        UpdateGUI();
    }

    // Method to enable or disable a sprite in the GUI
    public void setGUIActivity(GameObject element, bool active)
    {
        Vector3 GUIScale;

        // If the active parameter is true, set the scale of the image to 1
        if (active == true)
        {
            GUIScale = new Vector3(1, 1, 1);
        }
        // Otherwise, set it to 0 (Rendering it invisible)
        else
        {
            GUIScale = new Vector3(0, 0, 0);
        }

        element.transform.localScale = GUIScale;
    }

    // Called using the OnCollisionEnter2D method in the playercontroller script
    public void playerCollision(Collision2D collision)
    {
        // Call ItemPickup if the collided object is an item
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemPickup(collision.gameObject);
        }
        // Reduce health if the collided object is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            healthCount -= 1;
        }
        // Destroy the object
        Destroy(collision.gameObject);
        // Update the GUI to reflect any changes
        UpdateGUI();
        return;
    }

    void ItemPickup(GameObject pickup)
    {
        // Get the itemAttributes component of the item
        ItemAttributes itemAttributes = pickup.GetComponent<ItemAttributes>();

        // Alter the player's stats depending on which item was picked up, identified by a variable inside itemAttributes
        switch (itemAttributes.itemType)
        {
            case "Heart":
                // Add to the player's health if they are missing some
                if (healthCount < 3)
                {
                    healthCount += 1;
                    break;
                }
                // If their health is full, give them some score as a bonus
                scoreCount += 25;
                break;

            case "Key":
                if (keyCount >= 5)
                {
                    scoreCount += 25;
                    break;
                }
                // If the player has the prerequisite number of keys, give them some score as a bonus
                keyCount += 1;
                break;

            case "BombUnlit":
                trapCount += 1;
                break;

            case "Treasure5":
                scoreCount += 5;
                break;

            case "Treasure10":
                scoreCount += 10;
                break;

            case "Treasure25":
                scoreCount += 25;
                break;

            case "Treasure50":
                scoreCount += 50;
                break;

            case "Treasure100":
                scoreCount += 100;
                break;
        }
    }

    // Place a trap on the given position
    public void PlaceTrap(Vector3 position)
    {
        // If the player has at least one trap, place one on the given position, then update the GUI
        if (trapCount > 0)
        {
            trapCount -= 1;
            Instantiate(trap, position, Quaternion.identity);
            UpdateGUI();
        }
    }

    public void UpdateGUI()
    {
        // Set all hearts to invisible as a default
        setGUIActivity(heart1.gameObject, false);
        setGUIActivity(heart2.gameObject, false);
        setGUIActivity(heart3.gameObject, false);

        // Re-enable one for each point of health that the player has
        if (healthCount > 0)
        {
            setGUIActivity(heart1.gameObject, true);
        }
        if (healthCount > 1)
        {
            setGUIActivity(heart2.gameObject, true);
        }
        if (healthCount > 2)
        {
            setGUIActivity(heart3.gameObject, true);
        }
        if (healthCount > 3)
        {
            healthCount = 3;
        }

        // Set the GUI text to the proper values
        scoreText.text = "" + scoreCount;
        keyText.text = "" + keyCount + "/5";
        trapText.text = "" + trapCount;

        // If health is zero or lower, the player dies
        if (healthCount < 1)
        {
            playerController.PlayerDeath(false);
        }
    }
}
