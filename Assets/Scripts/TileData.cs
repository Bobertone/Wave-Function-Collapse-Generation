using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public bool visited = false;
    public int tileID = -1;
    public int tilesetLength;
    public List<int> possibleTiles = new List<int>();


    public void Setup()
    {
        for (int i = 0; i < tilesetLength; i++)
        {
            possibleTiles.Add(i);
        }
    }

    public int ChooseRandomPossibleTile() 
    {
        if(possibleTiles.Count != 0)
        {
            int randTile = Random.Range(0, possibleTiles.Count - 1);
            randTile = possibleTiles[randTile];
            tileID = randTile;
            return tileID;
        }
        else { Debug.Log(gameObject.transform.position.x +","+ gameObject.transform.position.y+" err: No possible tiles");  return 0; }
    }

    public void RemovePossibleTile(int tileID) 
    {
        possibleTiles.Remove(tileID);
    }

}
