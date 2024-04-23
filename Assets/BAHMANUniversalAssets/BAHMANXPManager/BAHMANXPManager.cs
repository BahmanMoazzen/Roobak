using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BAHMANXPManager : MonoBehaviour
{
    const string TAG = "XPManagerTag";
    public static BAHMANXPManager _Instance;
    /// <summary>
    /// Invokes whenever Player levels up
    /// </summary>
    public static event UnityAction<int, int> OnLevelUp;
    List<levelStructure> _levelSteps;
    [SerializeField] int _stepLevel = 25;
    [SerializeField] int _baseLevel = 0;
    [SerializeField] float _baseXP = 0;

    float _currentXP
    {
        get
        {
            return PlayerPrefs.GetFloat(TAG, _baseXP);
        }
        set
        {
            PlayerPrefs.SetFloat(TAG, value);
        }
    }

    //IEnumerator _loadXPLevels()
    //{
    //    yield return null;
    //    _levelSteps = new List<levelStructure>();
    //    _levelSteps.Add(new levelStructure(0, _stepLevel));
    //    for (int i = 1; i < 120; i++)
    //    {

    //        levelStructure ls;

    //        ls.StartXP = _levelSteps[i - 1].EndXP + 1;
    //        ls.EndXP = _levelSteps[i - 1].EndXP + (Math.Pow(_stepLevel, i));
    //        _levelSteps.Add(ls);

    //    }
    //    yield return null;
    //}
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _levelSteps = new List<levelStructure>();
        _levelSteps.Add(new levelStructure(0, _stepLevel));
        for (int i = 1; i < 120; i++)
        {

            _levelSteps.Add(
                new levelStructure(
                _levelSteps[i - 1].EndXP + 1,
                _levelSteps[i - 1].EndXP + Mathf.Pow(_stepLevel, i)
                )
                );

        }
    }
    private void Start()
    {
        Debug.Log("CurrentLevel: " + _GetCurrentLevel() + " CurrentXP: " + _currentXP);
    }
    /// <summary>
    /// calculates the current level of the player
    /// </summary>
    /// <returns>the current level of the player</returns>
    public int _GetCurrentLevel()
    {
        return _XPToLevel(_currentXP);
    }
    public void _SetExperience(int iXPAmount)
    {
        int currentLevel = _XPToLevel(_currentXP);

        _currentXP += iXPAmount;
        int nextLevel = _XPToLevel(_currentXP);

        if (nextLevel > currentLevel)
        {
            Debug.Log("Level Up");
            OnLevelUp?.Invoke(currentLevel, nextLevel);
        }

    }
    /// <summary>
    /// The value to set to the slider for diplaying purposes
    /// </summary>
    /// <returns>the slider value</returns>
    public float _GetSliderValue()
    {
        levelStructure _currentLevel = _levelSteps[_GetCurrentLevel()];
        float range = _currentLevel.EndXP - _currentLevel.StartXP;
        return (_currentXP - _currentLevel.StartXP) / range;

    }
    int _XPToLevel(float iCurrentXP)
    {
        int currentLevel = _baseLevel;

        foreach (levelStructure item in _levelSteps)
        {
            if (iCurrentXP >= item.StartXP && iCurrentXP <= item.EndXP)
            {
                return currentLevel;
            }
            currentLevel++;
        }
        return -1;
    }
}
[System.Serializable]
struct levelStructure
{
    public float StartXP;
    public float EndXP;
    public levelStructure(float iStart, float iEnd)
    {
        StartXP = iStart;
        EndXP = iEnd;
    }
}
