using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public static class LevelGenerator
{
    static bool[][] obstaclePattern =
    {
        new bool[]{
            true, true, true, true, true,
            false, false, true, false, false,
            false, false, true, false, false,
            false, false, false, false, false,
            false, false, false, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, false, false, false, true,
            true, true, true, true, true,
            false, false, false, false, false,
            false, false, false, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, true, true, true, true,
            false, true, true, true, false,
            false, false, true, false, false,
            false, false, false, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, true, true, true, true,
            false, false, true, true, true,
            false, false, true, true, true,
            false, false, true, true, true
        },

        new bool[]{
            true, true, true, true, true,
            true, true, true, true, true,
            true, true, true, false, false,
            true, true, true, false, false,
            true, true, true, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, true, false, true, true,
            true, false, false, false, true,
            false, false, false, false, false,
            false, false, false, false, false
        },

        new bool[]{
            true, false, true, false, true,
            false, true, false, true, false,
            true, false, true, false, true,
            false, true, false, true, false,
            false, false, false, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, true, true, true, true,
            false, true, false, true, false,
            false, true, false, true, false,
            false, false, false, false, false
        },

        new bool[]{
            true, true, true, true, true,
            true, false, false, false, true,
            true, false, false, false, true,
            true, false, false, false, true,
            true, true, true, true, true,
        },

        new bool[]{
            true, false, false, false, true,
            true, true, true, true, true,
            false, false, false, false, false,
            false, false, false, false, false,
            false, false, false, false, false,
        },

        new bool[]{
            true, true, true, true, true,
            false, true, true, false, false,
            false, true, true, false, false,
            false, true, true, false, false,
            false, false, false, false, false,
        },

        new bool[]{
            true, true, true, true, true,
            false, false, true, true, false,
            false, false, true, true, false,
            false, false, true, true, false,
            false, false, false, false, false,
        },

        new bool[]{
            true, true, true, true, true,
            true, false, true, false, true,
            true, false, true, false, true,
            true, true, true, true, true,
            false, false, false, false, false,
        },
    };

    static private List<GameObject> obstaclesList = new List<GameObject>();

    static private GameObject obstacle;
    static private GameObject defaultBox;
    static private GameObject wall;

    static public float startZPosition { private get; set; } = 0;

    static public void CreateFirstObstacles()
    {
        obstacle = Resources.Load<GameObject>("Obstacle");
        defaultBox = Resources.Load<GameObject>("DefaultBox");
        wall = Resources.Load<GameObject>("Wall");

        for (int i = 0; i < 5; i++)
            CreateNewObstacle();
    }

    static public void CreateNewObstacle()
    {
        GameObject newObstacle = GameObject.Instantiate(obstacle);
        newObstacle.transform.position = new Vector3(0, 2, startZPosition);
        startZPosition += 20;

        obstaclesList.Add(newObstacle);
        if(obstaclesList.Count > 9)
        {
            Object.Destroy(obstaclesList[0]);
            obstaclesList.RemoveAt(0);
        }

        int ranNumWall = Random.Range(0, obstaclePattern.Length);

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if (obstaclePattern[ranNumWall][i * 5 + j])
                {
                    GameObject newWall = GameObject.Instantiate(wall, newObstacle.transform);
                    newWall.transform.localPosition = new Vector3(j - 2, i, 20);
                }
            }
        }

        int ranNumPickup = Random.Range(2, 4);

        string[] values = new string[ranNumPickup];

        bool isOrginal = true;

        for(int i = 0; i < ranNumPickup; i++)
        {
            GameObject newDefaultBox = GameObject.Instantiate(defaultBox, newObstacle.transform);

            int fValue = Random.Range(0, 5) - 2;
            int sValue = Random.Range(5, 17);

            string strValues = fValue.ToString() + sValue.ToString();

            foreach(string v in values)
            {
                if(v == strValues) 
                    isOrginal = false;
            }

            if (isOrginal)
            {
                newDefaultBox.transform.localPosition = new Vector3(fValue, 0, sValue);
            }
            else
            {
                i--;
            }
        }

    }
}
