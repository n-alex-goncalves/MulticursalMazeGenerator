
using System.Collections.Generic;
using UnityEngine;


public class CPU : MonoBehaviour
{
    private Rigidbody2D rb;

    //Initialization of generic CPU information.
    List<Transform> waypoints;
    List<Transform> node_position_list = new List<Transform>();

    public Dictionary<Vector2, Cell> allcell_dictionary = new Dictionary<Vector2, Cell>();

    public static float moveSpeed = 15f;
    int waypointIndex = 0;
    float _timer = 0f;

    //Initialization of the maze row and maze column variables.
    int maze_rows = 10;
    int maze_columns = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject Starting_Cell = GameObject.Find("Maze/Cell - X:1 Y:1"); //Searchs for the starting cell object of the maze - ultimately spawns the CPU.
        GetComponent<Rigidbody2D>().position = new Vector2(Starting_Cell.transform.position.x, Starting_Cell.transform.position.y);

        allcell_dictionary_creation();
        Node_Creation();
        waypoints = Breadth_First();
    }

    public class Cell //The Cell class used to describe each cell in playview.
    {
        public Vector2 grid_position;
        public GameObject cellObject;
        public CellScript cScript;
        public List<Cell> neighbours;
    }

   public void allcell_dictionary_creation() //Function that recreates the allcell_dictionary. 
    {
        for (int x = 1; x <= maze_columns; x++)
        {
            for (int y = 1; y <= maze_rows; y++)
            {
                Cell newCell = new Cell();

                newCell.grid_position = new Vector2(x, y);
                newCell.cellObject  = GameObject.Find("Maze/Cell - X:" + x + " Y:" + y);
                newCell.cScript = newCell.cellObject.GetComponent<CellScript>();
                newCell.neighbours = new List<Cell>();

                allcell_dictionary[new Vector2(x, y)] = newCell;
            }
        }

    }

     public List<Transform> Breadth_First() //Function that performs the BFS algorithm. Figures out the designated path for the CPU.
     {
         Cell end_cell = null; //End_cell is null as it has not been found yet.
         Cell start_cell = allcell_dictionary[new Vector2(1, 1)];

         foreach (KeyValuePair<Vector2, Cell> cell in allcell_dictionary)
         {
             if (cell.Key.y == maze_rows)
             {
                 if (cell.Value.cScript.wallU.activeSelf == false) //The end_cell has no upper wall, thus the location of the end_cell can be determined.
                 {
                     end_cell = cell.Value;
                 }
             }
         }

         //Initialization of all variables needed to complete the BFS algorithm.
         List<Cell> Queue = new List<Cell>();
         List<Cell> path = new List<Cell>();
         List<Cell> visited_list = new List<Cell>();
         List<Cell> pathway = new List<Cell>();
         bool t_junction = false;

         Queue.Add(start_cell); //Adds the start_cell into the queue.

         while (Queue.Count > 0)
         {
             Cell cell = Queue[Queue.Count - 1];
             visited_list.Add(cell);
             Queue.RemoveAt(Queue.Count - 1);
             path.Add(cell);

             Cell node = path[path.Count - 1];

            if (t_junction == true)
            {
                pathway.Add(cell);
            }

            if ((4 - GetList_of_Walls(node.cScript, node).Count) >= 3)
            {
                pathway = new List<Cell>();
                t_junction = true;
            }

            //The ending IF statement to determine whether the algorithm has found all paths.
             if (node == end_cell)
             {
                 foreach (Cell nodecell in path)
                 {
                     var position_of_nodecell = nodecell.cellObject.transform;
                     node_position_list.Add(position_of_nodecell);
                 }
                 return node_position_list;
             }

             foreach (Cell n in node.neighbours)
             {
                 if (visited_list.Contains(n) == false)
                 {
                     List<Cell> new_path = new List<Cell>();
                     new_path.Add(n);
                     Queue.AddRange(new_path);
                 }
                 else
                 {
                    continue;
                 }
             }

             //Translate the node_position list into a format suitable for the waypoints list.
            bool allVisited = node.neighbours.TrueForAll(x => visited_list.Contains(x));
            if (allVisited == true && pathway.Contains(node))
            {
                foreach (var path_cell in pathway) path.Remove(path_cell);
                t_junction = false;
                pathway = new List<Cell>();
            }


        }
         return null;
     }

    public bool CheckVertex(Cell curCell) //Function used to denote the location of T-Junctions in the maze.
    {
 
        List<string> allWallList = GetList_of_Walls(curCell.cScript, curCell);
        int allNeighbours = 4 - allWallList.Count;

        if (allNeighbours >= 3)
        {
            return true;
        }

        if (allWallList.Contains("LWall") && allWallList.Contains("UWall"))
        {
            return true;
        }

        if (allWallList.Contains("LWall") && allWallList.Contains("DWall"))
        {
            return true;
        }

        if (allWallList.Contains("RWall") && allWallList.Contains("UWall"))
        {
            return true;
        }

        if (allWallList.Contains("RWall") && allWallList.Contains("DWall"))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public List<string> GetList_of_Walls(CellScript cScript, Cell curCell) //Function used to determine the list of walls of the current cell.
    {
        List<string> neighbours = new List<string>();

        if (cScript.wallL.activeSelf == true)
        {
            neighbours.Add("LWall");
        }

        if (cScript.wallR.activeSelf == true)
        {
            neighbours.Add("RWall");
        }

        if (cScript.wallU.activeSelf == true)
        {
            neighbours.Add("UWall");
        }

        if (cScript.wallD.activeSelf == true)
        {
            neighbours.Add("DWall");
        }

        return neighbours;
    }

    private void Node_Creation() //Function used to determine the nodes of the maze.
    {
        Cell prev_cell = null;

        //Iteratively checks each cell and determines whether the cell is a node. 
        for (int y = 1; y <= maze_rows; y++)
        {
            for (int x = 1; x <= maze_columns; x++)
            {
                Cell currentCell = allcell_dictionary[new Vector2(x, y)];
                bool nodevertex = CheckVertex(currentCell);

                if (currentCell.cScript.wallL.activeSelf == true)
                {
                    if (currentCell.cScript.wallD.activeSelf == false)
                    {
                        prev_cell = currentCell;
                        currentCell.neighbours.Add(allcell_dictionary[new Vector2(x, y - 1)]);
                        allcell_dictionary[new Vector2(x, y - 1)].neighbours.Add(currentCell);
                    }
                    else if (nodevertex == true)
                    {
                        prev_cell = currentCell;
                    }

                }
                else
                {
                    if (currentCell.cScript.wallD.activeSelf == false)
                    {
                        currentCell.neighbours.Add(prev_cell);
                        prev_cell.neighbours.Add(currentCell);
                        currentCell.neighbours.Add(allcell_dictionary[new Vector2(x, y - 1)]);
                        allcell_dictionary[new Vector2(x, y - 1)].neighbours.Add(currentCell);
                    }
                    else if (nodevertex == true)
                    {
                        currentCell.neighbours.Add(prev_cell);
                        prev_cell.neighbours.Add(currentCell);
                        prev_cell = currentCell;
                    }
                }
            }
        }

    }

    private void Update()
    {
        Move(); //Translates the cell object.
    }

    private void Move()
    {
        _timer = Time.deltaTime; 

        //CPU object goes toward waypoint
        rb.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * _timer); 
        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }
    }
}