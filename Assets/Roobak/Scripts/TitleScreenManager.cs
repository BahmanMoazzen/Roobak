using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Primitives")]
    /// the animator of the fox
    [SerializeField] Animator _anim;
    [Header("UI Texts")]
    /// the Text compounent of the name of the dance
    [SerializeField] Text _danceNameText;
    /// <summary>
    /// the Text compounent of the XP level
    /// </summary>
    [SerializeField] Text _XPLevelText;
    /// <summary>
    /// the Text compounent of the start button
    /// </summary>
    [SerializeField] Text _startText;
    [Header("UI Buttons")]
    /// the select button
    [SerializeField] GameObject _selectButton;
    /// <summary>
    /// the unlock button
    /// </summary>
    [SerializeField] GameObject _unlockButton;
    /// <summary>
    /// the base index of the clip
    /// </summary>
    int _currentClipIndex = 0;
    /// <summary>
    /// the default position of the fox
    /// </summary>
    Vector3 _currentPosition;
    /// <summary>
    /// the current dance info for the fox
    /// </summary>
    DanceInfo _currentDanceInfo;
    private void Start()
    {
        _XPLevelText.text = string.Format(_XPLevelText.text, BAHMANXPManager._Instance._GetCurrentLevel());
        _startText.text =  GameSettingInfo.Instance.CurrentGameLevel.ToString();
        _currentPosition = _anim.transform.position;
        _setDance();
    }
    public void _NextClip()
    {
        _currentClipIndex++;
        if (_currentClipIndex >= GameSettingInfo.Instance.DanceClips.Length)
        {
            _currentClipIndex = 0;
        }
        _setDance();
    }
    private void _setDance()
    {
        _currentDanceInfo = GameSettingInfo.Instance.DanceClips[_currentClipIndex];
        _danceNameText.text = BAHMANLanguageManager._Instance._Translate( _currentDanceInfo.DanceName);
        _anim.transform.rotation = Quaternion.identity;
        _anim.transform.position = _currentPosition;
        if (_currentDanceInfo.IsLocked)
        {
            _selectButton.SetActive(false);
            _unlockButton.SetActive(true);
            _unlockButton.GetComponentInChildren<Text>().text = _currentDanceInfo.UnlockButtonLabelTranslated;
            _anim.SetFloat("DanceType", GameSettingInfo.Instance.DefaultDanceClipIndex);
        }
        else
        {
            _selectButton.SetActive(true);
            _unlockButton.SetActive(false);
            _anim.SetFloat("DanceType", _currentClipIndex);
            
        }
    }
    public void _PreviousClip()
    {
        _currentClipIndex--;
        if (_currentClipIndex < 0)
        {
            _currentClipIndex = GameSettingInfo.Instance.DanceClips.Length - 1;
        }
        _setDance();
    }
    public void _LoadSavedDance()
    {
        _currentClipIndex = GameSettingInfo.Instance.CurrentDanceClipIndex;
        _setDance();
    }
    public void _SaveDance()
    {
        GameSettingInfo.Instance.CurrentDanceClipIndex = _currentClipIndex;
        _currentClipIndex = 0;
        _setDance();
    }
    public void _StartGame()
    {
        BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.GameScene);
    }
    public void _PlayClickSound()
    {
        BAHMANSoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
    }
    public void _PlaySlideSound()
    {
        BAHMANSoundManager._Instance._PlaySound(GameSounds.ButtonSlide);
    }
    public void _UnlockDance()
    {
        _currentDanceInfo.Unlock(_unlockSuccess, _unlockFailed);
    }
    void _unlockFailed()
    {
        BAHMANMessageBoxManager._INSTANCE._ShowMessage("Unlock Failed", Color.red);
    }
    void _unlockSuccess()
    {
        _setDance();
        _selectButton.SetActive(true);
        _unlockButton.SetActive(false);
        BAHMANMessageBoxManager._INSTANCE._ShowMessage("Unlock Success", Color.green);
    }
}
