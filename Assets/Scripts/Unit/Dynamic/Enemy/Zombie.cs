using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public Zombie(LabyrinthManager owner, Coordinate currentCoordinate) : base(owner, currentCoordinate)
    {
        currentDirection = Direction.North;
        speed = 0.3f;
        Spawn();
    }
    
    public override void PlayerIntersectionBehaviour(List<Unit> unitsInLabyrinth)
    {
        References.labyrinthManager.GameOver();
    }
    
    /// <summary>
    /// Visualization of movement for zombie
    /// </summary>
    /// <param name="newPosition">Taget position</param>
    /// <returns>Coroitine</returns>
    public override IEnumerator VisualizeMove(Vector3 newPosition)
    {
        newPosition.z -= 0.5f;
        TurnGameObjectToDirection();
        animatorComponent.SetBool("IsWalking", true);
        while (gameObj.transform.position != newPosition) {
            gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, newPosition, speed * SpeedMultiplier * Time.deltaTime);
            yield return null;
        }
        animatorComponent.SetBool("IsWalking", false);
        yield break;
    }
    
    /// <summary>
    /// Logic for zombie spawning. Staring movement
    /// </summary>
    protected override void Spawn()
    {
        if (gameObj == null) {
            enemyCount++;
            Vector3 spawnPosition = owner.AccessCell(currentCoordinate.x, currentCoordinate.y).GetGameObjectPosition();
            spawnPosition.z -= 0.5f;
            gameObj = GameObject.Instantiate(owner.zombiePrefab, spawnPosition, Quaternion.identity, owner.transform);
            animatorComponent = gameObj.GetComponent<Animator>();
            owner.StartCoroutine(MovementLoop());
        }
    }

    /// <summary>
    /// Turning unit's gameObject accoridng to the current direction. Leaving rotation in North and South cases
    /// </summary>
    protected override void TurnGameObjectToDirection()
    {
        switch (currentDirection) {
            case Direction.East:
                gameObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                break;

            case Direction.West:
                gameObj.transform.rotation = Quaternion.Euler(Vector3.zero);
                break;

            default:
                return;
        }
    }
}