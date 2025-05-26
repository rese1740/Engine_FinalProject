using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject player;
    public int currentStage = 0;
    int lastStage = 20;

    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }
    public StartPositionArray[] startPositionArrays;

    public List<Transform> StartPositionAngle = new List<Transform>();

    public List<Transform> StartPositionBoss = new List<Transform>();

    public Transform StartPositionLastBoss;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void NextStage()
    {
        currentStage++;
        if (currentStage >= lastStage)
        {
            return;
        }


        if (currentStage % 5 != 0)
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else 
        {
            if (currentStage % 10 == 5)
            {
                int randomIndex = Random.Range(0, StartPositionAngle.Count);
            player.transform.position = StartPositionAngle[randomIndex].position;
            }
            else
            {
                if (currentStage == lastStage)
                {
                    player.transform.position = StartPositionLastBoss.position;
                }
                else
                {
                    int randomIndex = Random.Range(0, StartPositionBoss.Count);
                    player.transform.position = StartPositionBoss[randomIndex].position;
                    StartPositionBoss.RemoveAt(currentStage / 10);
                }
            }
        }
    }
}
