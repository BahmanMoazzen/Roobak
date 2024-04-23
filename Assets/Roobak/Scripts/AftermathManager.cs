using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AftermathManager : MonoBehaviour
{
    [SerializeField] Animator _anim;
    // 0 for loose , 1 for win
    [SerializeField] PlayableDirector[] _loadingAnimations;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _currentLevelText;
    [SerializeField] Text _XPLevelText;
    [SerializeField] Slider _XPSlider;
    [SerializeField] GameObject _againButton;

    int _thisRunScore;
    private void Awake()
    {
        Time.timeScale = 1;
        int sadnessType = Random.Range(0, 4);
        _anim.SetFloat("SadnessType", sadnessType);
        _anim.SetBool("IsWon", GameSettingInfo.Instance.IsGameWon);
        _loadingAnimations[GameSettingInfo.Instance.IsGameWon ? 1 : 0].Play();
        _anim.SetFloat("DanceType", GameSettingInfo.Instance.CurrentDanceClipIndex);
        _thisRunScore = GameSettingInfo.Instance.ThisRunScore;
        _currentLevelText.text = BAHMANLanguageManager._Instance._Translate(_currentLevelText.text).Replace("$$$", (GameSettingInfo.Instance.CurrentGameLevel - 1).ToString());
        _scoreText.text = _thisRunScore.ToString();
        _XPLevelText.text = BAHMANXPManager._Instance._GetCurrentLevel().ToString();
        _XPSlider.value = BAHMANXPManager._Instance._GetSliderValue();
    }
    public void _StartCountDown()
    {
        Debug.Log("CountDownStarted");
        StartCoroutine(_countDownRoutine());
    }

    public void _TryAgain()
    {
        BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.TitleScreenScene);
    }
    IEnumerator _countDownRoutine()
    {
        const byte step = 5;
        for (int i = _thisRunScore; i >= 0; i -= step)
        {
            _scoreText.text = i.ToString();
            BAHMANXPManager._Instance._SetExperience(step);
            _XPSlider.value = BAHMANXPManager._Instance._GetSliderValue();
            //int currentXPLevel = BAHMANXPManager._Instance._GetCurrentLevel();
            //if (!_XPLevelText.text.Equals(currentXPLevel.ToString()))
            //{
            //    // new level
            //    Debug.Log("Level Up!");
            //}
            _XPLevelText.text = BAHMANXPManager._Instance._GetCurrentLevel().ToString();
            yield return null;
        }

        _scoreText.text = "0";
        for (float i = 0; i <= 1; i += .1f)
        {
            _againButton.GetComponent<CanvasGroup>().alpha = i;
            yield return null;
        }
        _againButton.GetComponent<CanvasGroup>().alpha = 1f;

    }

    private void OnEnable()
    {
        BAHMANXPManager.OnLevelUp += BAHMANXPManager_OnLevelUp;
    }
    private void OnDisable()
    {
        BAHMANXPManager.OnLevelUp -= BAHMANXPManager_OnLevelUp;
    }
    private void BAHMANXPManager_OnLevelUp(int arg0, int arg1)
    {
        Debug.Log("Level Up Sound!");
    }
}
