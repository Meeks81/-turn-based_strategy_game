using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{

    public static int GenerateSeed()
    {
        return Random.Range(100000, int.MaxValue);
    }

    public static CellData[,] GenerateMap(MapSize size) => GenerateMap(size, GenerateSeed());
    public static CellData[,] GenerateMap(MapSize size, int seed)
    {
        System.Random random = new System.Random(seed);

        MapGenerateSettings settings = new MapGenerateSettings(size);

        Vector2Int minCellPos = Vector2Int.zero;
        Vector2Int maxCellPos = Vector2Int.zero;
        List<Vector2Int> cellPoints = new List<Vector2Int>();
        for (int i = 0; i < settings.ellipsesCount; i++)
        {
            Vector2 pos = new Vector2((float)random.NextDouble() * settings.ellipsePos, (float)random.NextDouble() * settings.ellipsePos);

            Vector2 radius = new Vector2();
            radius.x = (float)RandomRange(random, settings.ellipseMinRadius, settings.ellipseMaxRadius);
            radius.y = (float)RandomRange(random, settings.ellipseMinRadius, settings.ellipseMaxRadius);

            float rotation = (float)random.NextDouble() * 360f;

            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            int pointsCount = 100;
            for (int k = 0; k < pointsCount; k++)
            {
                float alpha = k * (360f / pointsCount) * Mathf.Deg2Rad;
                float beta = rotation * Mathf.Deg2Rad;
                float x = radius.x * Mathf.Cos(alpha) * Mathf.Cos(beta) - radius.y * Mathf.Sin(alpha) * Mathf.Sin(beta) + pos.x;
                float y = radius.x * Mathf.Cos(alpha) * Mathf.Sin(beta) + radius.y * Mathf.Sin(alpha) * Mathf.Cos(beta) + pos.y;

                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }

            for (float x = minX; x < maxX; x += 0.1f)
            {
                for (float y = minY; y < maxY; y += 0.1f)
                {
                    float x0 = pos.x;
                    float y0 = pos.y;
                    float rx = radius.x;
                    float ry = radius.y;
                    float p = rotation * Mathf.Deg2Rad;

                    float res = Mathf.Pow((x - x0) * Mathf.Cos(p) + (y - y0) * Mathf.Sin(p), 2) / Mathf.Pow(rx, 2) + Mathf.Pow(-(x - x0) * Mathf.Sin(p) + (y - y0) * Mathf.Cos(p), 2) / Mathf.Pow(ry, 2);
                    Vector2Int cellPos = new Vector2Int((int)x, (int)y);
                    if (res <= 1)
                    {
                        if (minCellPos.x > cellPos.x) minCellPos.x = cellPos.x;
                        if (minCellPos.y > cellPos.y) minCellPos.y = cellPos.y;
                        if (maxCellPos.x < cellPos.x) maxCellPos.x = cellPos.x;
                        if (maxCellPos.y < cellPos.y) maxCellPos.y = cellPos.y;

                        cellPoints.Add(cellPos);
                    }
                }
            }
        }

        Vector2Int offset = -minCellPos;
        for (int i = 0; i < cellPoints.Count; i++)
            cellPoints[i] += offset;

        Vector2Int mapSize = maxCellPos + offset + Vector2Int.one;
        CellData[,] map = new CellData[mapSize.x, mapSize.y];

        foreach (var item in cellPoints)
            map[item.x, item.y] = new CellData()
            {
                color = TeamColor.none,
                height = 1
            };

        return map;
    }

    public static CellData[,] GenerateTeams(CellData[,] map, Team[] teams)
    {
        Vector2Int mapSize = new Vector2Int(map.GetLength(0), map.GetLength(1));

        List<Vector2Int> availablePositions = new List<Vector2Int>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map[x, y].height <= 0)
                    continue;

                availablePositions.Add(new Vector2Int(x, y));
            }
        }

        float mapArea = mapSize.x * mapSize.y;
        float mapBusy = 1f / mapArea * availablePositions.Count;
        float minDistanceBetween = Mathf.Sqrt(mapArea / teams.Length) * mapBusy;
        if (minDistanceBetween < 2f)
            minDistanceBetween = 2f;

        Vector2Int[] positions = new Vector2Int[teams.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            Vector2Int pos = -Vector2Int.one;

            int iterations = 0;
            for (int j = 0; j < 100; j++)
            {
                iterations++;
                pos = availablePositions[Random.Range(0, availablePositions.Count)];
                if (map[pos.x, pos.y].height > 0)
                {
                    Vector2Int[] aroundCells = Cell.GetPositionsAround(pos);
                    bool isCorrect = true;
                    foreach (var item in aroundCells)
                    {
                        Vector2Int itemPos = pos + item;
                        if (itemPos.x < 0 ||
                            itemPos.y < 0 ||
                            itemPos.x >= mapSize.x ||
                            itemPos.y >= mapSize.y ||
                            map[itemPos.x, itemPos.y].height <= 0)
                        {
                            isCorrect = false;
                            break;
                        }
                    }
                    if (isCorrect == false)
                    {
                        availablePositions.Remove(pos);
                        continue;
                    }    

                    for (int k = 0; k < i; k++)
                    {
                        float distance = Vector2.Distance(positions[k], pos);
                        if (distance < minDistanceBetween)
                        {
                            isCorrect = false;
                            break;
                        }
                    }
                    if (isCorrect)
                        break;
                    else
                        availablePositions.Remove(pos);
                }
            }
            Debug.Log(iterations);

            positions[i] = pos;
        }

        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].x < 0)
                continue;

            map[positions[i].x, positions[i].y].color = teams[i].Color;
            Vector2Int[] aroundCells = Cell.GetPositionsAround(positions[i]);
            foreach (var item in aroundCells)
            {
                Vector2Int itemPos = positions[i] + item;
                map[itemPos.x, itemPos.y].color = teams[i].Color;
            }
        }

        return map;
    }

    private static double RandomRange(System.Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

}
