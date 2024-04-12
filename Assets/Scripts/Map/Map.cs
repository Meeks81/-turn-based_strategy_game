using UnityEngine;

public class Map
{

    public readonly int Seed;

    public Vector2Int Size { get; private set; }
    public Team[] Teams { get; private set; }
    public CellData[,] Cells { get; private set; }

    public Map(MapSize size, Team[] teams) : 
        this(size, MapGenerator.GenerateSeed(), teams)
    {
    }

    public Map(MapSize size, int seed, Team[] teams) :
        this(MapGenerator.GenerateMap(size, seed), teams)
    {
        Seed = seed;
    }

    public Map(CellData[,] cells, Team[] teams)
    {
        Seed = -1;
        Cells = MapGenerator.GenerateTeams(cells, teams);
        Teams = teams;
        Size = new Vector2Int(Cells.GetLength(0), Cells.GetLength(1));
    }

}
