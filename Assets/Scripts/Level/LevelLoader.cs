using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadMap(Map map)
    {
        Level level = FindFirstObjectByType<Level>();

		if (level == null)
            throw new System.Exception("Scene doesn't have object with Level component");

		level.SpawnMap(map);
        level.gameMoves.Initialize(map.Teams);
        level.player.Initialize(map.Teams[0]);
	}

}
