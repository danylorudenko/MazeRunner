using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    protected LabyrinthManager owner;

    protected GameObject gameObj;
    protected Animator animatorComponent;

    protected Coordinate currentCoordinate;

    protected Unit(LabyrinthManager owner, Coordinate currentCoordinate)
    { 
        this.owner = owner;
        this.currentCoordinate = currentCoordinate;
    }

    /// <summary>
    /// Spawning unit in currentCoordinate in owner Labyrinth
    /// </summary>
    protected abstract void Spawn();

    /// <summary>
    /// Current array coordinate of the unit
    /// </summary>
    /// <returns>Current array coordinate of the unit</returns>
    public virtual Coordinate GetCurrentCoordinate()
    {
        return currentCoordinate;
    }

    /// <summary>
    /// GameObject which represents this logical unit
    /// </summary>
    /// <returns>GameObject in scene</returns>
    public virtual GameObject GetGameObject()
    {
        return gameObj;
    }

    /// <summary>
    /// Checking of coordinate intersects current object's coordinate
    /// </summary>
    /// <param name="coordinate">Checked coordinate</param>
    /// <returns>"true" - coordinate intersects, "false" = doesn't</returns>
    public virtual bool IsIntersecting(Coordinate coordinate)
    {
        if(currentCoordinate.IsEqual(coordinate)) {
            return true;
        }
        else {
            return false;
        }
    }

    public abstract void PlayerIntersectionBehaviour(List<Unit> unitsInLabyrinth);

    public abstract void DestroyGameObject();
}