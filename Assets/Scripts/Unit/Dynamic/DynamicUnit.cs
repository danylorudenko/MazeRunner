using System.Collections;
using UnityEngine;

public abstract class DynamicUnit : Unit
{
    protected Direction currentDirection;
    protected Direction newDirection;

    protected float speed;

    protected DynamicUnit(LabyrinthManager owner, Coordinate currentCoordinate) : base(owner, currentCoordinate) { }

    /// <summary>
    /// Logical movement of dynamic through cell array
    /// </summary>
    public virtual void Move()
    {
        if (IsDirectionChanged()) {
            if (IsStepPossible(newDirection)) {
                MakeStep(newDirection);
                currentDirection = newDirection;
                return;
            }
            else {
                if (IsStepPossible(currentDirection)) {
                    MakeStep(currentDirection);
                    return;
                }
                else {
                    return;
                }
            }

        }
        else {
            if (IsStepPossible(newDirection)) {
                MakeStep(currentDirection);
                return;
            }
            else {
                return;
            }
        }
    }

    /// <summary>
    /// Movement visualization according to the unit speed
    /// </summary>
    /// <param name="newPosition">Position, where to move</param>
    /// <returns>Coroutine</returns>
    public abstract IEnumerator VisualizeMove(Vector3 newPosition);

    /// <summary>
    /// Turning unit's gameObject accoridng to the current direction. Leaving rotation in North and South cases
    /// </summary>
    protected virtual void TurnGameObjectToDirection()
    {
        switch (currentDirection) {
            case Direction.East:
                gameObj.transform.rotation = Quaternion.Euler(Vector3.zero);
                break;

            case Direction.West:
                gameObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                break;

            default:
                return;
        }
    }

    /// <summary>
    /// Checking if one step is possible in the passed direction
    /// </summary>
    /// <param name="direction">Direction to check</param>
    /// <returns>"true" - step is possible, "false" - not</returns>
    protected virtual bool IsStepPossible(Direction direction)
    {
        switch (direction) {
            case Direction.INVALID:
                return false;

            case Direction.North:
                if (owner.AccessCell(currentCoordinate.x, currentCoordinate.y + 1).IsWall()) {
                    return false;
                }
                break;

            case Direction.South:
                if (owner.AccessCell(currentCoordinate.x, currentCoordinate.y - 1).IsWall()) {
                    return false;
                }
                break;

            case Direction.East:
                if (owner.AccessCell(currentCoordinate.x + 1, currentCoordinate.y).IsWall()) {
                    return false;
                }
                break;

            case Direction.West:
                if (owner.AccessCell(currentCoordinate.x - 1, currentCoordinate.y).IsWall()) {
                    return false;
                }
                break;
        }

        return true;
    }

    /// <summary>
    /// Changing logical coordinates according to the passed direction
    /// Direction should be checked!
    /// </summary>
    /// <param name="direction">Directon in which coordinates should bo moved</param>
    protected virtual void MakeStep(Direction direction)
    {
        switch (direction) {
            case Direction.North:
                currentCoordinate.y++;
                break;

            case Direction.South:
                currentCoordinate.y--;
                break;

            case Direction.East:
                currentCoordinate.x++;
                break;

            case Direction.West:
                currentCoordinate.x--;
                break;
        }
    }

    /// <summary>
    /// Checking newDirection is set and not used
    /// </summary>
    /// <returns>"true" - newDirections differs from current direction, newDirection was not used</returns>
    protected virtual bool IsDirectionChanged()
    {
        return currentDirection != newDirection;
    }
}
