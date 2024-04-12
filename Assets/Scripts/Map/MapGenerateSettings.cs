public enum MapSize
{
    small, medium, large
}

public struct MapGenerateSettings
{

    public int ellipsesCount;
    public float ellipsePos;
    public float ellipseMinRadius;
    public float ellipseMaxRadius;

    public MapGenerateSettings(MapSize mapSize)
    {
        switch (mapSize)
        {
            case MapSize.small:
                ellipsesCount = 6;
                ellipsePos = 10f;
                ellipseMinRadius = 1f;
                ellipseMaxRadius = 7f;
                break;
            case MapSize.medium:
                ellipsesCount = 8;
                ellipsePos = 20f;
                ellipseMinRadius = 2f;
                ellipseMaxRadius = 14f;
                break;
            case MapSize.large:
                ellipsesCount = 9;
                ellipsePos = 30f;
                ellipseMinRadius = 3f;
                ellipseMaxRadius = 20f;
                break;
            default:
                ellipsesCount = 0;
                ellipsePos = 0;
                ellipseMinRadius = 0;
                ellipseMaxRadius = 0;
                break;
        }
    }
}
