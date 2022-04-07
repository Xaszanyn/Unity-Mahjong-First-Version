using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private List<Level> levels;

    public void LoadLevel()
    {
        var levelToLoad = levels[Random.Range(0, levels.Count)];
        var level = Instantiate(levelToLoad);
        TileManager.instance.Initialize(level.tiles);
    }
}
