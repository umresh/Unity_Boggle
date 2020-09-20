using System.Collections.Generic;

[System.Serializable]
public class LevelsData
{
    public List<Level> data { get; set; }
}
[System.Serializable]
public class Level
{
    public int bugCount { get; set; }
    public int wordCount { get; set; }
    public int timeSec { get; set; }
    public int totalScore { get; set; }
    public GridSize gridSize { get; set; }
    public List<GridData> gridData { get; set; }
    public int? levelType { get; set; }
}
[System.Serializable]
public class GridSize
{
    public int x { get; set; }
    public int y { get; set; }
}
[System.Serializable]
public class GridData
{
    public int tileType { get; set; }
    public string letter { get; set; }
}