using UnityEngine;

public class Cell
{
    /// <summary>
    /// Counter for cell's were visited by the PathwayMaker
    /// </summary>
    private static int visitedCellsCount;

    /// <summary>
    /// If "true" then then the cell is visited by the algorythm, "false" - not
    /// </summary>
    private bool bPath;

    /// <summary>
    /// If "true" the cell is a wall, "false" - ground
    /// </summary>
    private bool bWall;

    /// <summary>
    /// If "true" - state of block can't be changed
    /// </summary>
    private bool bUnbreakable;

    /// <summary>
    /// Cell representation in the scene
    /// </summary>
    private GameObject gameObj;

    /// <summary>
    /// Creates new wall cell
    /// </summary>
    public Cell()
    {
        bPath = false;
        bWall = true;
        bUnbreakable = false;
    }

    /// <summary>
    /// GameObject that represents this cell in the scene
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        return gameObj;
    }

    /// <summary>
    /// Count of cells visited by the PatywayFinder algorythm
    /// </summary>
    /// <returns></returns>
    public static int GetVisitedCellsCount()
    {
        return visitedCellsCount;
    }

    /// <summary>
    /// Reseting visited cells counter to zero
    /// </summary>
    public static void ResetVisitedCellsCounter()
    {
        visitedCellsCount = 0;
    }
    
    /// <summary>
    /// Returns if the cell is marked as visited
    /// </summary>
    /// <returns>"bVisited" marker value</returns>
    public bool IsPath()
    {
        return bPath;
    }

    /// <summary>
    /// Visiting cell, destroying wall
    /// </summary>
    public void SetPath()
    {
        if (!IsUnbreakable()) {
            bWall = false;
            bPath = true;
        }
    }

    /// <summary>
    /// Returns if the cell is a wall cell
    /// </summary>
    /// <returns>bWall value</returns>
    public bool IsWall()
    {
        return bWall;
    }

    /// <summary>
    /// Set's cell's state to Unbreakable, automatically makes it as bWall and bVisited
    /// </summary>
    public void SetUnbreakable()
    {
        bUnbreakable = true;
        bWall = true;
        bPath = false;
    }

    /// <summary>
    /// Cell gameObject world position
    /// </summary>
    /// <returns>Vector3 world position</returns>
    public Vector3 GetGameObjectPosition()
    {
        if(gameObj != null) {
            return gameObj.transform.position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Is the cell is unbreakable
    /// </summary>
    /// <returns>bUnbreakable value</returns>
    public bool IsUnbreakable()
    {
        return bUnbreakable;
    }

    /// <summary>
    /// Instantiating GameObject to represent the cell in the scene
    /// </summary>
    /// <param name="x">X position of the cell</param>
    /// <param name="y">Y position of the cell</param>
    /// <param name="startPosition">Starting position according to which cell gameObject is generated</param>
    public void Visualize(int x, int y, Vector3 startPosition)
    {
        if (IsWall() || IsUnbreakable()) {
            gameObj = (GameObject)GameObject.Instantiate(References.labyrinthManager.wallPrefab, new Vector3(x, y, 0) + startPosition, Quaternion.Euler(Vector3.zero));
        }
        else {
            gameObj = (GameObject)GameObject.Instantiate(References.labyrinthManager.groundPrefab, new Vector3(x, y, 0) + startPosition, Quaternion.Euler(Vector3.zero));
        }
    }
}
