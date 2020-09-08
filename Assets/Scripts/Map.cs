using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    internal Camera mainCamera;

    private float tileSize = 1;
    public int rows = 9;
    public int cols = 16;

    List<GameObject> objectsOnMap = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        CenterCamera();
    }

    void CenterCamera()
    {
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        mainCamera.transform.position = new Vector3(gridW / 2 - tileSize / 2, -gridH / 2 + tileSize / 2, -1);
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                SpawnUnit(row, col, tilePrefab);

            }
        }
    }

    public void MoveObject(GameObject obj, int[] newPos)
    {
        PlaceUnit(newPos[0], newPos[1], obj);
    }

    public void PlaceUnit(int row, int col, GameObject reference)
    {
        float posX = col * tileSize;
        float posY = row * -tileSize;
        reference.transform.position = new Vector2(posX, posY);
        if (!objectsOnMap.Contains(reference))
        {
            objectsOnMap.Add(reference);
        }
    }

    public bool IsObjectAtPos(int row, int col)
    {
        foreach (GameObject obj in objectsOnMap)
        {
            float posX = col * tileSize;
            float posY = row * -tileSize;
            if (obj.transform.position.x == posX && obj.transform.position.y == posY)
            {
                return true;
            }
        }
        return false;
    }

    public int[] PlaceUnitAtRandomPos(GameObject reference)
    {
        List<int[]> validCoordinates = new List<int[]>();
        for (int row = 1; row < rows - 1; row++)
        {
            for (int col = 1; col < cols - 1; col++)
            {
                if (!IsObjectAtPos(row, col))
                {
                    validCoordinates.Add(new int[] { row, col });
                }
            }
        }
        int randIndex = Random.Range(0, validCoordinates.Count);
        int[] randCoord = validCoordinates[randIndex];
        PlaceUnit(randCoord[0], randCoord[1], reference);
        return randCoord;
    }

    internal GameObject SpawnUnit(int row, int col, GameObject reference)
    {
        GameObject newObj = (GameObject)Instantiate(reference, transform);
        float posX = col * tileSize;
        float posY = row * -tileSize;
        newObj.transform.position = new Vector2(posX, posY);
        return newObj;
    }

    public static int GetDistanceBetweenPoints(int[] pointA, int[] pointB)
    {
        return Mathf.Abs(pointA[0] - pointB[0]) + Mathf.Abs(pointA[1] - pointB[1]);
    }

    public bool CheckWithinBounds(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < rows && pos[1] >= 0 && pos[1] < cols;
    }
}
