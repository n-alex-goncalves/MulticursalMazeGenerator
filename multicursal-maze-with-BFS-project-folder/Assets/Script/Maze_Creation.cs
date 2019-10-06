using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Maze_Creation : MonoBehaviour
{
    public class Cell //The Cell class for each square of the grid
    {
        //Declaration of attributes for the Cell class
        public Vector2 position;
        public GameObject cellObject;
        public CellScript cellScript;
        public List<Cell> neighbours;
        public MeshRenderer colour;
    }

    //Initialization of generic maze information for the algorithm
    private Cell currentCell;
    private Cell nextCell;
    private float cellSize;
    public int mazeRows;
    public int mazeColumns;

    //Initialization of all lists used in the script
    private List<Cell> unvisitedCellList = new List<Cell>();
    private List<Cell> cellStack = new List<Cell>();
    private List<Cell> T_JunctionList;

    //Dictionary of all the cells created in the script
    public Dictionary<Vector2, Cell> allcellDictionary;

    //Vector of the potential position of the neighbour cell.
    private Vector2[] neighbour_vector = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    //Initialization of GameObjects that are viewable in the playview of the game
    [SerializeField] 
    private GameObject cellPrefab;
    private GameObject mazeParent;

    //Start function
    private void Start()
    {
        CreateMaze(mazeRows, mazeColumns);
    }

    //Abstraction of the Maze_Grid() algorithm for the start function. Needed for row and column definition
    private void CreateMaze(int rows, int columns) 
    {
        mazeRows = rows;
        mazeColumns = columns;
        Maze_Grid();
    }

    //Function that creates the grid of the maze.
    public void Maze_Grid() 
    {

        allcellDictionary = new Dictionary<Vector2, Cell>();
        //Declaration of generic maze information.
        cellSize = cellPrefab.transform.localScale.x;
        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";

        //The startPosition and spawnPosition is required for the scene position
        Vector2 startPosition = new Vector2(-(cellSize * (mazeColumns / 2)) + (cellSize / 2), -(cellSize * (mazeRows / 2)) + (cellSize / 2));
        Vector2 spawnPosition = startPosition;

        //Nested for-Loop to create all squares of the maze.
        for (int x = 1; x <= mazeColumns; x++) 
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                ConstructCell(spawnPosition, new Vector2(x, y));
                spawnPosition.y += cellSize * 2;
            }
            spawnPosition.y = startPosition.y;
            spawnPosition.x += cellSize * 2;
        }

        //Order of all algorithms in the script.
        currentCell = allcellDictionary[new Vector2(1, 1)];
        DFSBackTrackingAlgorithm();
        CreateExit();
        TJunctionAlgorithm();
    }

    //Function that removes walls from the maze object to create the grid of the cell
    //Depth-First Algorithm
    public void DFSBackTrackingAlgorithm() 
    {
        unvisitedCellList.Remove(currentCell);
        while (unvisitedCellList.Count > 0)
        {
            List<Cell> unvisitedNeighbours = List_ofUnvisitedNeighbours(currentCell);
            if (unvisitedNeighbours.Count > 0)
            {
                nextCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                cellStack.Add(currentCell);
                Delete_SimilarWall(currentCell, nextCell);
                currentCell = nextCell;
                unvisitedCellList.Remove(currentCell);
            }
            else if (cellStack.Count > 0)
            {
                currentCell = cellStack[cellStack.Count - 1];
                cellStack.Remove(currentCell);
            }
        }
    }

    //Function that activates the T-Junction script for all T-Junctions on the scene.
    public void TJunctionAlgorithm() 
    {
        //Nested for-loop to check each cell in the playview. 
        for (int x = 1; x <= mazeColumns; x++)
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                currentCell = allcellDictionary[new Vector2(x, y)];
                int allNeighbours = Number_ofNeighbours(currentCell.cellScript, currentCell);

                //Determines whether the current cell has three neighbours (in which case the cell is a T-junction)
                if (allNeighbours >= 3)
                {
                    //Activation of all T-Junction scripts for each T-Junction cell.
                    currentCell.cellScript.triggerL.SetActive(true);
                    currentCell.cellScript.triggerR.SetActive(true);
                    currentCell.cellScript.triggerU.SetActive(true);
                    currentCell.cellScript.triggerD.SetActive(true);

                }
            }
        }
    }

    //Function that activates the exit script for the end cell of the scene.
    public void CreateExit()
    {
        //The top row of cells in the maze
        List<Cell> edgeCells = new List<Cell>();

        //Determines the top row of the cell in the allcellDictionary. Adds the top row into a list of cell.
        foreach (KeyValuePair<Vector2, Cell> cell in allcellDictionary)
        {
            if (cell.Key.y == mazeRows)
            {
                edgeCells.Add(cell.Value);
            }
        }

        Cell end_Cell = edgeCells[Random.Range(0, edgeCells.Count)];
        end_Cell.cellScript.GetComponent<SpriteRenderer>().color = Color.green;
        end_Cell.cellObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        //Variable that holds a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene(); 

        //Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Standard_Game_Scene")
        {
            end_Cell.cellObject.AddComponent(typeof(endCell_standard_game));
        }
        else if (sceneName == "Multiplayer_Game_Scene")
        {
            end_Cell.cellObject.AddComponent(typeof(endCell_multiplayer_game));
        }

        end_Cell.cellObject.AddComponent<BoxCollider2D>();
        end_Cell.cellObject.GetComponent<BoxCollider2D>().isTrigger = true;
        RemoveWall(end_Cell.cellScript, 3);
    }

    //Function that finds the number of neighbour for the current cell. 
    public int Number_ofNeighbours(CellScript cellScript, Cell currentCell) 
    {
        int no_ofNeighbours = 0;
        Cell nCell = currentCell;
        Vector2 cPos = currentCell.position;

        Vector2 nLPos = cPos + new Vector2(-1, 0);
        if (cellScript.wallL.activeSelf == false)
        {
            no_ofNeighbours += 1;
        }

        Vector2 nRPos = cPos + new Vector2(1, 0);
        if (cellScript.wallR.activeSelf == false)
        {
            no_ofNeighbours += 1;
        }

        Vector2 nUPos = cPos + new Vector2(0, 1);
        if (cellScript.wallU.activeSelf == false)
        {
            no_ofNeighbours += 1;
        }

        Vector2 nDPos = cPos + new Vector2(0, -1);
        if (cellScript.wallD.activeSelf == false)
        {
            no_ofNeighbours += 1;
        }

        return no_ofNeighbours;
    }

    //Function that returns a list of all unvisited neighbours when compared to the current cell.
    public List<Cell> List_ofUnvisitedNeighbours(Cell currentCell) 
    {
        List<Cell> neighbours = new List<Cell>();
        Cell nCell = currentCell;
        Vector2 cPos = currentCell.position;

        foreach (Vector2 p in neighbour_vector)
        {
            Vector2 nPos = cPos + p;
            if (allcellDictionary.ContainsKey(nPos)) nCell = allcellDictionary[nPos];
            if (unvisitedCellList.Contains(nCell)) neighbours.Add(nCell);
        }

        return neighbours;
    }

    //Function that deactivates the corresponding wall with the ID.
    public void RemoveWall(CellScript cellScript, int wallID) 
    {
        if (wallID == 1) cellScript.wallL.SetActive(false);
        else if (wallID == 2) cellScript.wallR.SetActive(false);
        else if (wallID == 3) cellScript.wallU.SetActive(false);
        else if (wallID == 4) cellScript.wallD.SetActive(false);
    }

    //Function that deletes the wall between the current cell and the neighbour cell.
    public void Delete_SimilarWall(Cell currentCell, Cell neighbourCell)
    {
        {
            if (neighbourCell.position.x < currentCell.position.x)
            {
                RemoveWall(neighbourCell.cellScript, 2);
                RemoveWall(currentCell.cellScript, 1);
            }

            else if (neighbourCell.position.x > currentCell.position.x)
            {
                RemoveWall(neighbourCell.cellScript, 1);
                RemoveWall(currentCell.cellScript, 2);
            }

            else if (neighbourCell.position.y > currentCell.position.y)
            {
                RemoveWall(neighbourCell.cellScript, 4);
                RemoveWall(currentCell.cellScript, 3);
            }

            else if (neighbourCell.position.y < currentCell.position.y)
            {
                RemoveWall(neighbourCell.cellScript, 3);
                RemoveWall(currentCell.cellScript, 4);
            }
        }
    }

    //Function that instantiates each cell in the maze grid. 
    public void ConstructCell(Vector2 position, Vector2 gridPosition) 

    {
        Cell newCell = new Cell();

        newCell.position = gridPosition;

        //Instantiation of the cell object
        newCell.cellObject = Instantiate(cellPrefab, position, cellPrefab.transform.rotation);

        if (mazeParent != null) newCell.cellObject.transform.parent = mazeParent.transform;

        //Defines the object name for each cell
        newCell.cellObject.name = "Cell - X:" + gridPosition.x + " Y:" + gridPosition.y;
        newCell.cellScript = newCell.cellObject.GetComponent<CellScript>();
        newCell.neighbours = new List<Cell>();

        //Deactivation of all TriggerScript beforehand
        newCell.cellScript.triggerL.SetActive(false);
        newCell.cellScript.triggerR.SetActive(false);
        newCell.cellScript.triggerU.SetActive(false);
        newCell.cellScript.triggerD.SetActive(false);

        allcellDictionary[gridPosition] = newCell;

        unvisitedCellList.Add(newCell);
    }
   }
