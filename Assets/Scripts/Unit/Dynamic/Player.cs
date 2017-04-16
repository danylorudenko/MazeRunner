using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : DynamicUnit
{
    private int coinsCollected;
    
    /// <summary>
    /// Logical construction of player entity. No visualization
    /// </summary>
    /// <param name="newCoordinate">Array coordinate of cell to swawn on</param>
    /// <param name="owner">LabyrinthManager, where player is</param>
    public Player(LabyrinthManager owner, Coordinate currentCoordinate) : base(owner, currentCoordinate)
    {
        coinsCollected = 0;
        speed = 4;
        currentDirection = Direction.INVALID;
        newDirection = Direction.INVALID;
        Spawn();
    }
    
    /// <summary>
    /// Getting nubmer of coins collected by player
    /// </summary>
    /// <returns>Collected coins count</returns>
    public int GetCoinsCollectedCount()
    {
        return coinsCollected;
    }

    /// <summary>
    /// Setting count of cellected coins to zero
    /// </summary>
    public void ResetCoinsCollectedCount()
    {
        coinsCollected = 0;
    }

    /// <summary>
    /// Incrementing count of collected coins
    /// </summary>
    public void AddCollectedCoin()
    {
        coinsCollected++;
    }

    /// <summary>
    /// Starting movement logic
    /// </summary>
    public void StartMovementLoop()
    {
        owner.StartCoroutine(PlayerMovementLoop());
    }

    /// <summary>
    /// Writing next direction according to the user input
    /// Should be called in Update()
    /// </summary>
    public void CheckUserInput()
    {
        bool WInput = Input.GetKeyDown(KeyCode.W);
        bool SInput = Input.GetKeyDown(KeyCode.S);
        bool AInput = Input.GetKeyDown(KeyCode.A);
        bool DInput = Input.GetKeyDown(KeyCode.D);

        if (WInput) {
            newDirection = Direction.North;
        }
        else if (SInput) {
            newDirection = Direction.South;
        }
        else if (AInput) {
            newDirection = Direction.West;
        }
        else if (DInput) {
            newDirection = Direction.East;
        }
    }

    /// <summary>
    /// Performing player pacman-like movement
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayerMovementLoop()
    {
        while (true) {
            Move();
            CheckIntersections();
            Coordinate newCoordinate = GetCurrentCoordinate();
            Vector3 newPosition = owner.AccessCell(newCoordinate.x, newCoordinate.y).GetGameObjectPosition();
            yield return owner.StartCoroutine(VisualizeMove(newPosition));
        }
    }

    /// <summary>
    /// Smooth visualization of movement
    /// </summary>
    /// <param name="newPosition">Where to move</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator VisualizeMove(Vector3 newPosition)
    {
        newPosition.z -= 0.5f;
        TurnGameObjectToDirection();
        animatorComponent.SetBool("IsMoving", true);
        while (gameObj.transform.position != newPosition) {
            gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, newPosition, speed * Time.deltaTime);
            yield return null;
        }
        animatorComponent.SetBool("IsMoving", false);
        yield break;
    }

    /// <summary>
    /// Checking intersections of all objects with the player
    /// </summary>
    private void CheckIntersections()
    {
        for (int i = 0; i < owner.GetUnitList().Count; i++) {
            try {
                if (owner.GetUnitList()[i].IsIntersecting(currentCoordinate)) {
                    owner.GetUnitList()[i].PlayerIntersectionBehaviour(owner.GetUnitList());
                }
            }
            catch (IndexOutOfRangeException) {
                break;
            }
        }
    }

    /// <summary>
    /// Spawning player on cell
    /// </summary>
    /// <param name="coordinate">Array coordinate of the cell</param>
    protected override void Spawn()
    {
        Vector3 spawnPosition = owner.AccessCell(currentCoordinate.x, currentCoordinate.y).GetGameObjectPosition();
        spawnPosition.z -= 0.5f;
        gameObj = GameObject.Instantiate(owner.playerPrefab, spawnPosition, Quaternion.identity, owner.transform);
        animatorComponent = gameObj.GetComponent<Animator>();
    }

    /// <summary>
    /// Intersection logic
    /// </summary>
    /// <param name="unitsInLabyrinth"></param>
    public override void PlayerIntersectionBehaviour(List<Unit> unitsInLabyrinth) { }

    /// <summary>
    /// Physical destruction logic
    /// </summary>
    public override void DestroyGameObject()
    {
        
    }
}