using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]
public enum GameStat { PreStart, Started, TimeUp, Win, Loose }
public class GameManager : MonoBehaviour
{

    public static event UnityAction<GameStat, GameStat> OnStatChanged;

    /// <summary>
    /// current time scale of the game
    /// </summary>
    [SerializeField] float _timeScale = 1f;
    /// <summary>
    /// the reference to display the score on UI
    /// </summary>
    [SerializeField] Text _scoreBoard;
    /// <summary>
    /// the visual path remainder
    /// </summary>
    [SerializeField] Slider _path;
    /// <summary>
    /// the values on which all the level length is calculated
    /// </summary>
    [SerializeField] float _levelTimeIncreaseFactor;
    [SerializeField] float _levelTimeBase;
    [SerializeField] float _speedIncreaseFactor;
    [SerializeField] int _increaseStep = 30;
    /// <summary>
    /// the game objects endicating the start and end of the line
    /// </summary>
    [SerializeField] GameObject _endOftheLine;
    [SerializeField] GameObject _debriesSpawner;
    /// <summary>
    /// total minuts pass from the begining
    /// </summary>
    int _minutsPass = 0;
    /// <summary>
    /// total step pass from the begining
    /// </summary>
    int _stepPass;

    [SerializeField] GameObject[] _livesImages;

    int _currentScore = 0;

    [SerializeField] GameStat _currentStat;

    [SerializeField] GameObject[] _allSpawners;

    [SerializeField] Text _gameLevelText;

    //[SerializeField] PlayableDirector _endGamePlayable;

    private void Start()
    {
        _changeStat(GameStat.PreStart);
    }
    public void _StartGame()
    {
        _changeStat(GameStat.Started);
    }
    void Update()
    {
        _performStat();
    }
    public void _FoxDied()
    {
        _changeStat(GameStat.Loose);
    }
    public void _GameWon()
    {
        _changeStat(GameStat.Win);
    }
    public void _updateHealth(int iCurrentHealth)
    {
        try
        {
            _livesImages[iCurrentHealth].SetActive(false);
        }
        catch
        {
            Debug.Log("Game Over");
            _changeStat(GameStat.Loose);
        }
    }
    public void _ChangeScore(int iScore)
    {
        _currentScore = _currentScore + iScore;
        _scoreBoard.text = _currentScore.ToString();
    }
    void _changeStat(GameStat iToStat)
    {

        _endStat();
        _currentStat = iToStat;
        _startStat();
    }
    void _startStat()
    {
        switch (_currentStat)
        {
            case GameStat.PreStart:

                _stepPass = GameSettingInfo.Instance.CurrentGameLevel;
                _gameLevelText.text = _stepPass.ToString();
                Debug.Log("Level:" + _stepPass);
                _path.value = 0;
                _path.maxValue = (_stepPass * _levelTimeIncreaseFactor) + _levelTimeBase;
                break;
            case GameStat.Started:
                break;
            case GameStat.Win:
                GameSettingInfo.Instance.IsGameWon = true;
                GameSettingInfo.Instance.ThisRunScore = _currentScore;
                GameSettingInfo.Instance.CurrentGameLevel++;
                BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.AftermathScene);

                break;
            case GameStat.Loose:
                GameSettingInfo.Instance.IsGameWon = false;
                BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.AftermathScene);
                break;
            case GameStat.TimeUp:
                _endOftheLine.SetActive(true);
                break;
        }
    }
    void _endStat()
    {
        switch (_currentStat)
        {
            case GameStat.PreStart:
                break;
            case GameStat.Started:
                foreach(GameObject GO in _allSpawners)
                {
                    GO.SetActive(false);
                }
                
                break;
            case GameStat.Win:
                break;
            case GameStat.Loose:
                break;
            case GameStat.TimeUp:
                break;
        }
    }
    void _performStat()
    {
        switch (_currentStat)
        {
            case GameStat.PreStart:
                break;
            case GameStat.Started:

                // calculate the minuts pass based on _increaseStep. passing avery _increaseStep second counts as a minuts
                _minutsPass = (int)(Time.time / _increaseStep);
                // increase the time scale based on default _timeScale, the number of _minutsPass have passed and the _speedIncreaseFactor
                Time.timeScale = _timeScale + (_minutsPass * _speedIncreaseFactor);
                // filling up the path slider based on level progression.  uses (Time.time / Time.timeScale) to normilize time regardless of game speed
                _path.value += (Time.time / Time.timeScale);
                // check if the pass have been run
                if (_path.value >= _path.maxValue)
                {
                    _changeStat(GameStat.TimeUp);
                    //Debug.Log("Game Won!");
                }

                break;
            case GameStat.Win:
                break;
            case GameStat.Loose:
                break;
            case GameStat.TimeUp:
                break;
        }
    }
}
