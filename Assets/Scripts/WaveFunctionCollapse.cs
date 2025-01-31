using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField] int gridWidth = 32;
    [SerializeField] int gridHeight = 18;
    [SerializeField] int maxStepSize = 576;

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject tileObject;
    [SerializeField] Sprite[] gbTileset;
    [SerializeField] Sprite[] grayscaleTileset;

    public int tilesetID = 0;
    Sprite[] currentTileset;
    GameObject[][] grid;
    Queue<GameObject> frontier = new Queue<GameObject>();

    bool firstTileFinished = false;

    void Start()
    {
        currentTileset = GetTileSheet(tilesetID);
        GenerateGrid();
        //use coroutine if the number of tiles isnt too big
        if(gridWidth*gridHeight > maxStepSize) 
        {
            WaveFunctionCollapseGeneration();
        }
        else 
        {
            StartCoroutine(WaveFunctionCollapseGenerationCR());
        } 
    }

    void GenerateGrid()
    {
        //create and fill the 2D Array
        grid = new GameObject[gridWidth][];
        for (int i = 0; i < gridWidth; i++)
        {
            grid[i] = new GameObject[gridHeight];
        }

        //instantiate each Array Object
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject newTile = Instantiate(tileObject);
                newTile.transform.position = new Vector2(x, y);
                newTile.GetComponent<TileData>().tilesetLength = currentTileset.Length;
                newTile.GetComponent<TileData>().Setup();
                grid[x][y] = newTile;
            }
        }
        //adjust camera object based on grid width/height
        mainCamera.transform.position = new Vector3(gridWidth/2, gridHeight/2,-10);
        mainCamera.orthographicSize = gridWidth / 3.2f;
    }

    Sprite[] GetTileSheet(int sheetID)
    {
        switch (sheetID)
        {
            case 0:
                return gbTileset;
            case 1:
                return grayscaleTileset;
            default:
                return gbTileset;
        }
    }

    //the body of the wave function collapse code
    void WaveFunctionCollapseGeneration()
    {
        //set up for first tile
        int randomX = Random.Range(0, gridWidth);
        int randomY = Random.Range(0, gridHeight);
        Debug.Log("start pos: " + randomX + "," + randomY);
        GameObject currentTile;
        int chosenTile;
        grid[randomX][randomY].GetComponent<TileData>().visited = true;
        frontier.Enqueue(grid[randomX][randomY]);

        while (frontier.Count != 0) //loop through all the tiles, using a frontier akin to a breadth first search
        {
            currentTile = frontier.Dequeue();
            chosenTile = currentTile.GetComponent<TileData>().ChooseRandomPossibleTile();
            if (!firstTileFinished) { Debug.Log("Chosen Tile: " + chosenTile); }
            currentTile.GetComponent<SpriteRenderer>().sprite = currentTileset[chosenTile];
            UpdateNeighbors(chosenTile, (int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            EnqueueNullNeighbors((int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            firstTileFinished = true;
        }
    }

    //the body of the wave function collapse code, with a pause for each tile
    IEnumerator WaveFunctionCollapseGenerationCR()
    {
        //set up for first tile
        int randomX = Random.Range(0, gridWidth);
        int randomY = Random.Range(0, gridHeight);
        Debug.Log("start pos: " + randomX + "," + randomY);
        GameObject currentTile;
        int chosenTile;
        grid[randomX][randomY].GetComponent<TileData>().visited = true;
        frontier.Enqueue(grid[randomX][randomY]);

        while (frontier.Count!=0) //loop through all the tiles, using a frontier akin to a breadth first search
        {
            yield return new WaitForSeconds(.0000001f); //this line is the only difference in the coroutine version
            currentTile = frontier.Dequeue();
            chosenTile = currentTile.GetComponent<TileData>().ChooseRandomPossibleTile();
            if (!firstTileFinished) { Debug.Log("Chosen Tile: "+chosenTile); }
            currentTile.GetComponent<SpriteRenderer>().sprite = currentTileset[chosenTile];
            UpdateNeighbors(chosenTile, (int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            EnqueueNullNeighbors((int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            firstTileFinished = true;
        }
    }

    //given an tile type, return the tiles that are not viable neighbors for that tile
    List<int> GetIncompatibleTiles(int tileID)
    {
        List<int> incompatibleTiles = new List<int>(); ;
        switch (tilesetID)
        {
            case 0:
                switch (tileID)
                {
                    case 0:
                        if (!firstTileFinished) { Debug.Log("case 0"); }
                        incompatibleTiles.Add(4);
                        return incompatibleTiles;
                    case 1:
                        if (!firstTileFinished) { Debug.Log("case 1"); }
                        incompatibleTiles.Add(2);
                        incompatibleTiles.Add(4);
                        incompatibleTiles.Add(5);
                        return incompatibleTiles;
                    case 2:
                        if (!firstTileFinished) { Debug.Log("case 2"); }
                        incompatibleTiles.Add(1);
                        return incompatibleTiles;
                    case 3:
                        if (!firstTileFinished) { Debug.Log("case 3"); }
                        incompatibleTiles.Add(4);
                        return incompatibleTiles;
                    case 4:
                        if (!firstTileFinished) { Debug.Log("case 4"); }
                        incompatibleTiles.Add(0);
                        incompatibleTiles.Add(1);
                        incompatibleTiles.Add(3);
                        return incompatibleTiles;
                    case 5:
                        if (!firstTileFinished) { Debug.Log("case 5"); }
                        incompatibleTiles.Add(1);
                        return incompatibleTiles;
                    default:
                        if (!firstTileFinished) { Debug.Log("DEFAULT CASE"); }
                        return incompatibleTiles;
                }
            case 1:
                switch (tileID)
                {
                    case 0:
                        if (!firstTileFinished) { Debug.Log("case 0"); }
                        incompatibleTiles.Add(4);
                        return incompatibleTiles;
                    case 1:
                        if (!firstTileFinished) { Debug.Log("case 1"); }
                        incompatibleTiles.Add(2);
                        incompatibleTiles.Add(4);
                        incompatibleTiles.Add(5);
                        return incompatibleTiles;
                    case 2:
                        if (!firstTileFinished) { Debug.Log("case 2"); }
                        incompatibleTiles.Add(1);
                        return incompatibleTiles;
                    case 3:
                        if (!firstTileFinished) { Debug.Log("case 3"); }
                        incompatibleTiles.Add(4);
                        return incompatibleTiles;
                    case 4:
                        if (!firstTileFinished) { Debug.Log("case 4"); }
                        incompatibleTiles.Add(0);
                        incompatibleTiles.Add(1);
                        incompatibleTiles.Add(3);
                        return incompatibleTiles;
                    case 5:
                        if (!firstTileFinished) { Debug.Log("case 5"); }
                        incompatibleTiles.Add(1);
                        return incompatibleTiles;
                    default:
                        if (!firstTileFinished) { Debug.Log("DEFAULT CASE"); }
                        return incompatibleTiles;
                }
            default:
                return incompatibleTiles;
        }
    }

    //update the neighbors of a given tile based on the given tiles value, so that neighbors cannot be an incompatible tile
    void UpdateNeighbors(int tileID, int x, int y)
    {
        List<int> incompatibleTiles = GetIncompatibleTiles(tileID);
        if (!firstTileFinished) 
        {
            for (int i = 0; i < incompatibleTiles.Count; i++) { Debug.Log(incompatibleTiles[i]); }
        }
        if (x+1 < gridWidth)
        {
            if (grid[x + 1][y].GetComponent<TileData>().tileID == -1)
            {
                for (int i = 0; i < incompatibleTiles.Count; i++)
                {
                    grid[x + 1][y].GetComponent<TileData>().RemovePossibleTile(incompatibleTiles[i]);
                }
            }
        }
        if (x-1 >= 0)
        {
            if (grid[x - 1][y].GetComponent<TileData>().tileID == -1)
            {
                for (int i = 0; i < incompatibleTiles.Count; i++)
                {
                    grid[x - 1][y].GetComponent<TileData>().RemovePossibleTile(incompatibleTiles[i]);
                }
            }
        }
        if (y+1 < gridHeight)
        {
            if (grid[x][y + 1].GetComponent<TileData>().tileID == -1)
            {
                for (int i = 0; i < incompatibleTiles.Count; i++)
                {
                    grid[x][y + 1].GetComponent<TileData>().RemovePossibleTile(incompatibleTiles[i]);
                }
            }
        }
        if (y-1 >= 0)
        {
            if (grid[x][y - 1].GetComponent<TileData>().tileID == -1)
            {
                for (int i = 0; i < incompatibleTiles.Count; i++)
                {
                    grid[x][y - 1].GetComponent<TileData>().RemovePossibleTile(incompatibleTiles[i]);
                }
            }
        }
    }

    //put the neighbors of a given tile in the frontier queue
    void EnqueueNullNeighbors(int x, int y)
    {
        if (x+1 < gridWidth)
        {
            if (!grid[x + 1][y].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x + 1][y]);
                grid[x + 1][y].GetComponent<TileData>().visited = true;
            }
        }
        if (x-1 >= 0)
        {
            if (!grid[x - 1][y].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x - 1][y]);
                grid[x - 1][y].GetComponent<TileData>().visited = true;
            }
        }
        if (y+1 < gridHeight)
        {
            if (!grid[x][y + 1].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x][y + 1]);
                grid[x][y + 1].GetComponent<TileData>().visited = true;
            }
        }
        if (y-1 >= 0)
        {
            if (!grid[x][y - 1].GetComponent<TileData>().visited) 
            {
                frontier.Enqueue(grid[x][y - 1]);
                grid[x][y - 1].GetComponent<TileData>().visited = true;
            }
        }
    }
}