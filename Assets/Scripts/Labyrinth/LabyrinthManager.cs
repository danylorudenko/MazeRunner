using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LabyrinthManager : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private int distanceBetweenCells;

    [SerializeField]
    private float coinSpawnPeriod;

    /// <summary>
    /// Distance between cells' gameObjects
    /// </summary>
    public int DistanceBetweenCells
    {
        set
        {
            distanceBetweenCells = (int)value;
        }
        get
        {
            return distanceBetweenCells;
        }
    }

    public GameObject wallPrefab;
    public GameObject groundPrefab;
    public GameObject playerPrefab;
    public GameObject coinPrefab;
    public GameObject zombiePrefab;
    public GameObject mummyPrefab;

    private GameObject cameraObject;
    private GameObject cameraTargetObject;

    public GameObject gameOverPanel;

    public Player player;

    /// <summary>
    /// 2-dimentional array of cells which are used to build random labytinth
    /// </summary>
    private Cell[,] cellGrid;

    /// <summary>
    /// List if units which are present in this labyrinth
    /// Player is not included
    /// </summary>
    private List<Unit> unitsInLabyrinth;

    /// <summary>
    /// List of all paths in labyrinth
    /// </summary>
    private List<Coordinate> pathsCoordinates;

    private void Awake()
    {
        AwakeInitialization();
    }

    private void Start()
    {
        DistanceBetweenCells = 1;
        GenerateLabyrinth();

        System.Random randomizer = new System.Random();

        StartCoroutine(StartGeneratingCoins(randomizer));

        InitializePlayer(randomizer);
        StartCoroutine(StartGeneratingEnemies(randomizer));
    }

    private void Update()
    {
        if (player != null) {
            player.CheckUserInput();
        }
        CenterCamera();
    }

    /// <summary>
    /// Labyrinth base initialization
    /// </summary>
    private void AwakeInitialization()
    {
        References.labyrinthManager = this;
        cameraObject = Camera.main.gameObject;
        unitsInLabyrinth = new List<Unit>();
        pathsCoordinates = new List<Coordinate>();
        SafeCreateCells();
    }

    /// <summary>
    /// Creation of cell grid, increasing sizes if they are too small
    /// </summary>
    private void SafeCreateCells()
    {
        if (width < 5) {
            width = 5;
        }
        if(width % 2 != 1) {
            width++;
        }
        if (height < 5) {
            height = 5;
        }
        if(height % 2 != 1) {
            height++;
        }

        cellGrid = new Cell[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                cellGrid[x, y] = new Cell();
            }
        }
    }

    /// <summary>
    /// Main labyrinth generation method
    /// </summary>
    private void GenerateLabyrinth()
    {
        SafeCreateCells();
        CreateBounds();

        PathwayMaker pathwayMaker = new PathwayMaker(cellGrid);
        pathwayMaker.GenerateLabyrinthByPrim();

        FindAllPaths();
        Visualize();
    }

    /// <summary>
    /// Logic for game finishing and exiting in menu scene
    /// </summary>
    public void GameOver()
    {
        StopAllCoroutines();
        CallGameOverWindow(player.GetCoinsCollectedCount());
    }

    /// <summary>
    /// Calling a window where player can insert his name
    /// </summary>
    private void CallGameOverWindow(int collectedCoinsCount)
    {
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Writes to a pathsList every cell marked as bPath
    /// </summary>
    private void FindAllPaths()
    {
        int width = cellGrid.GetLength(0);
        int height = cellGrid.GetLength(1);
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (cellGrid[x, y].IsPath()) {
                    pathsCoordinates.Add(new Coordinate(x, y));
                }
            }
        }
    }

    /// <summary>
    /// Accessing cells in cell grid
    /// </summary>
    /// <param name="x">X coordinate of cell</param>
    /// <param name="y">Y coordinate if cell</param>
    /// <returns>Cell from cell grid</returns>
    public Cell AccessCell(int x, int y)
    {
        return cellGrid[x, y];
    }

    /// <summary>
    /// Getting current player coordinate
    /// </summary>
    /// <returns>Coordinate in array</returns>
    public Coordinate GetPlayerCoordinate()
    {
        return player.GetCurrentCoordinate();
    }

    /// <summary>
    /// Getting list of units which are in labyrinth
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetUnitList()
    {
        return unitsInLabyrinth;
    }

    /// <summary>
    /// Full initialization of player data and logic
    /// </summary>
    public void InitializePlayer(System.Random randomizer)
    {
        SpawnPlayer(randomizer);
        cameraTargetObject = player.GetGameObject();
        player.StartMovementLoop();
    }

    /// <summary>
    /// Creating player owned by this LabyrinthManager in random cell
    /// </summary>
    private void SpawnPlayer(System.Random randomizer)
    {
        Coordinate spawnCoordinate = GetRandomPathCoordinate(randomizer);
        player = new Player(this, spawnCoordinate);
    }

    /// <summary>
    /// Smoothly centering camera over the cameraTargetObject
    /// </summary>
    private void CenterCamera()
    {
        if(cameraObject != null && cameraTargetObject != null) {
            Vector3 targetPosition = new Vector3(cameraTargetObject.transform.position.x, cameraTargetObject.transform.position.y, cameraObject.transform.position.z);
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Finding random path cell coordinate
    /// </summary>
    /// <returns>Path coordinate in the array</returns>
    private Coordinate GetRandomPathCoordinate(System.Random randomizer)
    {
        if(pathsCoordinates.Count > 0) {
            int index = randomizer.Next(0, pathsCoordinates.Count);
            return pathsCoordinates[index];
        }
        else {
            throw new System.Exception("Can't find any path in labyrinth!");
        }
    }
    
    /// <summary>
    /// Randomly spawning coins in coinSpawnPeriod. Spawning if coin count is less than 10
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator StartGeneratingCoins(System.Random randomizer)
    {
        while (true) {
            yield return new WaitForSeconds(coinSpawnPeriod);
            if(Coin.GetCoinCount() < 10) {
                Coordinate spawnCoordinate = GetRandomPathCoordinate(randomizer);
                while(player.IsIntersecting(spawnCoordinate)) {
                    spawnCoordinate = GetRandomPathCoordinate(randomizer);
                }
                unitsInLabyrinth.Add(new Coin(this, spawnCoordinate));
            }
        }
    }

    /// <summary>
    /// Looping logic for spawning enemies
    /// </summary>
    /// <param name="randomizer">Randomizer for random positioning</param>
    /// <returns></returns>
    private IEnumerator StartGeneratingEnemies(System.Random randomizer)
    {
        unitsInLabyrinth.Add(new Zombie(this, GetRandomPathCoordinate(randomizer)));
        while (true) {
            if (player.GetCoinsCollectedCount() == 5) {
                unitsInLabyrinth.Add(new Zombie(this, GetRandomPathCoordinate(randomizer)));
                break;
            }
            yield return new WaitForSeconds(coinSpawnPeriod);
        }

        while (true) {
            if (player.GetCoinsCollectedCount() == 10) {
                unitsInLabyrinth.Add(new Mummy(this, GetRandomPathCoordinate(randomizer)));
                break;
            }
            yield return new WaitForSeconds(coinSpawnPeriod);
        }

        yield break;
    }

    /// <summary>
    /// Creation of the outer walls of the labyrinth
    /// </summary>
    private void CreateBounds()
    {
        // Filling the southern and the nothern walls
        for (int x = 0; x < width; x++) {
            cellGrid[x, 0].SetUnbreakable();
            cellGrid[x, height - 1].SetUnbreakable();
        }

        // Filling the eastern and the western walls
        for (int y = 0; y < height; y++) {
            cellGrid[0, y].SetUnbreakable();
            cellGrid[width - 1, y].SetUnbreakable();
        }
    }

    /// <summary>
    /// Instantiating of labyrinth objects according to the mapped logic
    /// </summary>
    private void Visualize()
    {
        Vector3 startPosition = new Vector3(-width / 2, -height / 2);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                cellGrid[x, y].Visualize((int)(x * DistanceBetweenCells), (int)(y * DistanceBetweenCells), startPosition);
                cellGrid[x, y].GetGameObject().transform.SetParent(transform);
            }
        }
    }
}
