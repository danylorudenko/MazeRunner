public class AINavigationController
{
    private Enemy owner;
    private System.Random randomizer;

    private Direction previousDirection;

    public AINavigationController(Enemy owner)
    {
        this.owner = owner;
        randomizer = new System.Random();
        previousDirection = Direction.INVALID;
    }

    /// <summary>
    /// Using AI to get suitable direction for situation
    /// </summary>
    /// <param name="newDirection">Reference to resulting direction</param>
    public void Use()
    {
        owner.SetNewDirection(PickRandomDirection());
        //if (References.labyrinthManager.player.GetCoinsCollectedCount() < 20) {
        //    owner.SetNewDirection(PickRandomDirection());
        //}
        //else {
        //    owner.SetNewDirection(WhereToPlayer());
        //}
    }

    /// <summary>
    /// Checking if owner has paths on the left or on the right
    /// </summary>
    /// <returns>"true" - owner has other paths, "false"</returns>
    private bool IsOnCrossRoad()
    {
        Direction leftHandDirection = FindLeftHandDirection(owner.GetCurrentDirection());
        Direction rightHandDirecton = FindRightHandDirection(owner.GetCurrentDirection());
        if (!CheckWall(leftHandDirection, 1)) {
            return true;
        }
        if (!CheckWall(rightHandDirecton, 1)) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Finding best here&now direction to get to the player
    /// </summary>
    /// <returns></returns>
    private Direction WhereToPlayer()
    {
        Direction effectiveDirection = FindEffectiveDirection();
        if (!CheckWall(effectiveDirection, 1) && effectiveDirection != previousDirection) {
            previousDirection = Direction.INVALID;
            return effectiveDirection; // вроде все ок
        }
        else {
            return FindDirectionByRightHandRule();
        }
    }

    /// <summary>
    /// Finding the direction which will result into moving closer to the target
    /// </summary>
    /// <returns></returns>
    private Direction FindEffectiveDirection()
    {
        Coordinate playerCoordinate = References.labyrinthManager.GetPlayerCoordinate();
        Coordinate currentCoordinate = owner.GetCurrentCoordinate();

        int xDistance = playerCoordinate.x - currentCoordinate.x;
        int yDistance = playerCoordinate.y - currentCoordinate.y;

        Direction verticalDirection = Direction.INVALID;
        Direction horizontalDirection = Direction.INVALID;

        if (yDistance > 0) {
            verticalDirection = Direction.North;
        }
        else if (yDistance < 0) {
            verticalDirection = Direction.South;
        }

        if (xDistance > 0) {
            horizontalDirection = Direction.East;
        }
        else if (xDistance < 0) {
            horizontalDirection = Direction.West;
        }

        Direction chosenDirection = Direction.INVALID;
        for (int i = 0; chosenDirection == Direction.INVALID; i++) {
            if (CheckWall(verticalDirection, i)) {
                chosenDirection = horizontalDirection;
                break;
            }
            if (CheckWall(horizontalDirection, i)) {
                chosenDirection = verticalDirection;
                break;
            }
        }

        return chosenDirection;
    }

    /// <summary>
    /// Finding valid direction which suits the right-hand rule navigation
    /// </summary>
    /// <returns></returns>
    private Direction FindDirectionByRightHandRule()
    {
        Direction currentOwnerDirection = owner.GetCurrentDirection();
        Direction rightHandDirection = FindRightHandDirection(currentOwnerDirection);
        Direction leftHandDirection = FindLeftHandDirection(currentOwnerDirection);
        Direction inversedDirection = InverseDirection(currentOwnerDirection);

        if (!CheckWall(rightHandDirection, 1)) {
            previousDirection = rightHandDirection;
            return rightHandDirection;
        }
        else if (CheckWall(rightHandDirection, 1) && !CheckWall(currentOwnerDirection, 1)) {
            previousDirection = currentOwnerDirection;
            return currentOwnerDirection;
        }
        else if (CheckWall(rightHandDirection, 1) && CheckWall(currentOwnerDirection, 1) && !CheckWall(leftHandDirection, 1)) {
            previousDirection = leftHandDirection;
            return leftHandDirection;
        }
        else if (CheckWall(rightHandDirection, 1) && CheckWall(currentOwnerDirection, 1) && CheckWall(leftHandDirection, 1)) {
            previousDirection = inversedDirection;
            return inversedDirection;
        }
        return Direction.INVALID;
    }

    /// <summary>
    /// Retuning passed direction turned to the right
    /// </summary>
    /// <param name="frontDirection">Direction to turn</param>
    /// <returns></returns>
    private Direction FindRightHandDirection(Direction frontDirection)
    {
        switch (frontDirection) {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
        }
        return Direction.INVALID;
    }

    /// <summary>
    /// Returning passed direction turned to the left
    /// </summary>
    /// <param name="frontDirection">Direction to turn</param>
    /// <returns></returns>
    private Direction FindLeftHandDirection(Direction frontDirection)
    {
        switch (frontDirection) {
            case Direction.North:
                return Direction.West;
            case Direction.West:
                return Direction.South;
            case Direction.South:
                return Direction.East;
            case Direction.East:
                return Direction.North;
        }
        return Direction.INVALID;
    }

    /// <summary>
    /// Returning iversed direction
    /// </summary>
    /// <param name="frontDirection">Front direction</param>
    /// <returns>Direction value</returns>
    private Direction InverseDirection(Direction frontDirection)
    {
        switch (frontDirection) {
            case Direction.North:
                return Direction.South;
            case Direction.West:
                return Direction.East;
            case Direction.South:
                return Direction.North;
            case Direction.East:
                return Direction.West;
        }
        return Direction.INVALID;
    }

    /// <summary>
    /// Returning true if a cell in passed direction in passed steps is a wall
    /// </summary>
    /// <returns>"true" - cell is wall, "false" - cell is path</returns>
    private bool CheckWall(Direction direction, int stepCount)
    {
        switch (direction) {
            case Direction.North:
                if (References.labyrinthManager.AccessCell(owner.GetCurrentCoordinate().x, owner.GetCurrentCoordinate().y + stepCount).IsWall()) {
                    return true;
                }
                break;
            case Direction.South:
                if (References.labyrinthManager.AccessCell(owner.GetCurrentCoordinate().x, owner.GetCurrentCoordinate().y - stepCount).IsWall()) {
                    return true;
                }
                break;
            case Direction.East:
                if (References.labyrinthManager.AccessCell(owner.GetCurrentCoordinate().x + stepCount, owner.GetCurrentCoordinate().y).IsWall()) {
                    return true;
                }
                break;
            case Direction.West:
                if (References.labyrinthManager.AccessCell(owner.GetCurrentCoordinate().x - stepCount, owner.GetCurrentCoordinate().y).IsWall()) {
                    return true;
                }
                break;
        }
        return true;
    }

    /// <summary>
    /// Picking absolutely random direction
    /// </summary>
    /// <returns>Random direction</returns>
    private Direction PickRandomDirection()
    {
        Direction[] directions = { Direction.North, Direction.South, Direction.East, Direction.West };
        int index = randomizer.Next(0, directions.Length);
        return directions[index];
    }
}