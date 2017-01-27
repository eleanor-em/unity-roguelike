using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count {
        public int maximum;
        public int minimum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitialiseList() {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; ++x) {
            for (int y = 1; y < rows - 1; ++y) {
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }
    }

    private void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; ++x) {
            for (int y = -1; y < rows + 1; ++y) {
                GameObject tile = RandomValueInArray(floorTiles);
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    tile = RandomValueInArray(outerWallTiles);
                }

                GameObject instance = Instantiate(tile  , new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPos() {
        int index = Random.Range(0, gridPositions.Count);
        var pos = gridPositions[index];
        gridPositions.RemoveAt(index);
        return pos;
    }

    private T RandomValueInArray<T>(T[] array) {
        return array[Random.Range(0, array.Length)];
    }

    private void RandomObject(GameObject[] tileArray, int min, int max) {
        int count = Random.Range(min, max + 1);

        for (int i = 0; i < count; ++i) {
            Vector3 pos = RandomPos();
            var tile = RandomValueInArray(tileArray);
            Instantiate(tile, pos, Quaternion.identity);
        }
    }

    public void SetupScene(int level) {
        BoardSetup();
        InitialiseList();

        RandomObject(wallTiles, wallCount.minimum, wallCount.maximum);
        RandomObject(foodTiles, foodCount.minimum, foodCount.maximum);
        
        int enemies = (int)Mathf.Log(level, 2f);
        RandomObject(enemyTiles, enemies, enemies);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0), Quaternion.identity);
    }

    // Use this for initialization
    private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		
	}
}
