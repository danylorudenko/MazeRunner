using System;
using System.Collections.Generic;
using UnityEngine;

public class Coin : StaticUnit
{
    private static int coinCount = 0;

    public Coin(LabyrinthManager owner, Coordinate currentCoordinate) : base(owner, currentCoordinate)
    {
        Spawn();
    }

    /// <summary>
    /// Coins created
    /// </summary>
    /// <returns></returns>
    public static int GetCoinCount()
    {
        return coinCount;
    }

    /// <summary>
    /// Spawning coin
    /// </summary>
    protected override void Spawn()
    {
        if(gameObj == null) {
            Coin.coinCount++;
            Vector3 spawnPosition = owner.AccessCell(currentCoordinate.x, currentCoordinate.y).GetGameObjectPosition();
            spawnPosition.z -= 0.5f;
            gameObj = GameObject.Instantiate(owner.coinPrefab, spawnPosition, Quaternion.identity, owner.transform);
            animatorComponent = gameObj.GetComponent<Animator>();
        }
    }

    /// <summary>
    /// Destroying coin GameObject
    /// </summary>
    public override void DestroyGameObject()
    {
        if (gameObj != null) {
            GameObject.Destroy(gameObj);
            coinCount--;
        }
    }

    /// <summary>
    /// Player intersection behaviour
    /// </summary>
    public override void PlayerIntersectionBehaviour(List<Unit> unitsInLabyrinth)
    {
        References.labyrinthManager.player.AddCollectedCoin();
        Enemy.SpeedMultiplier += 0.5f;
        DestroyGameObject();
        unitsInLabyrinth.Remove(this);
    }
}