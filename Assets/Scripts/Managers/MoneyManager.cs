using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static List<Money> moneyInPlay { get; private set; } //For Enemy cars to reference 
    [SerializeField] private Vector2 moneySpawnTimeRange;
    [SerializeField] private Vector2Int moneySpawnAmountRange;
    [SerializeField] private List<Money> moneyPrefabs;
    [SerializeField] private List<Transform> moneySpawnPoints;

    private float currentSpawnTime;
    private int weightTotal = 0;

    void Start()
    {
        moneyInPlay = new List<Money>();
        currentSpawnTime = Random.Range(moneySpawnTimeRange.x, moneySpawnTimeRange.y);
        moneyPrefabs.ForEach(a => weightTotal += a.Weight); // For weighted random selection of money so higher values are rarer
        GameManager.Instance.AddActionOnGameStart(Restart);
    }

    private void Restart()
    {
        //Removing any existing money from play for a clean start
        foreach (var item in moneySpawnPoints)
        {
            if (item.childCount > 0)
            {
                Destroy(item.GetChild(0).gameObject);
            }
        }
        moneyInPlay = new List<Money>();

        //Spawning in money so players don't have to wait
        int randomSpawnIn = Random.Range(moneySpawnAmountRange.x, moneySpawnAmountRange.y + 1);
        for (int i = 0; i <= randomSpawnIn; i++)
        {
            SpawnMoney();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.Playing)
        {
            currentSpawnTime -= Time.deltaTime; //Countdown the timer
            if (currentSpawnTime <= 0)
            {
                //Checking if theirs free spots available
                bool pointFree = moneySpawnPoints.Exists(a => a.childCount == 0);
                int spawnPointsLeft = moneySpawnPoints.FindAll(a => a.childCount == 0).Count;
                int maxSpawn = spawnPointsLeft > moneySpawnAmountRange.y ? moneySpawnAmountRange.y : spawnPointsLeft;

                //Making sure that there is enough spots to spawn in more money
                if (maxSpawn > moneySpawnAmountRange.x)
                {
                    int randomSpawnIn = Random.Range(moneySpawnAmountRange.x, maxSpawn + 1);
                    for (int i = 0; i <= randomSpawnIn; i++)
                    {
                        SpawnMoney();
                    }
                }

                //Reset the timer even if the spots arn't available so that theirs more of a chance for free spaces
                currentSpawnTime = Random.Range(moneySpawnTimeRange.x, moneySpawnTimeRange.y);
            }
        }
    }

    private void SpawnMoney()
    {
        List<Transform> availableSpots = moneySpawnPoints.Where(a => a.childCount == 0).ToList();

        //Double checking there are spots available so no error gets thrown
        if (availableSpots.Count > 0)
        {
            int index = Random.Range(0, availableSpots.Count);

            Money money = Instantiate(GetRandomMoney(), availableSpots[index]); // Creating money
            money.actionOnDestroy = () => RemoveMoney(money);
            money.transform.localEulerAngles = new Vector3(-90, 0, Random.Range(0, 360));
            moneyInPlay.Add(money);
        }
    }

    private void RemoveMoney(Money money)
    {
        moneyInPlay.Remove(money);
    }

    //Weighted random check
    private Money GetRandomMoney()
    {
        //weighted random
        int r = Random.Range(0, weightTotal);
        foreach (var money in moneyPrefabs)
        {
            if (r < money.Weight)
            {
                return money;
            }
            r -= money.Weight;
        }

        Debug.LogError("issue with getting weighted random");
        return moneyPrefabs[0];
    }
}
