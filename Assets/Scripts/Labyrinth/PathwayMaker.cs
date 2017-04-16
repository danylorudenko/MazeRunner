using System;
using System.Collections.Generic;
using UnityEngine;

public class PathwayMaker
{
    /// <summary>
    /// Cell grid through which PathwayMaker will make pathways
    /// </summary>
    private Cell[,] cellGrid;

    /// <summary>
    /// Creating of PathwayMaker. Suits only the passed cellGrid
    /// </summary>
    /// <param name="cellGrid">the cell grid for PathwayMaker to work with</param>
    public PathwayMaker(Cell[,] cellGrid)
    {
        this.cellGrid = cellGrid;
    }

    /// <summary>
    /// Main method for generating labyrinth paths
    /// </summary>
    public void GenerateLabyrinthByPrim()
    {
        List<Coordinate> frontiers = new List<Coordinate>();
        System.Random randomizer = new System.Random();
        Coordinate currentPathCoordinate = FindStartingCellCoordinates();
        cellGrid[currentPathCoordinate.x, currentPathCoordinate.y].SetPath();
        FindFrontiers(currentPathCoordinate, frontiers);

        Direction direction;
        while (frontiers.Count > 0) {
            currentPathCoordinate = GetRandomFrontier(randomizer, frontiers);
            direction = GetRandomDirectionToPath(currentPathCoordinate, randomizer);
            TryBreakWall(direction, currentPathCoordinate, frontiers);
        }
    }

    /// <summary>
    /// Finding walls on distance 2 which could have potentially become paths
    /// </summary>
    /// <param name="coordinate">Coordinate to find neighbours</param>
    /// <param name="frontierList">List for found neighbours</param>
    private void FindFrontiers(Coordinate coordinate, List<Coordinate> frontierList)
    {
        int x = coordinate.x;
        int y = coordinate.y;
        try {
            if (!cellGrid[x, y + 2].IsPath()) {
                frontierList.Add(new Coordinate(x, y + 2));
            }
        }
        catch (IndexOutOfRangeException) { }

        try {
            if (!cellGrid[x, y - 2].IsPath()) {
                frontierList.Add(new Coordinate(x, y - 2));
            }
        }
        catch (IndexOutOfRangeException) { }
        try {
            if (!cellGrid[x + 2, y].IsPath()) {
                frontierList.Add(new Coordinate(x + 2, y));
            }
        }
        catch(IndexOutOfRangeException) { }

        try {
            if (!cellGrid[x - 2, y].IsPath()) {
                frontierList.Add(new Coordinate(x - 2, y));
            }
        }
        catch (IndexOutOfRangeException) { }
    }
    
    /// <summary>
    /// Getting random coodrinate from the from the passed List<Coordinate></Coordinate>
    /// </summary>
    /// <param name="randomizer">Randomizer to pick a random</param>
    /// <param name="frontiersList">List where potential paths are stored</param>
    /// <returns></returns>
    private Coordinate GetRandomFrontier(System.Random randomizer, List<Coordinate> frontiersList)
    {
        int index = randomizer.Next(0, frontiersList.Count);
        return frontiersList[index];
    }

    /// <summary>
    /// Breaking wall in the passed direction if it's neighbouring less than 3 paths
    /// </summary>
    /// <param name="direcition">Direction where wall should be destroyed</param>
    /// <param name="frontierCoordinate">Coordinate of cell neighbouring the wall</param>
    /// <param name="frontiersList">List where new potential paths are added and now-paths are destroyed</param>
    private void TryBreakWall(Direction direcition, Coordinate frontierCoordinate, List<Coordinate> frontiersList)
    {
        try {
            int x = frontierCoordinate.x;
            int y = frontierCoordinate.y;
            switch (direcition) {
                case Direction.INVALID:
                    frontiersList.Remove(frontierCoordinate);
                    break;

                case Direction.North:
                    y++;
                    if(!IsWallNeighbourTo3Paths(new Coordinate(x, y))) {
                        cellGrid[frontierCoordinate.x, frontierCoordinate.y].SetPath();
                        cellGrid[x, y].SetPath();
                        y++;
                        FindFrontiers(frontierCoordinate, frontiersList);
                    }
                    frontiersList.Remove(frontierCoordinate);
                    break;

                case Direction.South:
                    y--;
                    if (!IsWallNeighbourTo3Paths(new Coordinate(x, y))) {
                        cellGrid[frontierCoordinate.x, frontierCoordinate.y].SetPath();
                        cellGrid[x, y].SetPath();
                        y--;
                        FindFrontiers(frontierCoordinate, frontiersList);
                    }
                    frontiersList.Remove(frontierCoordinate);
                    break;

                case Direction.East:
                    x++;
                    if (!IsWallNeighbourTo3Paths(new Coordinate(x, y))) {
                        cellGrid[frontierCoordinate.x, frontierCoordinate.y].SetPath();
                        cellGrid[x, y].SetPath();
                        x++;
                        FindFrontiers(frontierCoordinate, frontiersList);
                    }
                    frontiersList.Remove(frontierCoordinate);
                    break;

                case Direction.West:
                    x--;
                    if (!IsWallNeighbourTo3Paths(new Coordinate(x, y))) {
                        cellGrid[frontierCoordinate.x, frontierCoordinate.y].SetPath();
                        cellGrid[x, y].SetPath();
                        x--;
                        FindFrontiers(frontierCoordinate, frontiersList);
                    }
                    frontiersList.Remove(frontierCoordinate);
                    break;
            }
        }
        catch (IndexOutOfRangeException) { }
    }

    /// <summary>
    /// Checking if wall has more than 3 path neighbours
    /// </summary>
    /// <param name="coordinate">Coordinate of the wall</param>
    /// <returns>"true" - 3 and more path neighbours, "false" - less than 3 path neighbours</returns>
    private bool IsWallNeighbourTo3Paths(Coordinate coordinate)
    {
        int pathCounter = 0;
        int x = coordinate.x;
        int y = coordinate.y;

        if (cellGrid[x, y + 1].IsPath()) {
            pathCounter++;
        }
        if (cellGrid[x, y - 1].IsPath()) {
            pathCounter++;
        }
        if (cellGrid[x + 1, y].IsPath()) {
            pathCounter++;
        }
        if (cellGrid[x - 1, y].IsPath()) {
            pathCounter++;
        }

        if(pathCounter >= 3) {
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// Finding random direction to the nearest paths form the 
    /// </summary>
    /// <param name="frontierCoordinate">Coordinate to find paths in distance 2</param>
    /// <param name="randomizer">Randomizer to pick random direction</param>
    /// <returns>Random direction to one of found paths</returns>
    private Direction GetRandomDirectionToPath(Coordinate frontierCoordinate, System.Random randomizer)
    {
        Direction[] directions = { Direction.North, Direction.South, Direction.East, Direction.West };
        List<Direction> directionsWithPaths = new List<Direction>();
        for(int i = 0; i < directions.Length; i++) {
            if (IsPathAhead(directions[i], frontierCoordinate)) {
                directionsWithPaths.Add(directions[i]);
            }
        }

        if(directionsWithPaths.Count >= 3) {
            return Direction.INVALID;
        }
        else {
            if(directionsWithPaths.Count != 0) {
                int directionIndex = randomizer.Next(0, directionsWithPaths.Count);
                return directionsWithPaths[directionIndex];
            }
            else {
                return Direction.INVALID;
            }
        }
    }

    /// <summary>
    /// Checking if frontier has a path in 2 blocks in passed direction
    /// </summary>
    /// <param name="direction">Direction where to check</param>
    /// <param name="frontierCoordinate">Coordinate of frontier to check</param>
    /// <returns>"true" - path detected in distance 2, "false" - no path detected</returns>
    private bool IsPathAhead(Direction direction, Coordinate frontierCoordinate)
    {
        int x = frontierCoordinate.x;
        int y = frontierCoordinate.y;
        try {
            switch (direction) {
                case Direction.North:
                    y += 2;
                    return cellGrid[x, y].IsPath();

                case Direction.South:
                    y -= 2;
                    return cellGrid[x, y].IsPath();

                case Direction.East:
                    x += 2;
                    return cellGrid[x, y].IsPath();

                case Direction.West:
                    x -= 2;
                    return cellGrid[x, y].IsPath();

                default:
                    return false;
            }
        }
        catch (IndexOutOfRangeException) {
            return false;
        }
    }

    /// <summary>
    /// Finding valid random staring point
    /// </summary>
    /// <returns>2D coordinate position in cell array</returns>
    private Coordinate FindStartingCellCoordinates()
    {
        int width = cellGrid.GetLength(0);
        int height = cellGrid.GetLength(1);

        System.Random randomizer = new System.Random();

        // We parameters we don't set bounding values of the grid array, cause we already have walls there
        int x = 0;
        int y = 0;

        while (cellGrid[x, y].IsUnbreakable()) {
            x = randomizer.Next(1, width - 2);
            y = randomizer.Next(1, height - 2);
            if (x % 2 != 1) {
                x++;
            }
            if (y % 2 != 1) {
                y++;
            }
        }
        return new Coordinate(x, y);
    }
}