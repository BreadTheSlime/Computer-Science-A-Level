using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] int finalScore;
    [SerializeField] int highScore;

    // An object that contains all UI elements to be displayed when the player dies
    public GameObject deathScreen;
    // An object that contains all UI elements to be displayed when the player wins, including finalScoreText and highScoreText
    public GameObject endScreen;
    // The player's final score
    public TextMeshProUGUI finalScoreText;
    // The player's highest score
    public TextMeshProUGUI highScoreText;
    CollisionManager collisionManager;

    private void Start()
    {
        // Set all game end UI elements to invisible
        collisionManager = GetComponent<CollisionManager>();
        collisionManager.setGUIActivity(endScreen, false);
        collisionManager.setGUIActivity(deathScreen, false);
    }

    public void DisplayDeathScreen()
    {
        // Set the death screen to visible
        collisionManager.setGUIActivity(deathScreen, true);
    }

    public void DisplayFinalScore()
    {
        // Set finalScore to the proper value
        finalScore = collisionManager.scoreCount;
        // Replace the value stored in the playerprefs variable "highScore" with the final score if it is higher
        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("highScore", finalScore);
        }
        // Set highScore to the value stored in "highScore"
        highScore = PlayerPrefs.GetInt("highScore");
        // Set the UI elements' text to their corresponding variable's values
        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();
        // Set the end screen to visible
        collisionManager.setGUIActivity(endScreen, true);
    }
}
