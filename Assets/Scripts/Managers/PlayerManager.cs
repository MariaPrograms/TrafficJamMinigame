using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> Players => players;

    public String PlayerColor => playerColorName;

    public int EnemyCount
    {
        get => PlayerPrefs.GetInt("EnemyCount", 1);
        private set => PlayerPrefs.SetInt("EnemyCount", value);
    }

    [SerializeField] private CarManager carPrefab;
    [SerializeField] private List<Transform> carSpawnPoints;

    private List<Player> players = new List<Player>();
    private Colors colors;
    private string playerColorName
    {
        get => PlayerPrefs.GetString("PlayerColorName", "Red");
        set => PlayerPrefs.SetString("PlayerColorName", value);
    }

    public void SetColors(Colors colorsObject)
    {
        colors = colorsObject;
    }

    public void SetPlayerColor(String colorName)
    {
        playerColorName = colorName;
    }

    public ColorInfo GetPlayerColorInfo()
    {
        return colors.GetColor(playerColorName);
    }

    public Color GetPlayerColor()
    {
        return colors.GetColor(playerColorName).Color;
    }

    public void UpdateEnemyCount(int amount)
    {
        //Set it up so users can circle through the numbers available instead of being locked at the limits
        int newCount = EnemyCount + amount;
        if (newCount < 1)
        {
            newCount = carSpawnPoints.Count - 1;
        }
        else if (newCount > carSpawnPoints.Count - 1)
        {
            newCount = 1;
        }
        EnemyCount = newCount;
    }

    public void OnGameStart()
    {
        CheckForRestart();
        SpawnCars();
    }

    private void CheckForRestart()
    {
        //Removing old players if they exist
        if (players.Count > 0)
        {
            foreach (var player in players)
            {
                Destroy(player.Car.gameObject);
            }
        }
        players = new List<Player>();
    }

    private void SpawnCars()
    {
        var points = new List<Transform>(carSpawnPoints);
        colors.SelectColor(playerColorName);

        for (int i = 0; i <= EnemyCount; i++)
        {
            var spawnPointIndex = UnityEngine.Random.Range(0, points.Count);
            CarManager car = Instantiate(carPrefab);

            //Setting up players
            Player player = new Player()
            {
                Car = car,
                ColorInfo = i == 0 ? GetPlayerColorInfo() : colors.GetRandomColor(true),
                NPC = i != 0 //When getting the winner to see if it's the player or not 
            };
            player.Name = player.ColorInfo.Name;
            players.Add(player);

            BaseCarController carType = i == 0 ? new PlayerCarController() : new CpuCarController();
            car.Setup(carType, points[spawnPointIndex], player.ColorInfo.Color);

            points.RemoveAt(spawnPointIndex);
        }
    }

    public bool DidPlayerWin()
    {
        return !GetPlayerWithMostMoney().NPC;
    }

    public Player GetPlayerWithMostMoney()
    {
        players.Sort((a, b) => Comparer<int>.Default.Compare(b.Car.TotalMoney, a.Car.TotalMoney));
        return players[0];
    }

    public void SetCarState(bool enable)
    {
        foreach (Player player in players)
        {
            player.Car.enabled = enable;
        }
    }
}

[Serializable]
public class Player
{
    public CarManager Car;
    public string Name;
    public ColorInfo ColorInfo;
    public bool NPC;
}

