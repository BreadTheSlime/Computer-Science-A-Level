using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    [SerializeField] int existenceRange;
    [SerializeField] Vector2Int moveDirection;

    [SerializeField] Transform movePoint;
    [SerializeField] float moveDelayTime;
    [SerializeField] bool onTileLastFrame;
    public bool movepointOnEnemy;

    TileManager tileManager;
    TrackerPathfinding trackerPathfinding;
    [SerializeField] Vector3Int targetTilePosition;
    [SerializeField] float unearthTime;
    [SerializeField] GameObject gameManager;
    CollisionManager collisionManager;
    [SerializeField] Transform player;

    void Awake()
    {
        movePoint.parent = null;
        player = GameObject.Find("Player").transform;
        gameManager = GameObject.Find("Game Manager");
        tileManager = FindObjectOfType<TileManager>();
        trackerPathfinding = FindObjectOfType<TrackerPathfinding>();
        collisionManager = gameManager.GetComponent<CollisionManager>();
        canMove = true;
    }

    void Update()
    {

        // Destroy the enemy if it is far away from the player
        if (player && Vector3.Distance(transform.position, player.transform.position) > existenceRange)
        {
            Destroy(gameObject);
        }

        // If the enemy was on a tile last frame and is not currently moving, delay their movement
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
            movepointOnEnemy = true;
        }
        else
        {
            movepointOnEnemy = false;
        }

        if (canMove)
        {
            MoveEnemy();
            if (movepointOnEnemy)
            {
                moveDirection = trackerPathfinding.findPath(gameObject, player.gameObject);
                movePoint.transform.position += new Vector3(moveDirection.x, moveDirection.y);
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

    void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            collisionManager.scoreCount += 50;
            collisionManager.UpdateGUI();
            Destroy(collision.gameObject);
            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
    }
}