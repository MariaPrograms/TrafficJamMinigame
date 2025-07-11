using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrafficManager : MonoBehaviour
{
    [SerializeField] private List<SplineContainer> paths;
    [SerializeField] private Transform carSpawnParent;
    [SerializeField] private NpcCar npcCar;
    [SerializeField] private Vector2 spawnTimeRange;

    private float currentSpawnTime;

    void Start()
    {
        currentSpawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
        GameManager.Instance.AddActionOnGameStart(Restart);
    }

    private void Restart()
    {
        for (int i = carSpawnParent.childCount - 1; i >= 0; i--)
        {
            Destroy(carSpawnParent.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.Playing)
        {
            currentSpawnTime -= Time.deltaTime; //Countdown
            if (currentSpawnTime <= 0)
            {
                var car = Instantiate(npcCar, carSpawnParent);
                car.SetCar(paths[Random.Range(0, paths.Count)]); //Spawning cars and setting path 
                currentSpawnTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y); //Reset the timer
            }
        }
    }

}
