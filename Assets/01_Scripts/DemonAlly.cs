using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAlly : MonoBehaviour
{
    public float moveRadius = 5f;         // The radius within which the ally can move
    public float moveSpeed = 2f;           // Speed of the ally movement
    public GameObject[] itemsPrefabs;          // The item that the ally will drop
    public Transform dropPoint;            // The position where the item will be dropped

    private Vector3 targetPosition;        // The next position the ally will move to
    private bool isMoving = false;         // Whether the ally is currently moving

    void Start()
    {
        Destroy(gameObject, 15f);
        // Start the random movement and item dropping coroutines
        StartCoroutine(MoveRandomly());
        StartCoroutine(DropItemRoutine());
    }

    // Coroutine for random movement
    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // Randomly decide whether to move or stay in position
            if (Random.Range(0, 2) == 0)
            {
                // Stay in the current position
                isMoving = false;
            }
            else
            {
                // Move to a random position within the moveRadius
                Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
                randomDirection += transform.position;
                randomDirection.y = transform.position.y; // Keep the same Y level for 2D ground
                targetPosition = randomDirection;
                isMoving = true;
            }

            // Move to the new target position if isMoving is true
            while (isMoving && Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Pause before deciding the next move
            yield return new WaitForSeconds(Random.Range(2, 5)); // Wait for 2-5 seconds
        }
    }

    // Coroutine to drop an item every 10 seconds
    IEnumerator DropItemRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);  // Wait for 10 seconds

            if (itemsPrefabs.Length > 0 && dropPoint != null)
            {
                // Randomly select an item from the list
                int randomItemIndex = Random.Range(0, itemsPrefabs.Length);
                GameObject selectedItem = itemsPrefabs[randomItemIndex];

                // Drop the randomly selected item at the drop point
                Instantiate(selectedItem, dropPoint.position, Quaternion.identity);
            }
        }
    }
}
