using UnityEngine;
using System.Collections;

public abstract class Enemy : DynamicUnit
{
    /// <summary>
    /// Multiplier that affects speed of all enemies
    /// </summary>
    public static float SpeedMultiplier
    {
        set
        {
            if(value < 0) {
                speedMultiplier = 1;
            }
            else {
                speedMultiplier = value;
            }
        }
        get
        {
            return speedMultiplier;
        }
    }
    protected static int enemyCount = 0;
    private static float speedMultiplier;

    protected AINavigationController AIController;
    
    public Enemy(LabyrinthManager owner, Coordinate currentCoordinate) : base(owner, currentCoordinate)
    {
        AIController = new AINavigationController(this);
        speedMultiplier = 1f;
    }
    
    /// <summary>
    /// GameObject destruction
    /// </summary>
    public override void DestroyGameObject()
    {
        if (gameObj != null) {
            GameObject.Destroy(gameObj);
            enemyCount--;
        }
    }

    /// <summary>
    /// Accessing to the cell in the grid where enemy belongs
    /// </summary>
    /// <param name="x">X coordinate of the cell</param>
    /// <param name="y">Y coordinate of the cell</param>
    /// <returns>Cell from enemy world</returns>
    public Cell GetOwnerCell(int x, int y)
    {
        return owner.AccessCell(x, y);
    }
    
    /// <summary>
    /// Getting current direction of enemy
    /// </summary>
    /// <returns>Direction enum</returns>
    public Direction GetCurrentDirection()
    {
        return currentDirection;
    }
    
    /// <summary>
    /// Setting new movement direction for the enemy instance
    /// </summary>
    /// <param name="newDirection">New direction to move</param>
    public void SetNewDirection(Direction newDirection)
    {
        currentDirection = newDirection;
    }

    /// <summary>
    /// Pacman-styled movement control loop
    /// </summary>
    /// <returns>Coroutine</returns>
    public IEnumerator MovementLoop()
    {
        while (true) {
            AIController.Use();
            Move();
            Coordinate newCoordinate = GetCurrentCoordinate();
            Vector3 newPosition = owner.AccessCell(newCoordinate.x, newCoordinate.y).GetGameObjectPosition();
            yield return owner.StartCoroutine(VisualizeMove(newPosition));
        }
    }
}