using System.Collections.Generic;

/// <summary>
/// Generates random letters and assignes to the grid depending on grid size and game play type
/// </summary>
public class LettersGenerator
{
    System.Random random = new System.Random();
    char[] alphabet = new char[26]{ 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                          'H', 'I', 'J', 'K', 'L', 'M', 'N',
                          'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                          'V', 'W', 'X', 'Y', 'Z' };
    LevelsData levelsJsonData;

    public LettersGenerator(List<TMPro.TextMeshProUGUI> _tiles, string _levelJson = "")
    {
        if (Manager.Instance.gamePlayType == Manager.GamePlayType.Endless)
            RandomLettersTile(_tiles);
        else
        {
            levelsJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<LevelsData>(_levelJson);
            LoadLevelData(_tiles, 0);
        }
    }
    public void LoadLevelData(List<TMPro.TextMeshProUGUI> _tiles, int _currentLevel)
    {
        UnityEngine.UI.GridLayoutGroup gridLayoutGroup = _tiles[0].GetComponentInParent<UnityEngine.UI.GridLayoutGroup>();
        gridLayoutGroup.enabled = true;
        gridLayoutGroup.constraintCount = (levelsJsonData.data[_currentLevel].gridSize.x > levelsJsonData.data[_currentLevel].gridSize.y) ? levelsJsonData.data[_currentLevel].gridSize.x : levelsJsonData.data[_currentLevel].gridSize.y;
        int totalTiles = levelsJsonData.data[_currentLevel].gridSize.x * levelsJsonData.data[_currentLevel].gridSize.y;
        List<GridData> gridData = levelsJsonData.data[_currentLevel].gridData;
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (i >= totalTiles)
                _tiles[i].transform.parent.gameObject.SetActive(false);
            else
            {
                _tiles[i].transform.parent.gameObject.SetActive(true);
                _tiles[i].text = gridData[i].letter;
                _tiles[i].GetComponentInParent<LettersTile>().assignedLetter = gridData[i].letter[0];
            }
        }
        UpdataLayout(gridLayoutGroup);
    }
    public void RandomLettersTile(List<TMPro.TextMeshProUGUI> _tiles)
    {
        List<char> _tileChar = new List<char>();
        UnityEngine.UI.GridLayoutGroup gridLayoutGroup = _tiles[0].GetComponentInParent<UnityEngine.UI.GridLayoutGroup>();
        gridLayoutGroup.constraintCount = 4;
        gridLayoutGroup.enabled = true;
        while (_tileChar.Count < 16)
        {
            char _tempChar = alphabet[random.Next(0, 26)];
            if (!_tileChar.Contains(_tempChar))
            {
                _tiles[_tileChar.Count].transform.parent.gameObject.SetActive(true);
                _tiles[_tileChar.Count].text = _tempChar.ToString();
                _tiles[_tileChar.Count].GetComponentInParent<LettersTile>().assignedLetter = _tempChar;
                _tileChar.Add(_tempChar);
            }
        }
        UpdataLayout(gridLayoutGroup);
#if Logs
        string _res = "";
        for (int i = 0; i < _tileChar.Count; i++)
            _res += _tileChar[i];
        Debug.LogError($"=={_res}==");
#endif
    }
    void UpdataLayout(UnityEngine.UI.GridLayoutGroup _gridLayoutGroup)
    {
        _gridLayoutGroup.CalculateLayoutInputHorizontal();
        _gridLayoutGroup.CalculateLayoutInputVertical();
        _gridLayoutGroup.SetLayoutHorizontal();
        _gridLayoutGroup.SetLayoutVertical();
        _gridLayoutGroup.enabled = false;
    }
    public void ResetLetterTiles(List<TMPro.TextMeshProUGUI> _tiles, int _loadedLevel = 0)
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            LettersTile _lettersTile = _tiles[i].GetComponentInParent<LettersTile>();
            if (_lettersTile)
                _lettersTile.ResetToInitialPositionTile();
        }
        if (Manager.Instance.gamePlayType == Manager.GamePlayType.Endless)
            RandomLettersTile(_tiles);
        else
            LoadLevelData(_tiles, _loadedLevel);
    }
}
