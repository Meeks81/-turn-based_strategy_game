using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private MapSize m_mapSize;
    [SerializeField] private int teamsCount;
    [SerializeField] private int startCoins;

    public void Start()
    {
        LevelLoader levelLoader = FindFirstObjectByType<LevelLoader>();

        List<TeamColor> teamColors = new List<TeamColor>();
        int teamColorsCount = System.Enum.GetNames(typeof(TeamColor)).Length;
        for (int i = 1; i < teamColorsCount; i++)
            teamColors.Add((TeamColor)i);

        Team[] teams = new Team[teamsCount];
        for (int i = 0; i < teamsCount; i++)
        {
            int colorIndex = Random.Range(0, teamColors.Count);
            TeamColor color = teamColors[colorIndex];
            teamColors.RemoveAt(colorIndex);

            teams[i] = new Team(color, startCoins);
        }
        Map map = new Map(m_mapSize, teams);

        levelLoader.LoadMap(map);
    }

}
