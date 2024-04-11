using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDelayTime;
    [SerializeField] float unearthTime;
    [SerializeField] Transform movePoint;
    [SerializeField] bool onTileLastFrame;
    [SerializeField] Vector3Int targetTilePosition;
    [SerializeField] bool canMove;
    [SerializeField] GameObject gameManager;
    TileManager tileManager;
    TileSpawner tileSpawner;
    [SerializeField] GameOverController gameOverController;
    CollisionManager collisionManager;
    public bool movepointOnPlayer;
    public GameObject door;

    void Start()
    {
        movePoint.parent = null;
        tileManager = gameManager.GetComponent<TileManager>();
        tileSpawner = gameManager.GetComponent<TileSpawner>();
        gameOverController = gameManager.GetComponent<GameOverController>();
        collisionManager = gameManager.GetComponent<CollisionManager>();
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionManager.playerCollision(collision);
    }

    void Update()
    {
        if (!onTileLastFrame && transform.position.x % 1 == 0 && transform.position.y % 1 == 0)
        {
            StartCoroutine("MoveDelay");
        }

        if (transform.position.x % 1 == 0 && transform.position.y % 1 == 0)
        {
            onTileLastFrame = true;
        }
        else
        {
            onTileLastFrame = false;
        }

        if (transform.position == movePoint.transform.position)
        {
            movepointOnPlayer = true;
        }
        else
        {
            movepointOnPlayer = false;
        }

        // If the player can move, move the player
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (tileManager.CheckTile(Vector3Int.FloorToInt(transform.position)) != "trap")
                {
                    collisionManager.PlaceTrap(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y, -4)));
                }
            }

            MovePlayer();

            // Move movepoint if the player is giving input
            if (movepointOnPlayer)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f);
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"));
                }

                tileSpawner.SpawnTiles();
            }

            targetTilePosition = Vector3Int.FloorToInt(new Vector3(movePoint.position.x, movePoint.position.y, 0));

            if (tileManager.CheckTile(targetTilePosition) == "sand")
            {
                StartCoroutine(UnearthTile(targetTilePosition));
            }
            else if (tileManager.CheckTile(targetTilePosition) == "rock")
            {
                movePoint.position = transform.position;
            }
        }
    }

    void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void PlayerDeath(bool won)
    {
        // If the game was won, display the final score
        if (won == true)
        {
            gameOverController.DisplayFinalScore();
        }
        else
        {
            gameOverController.DisplayDeathScreen();
        }
        // Destroy all parts of the player gameObject, including the movepoint
        gameObject.SetActive(false);
        Destroy(movePoint.gameObject);
        Destroy(gameObject);
    }

    IEnumerator MoveDelay()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelayTime);
        movePoint.position = transform.position;
        canMove = true;
    }

    IEnumerator UnearthTile(Vector3Int tileToUnearth)
    {
        canMove = false;
        yield return new WaitForSeconds(unearthTime);
        tileManager.Unearth(tileToUnearth);
        movePoint.position = transform.position;
        canMove = true;
    }
}