using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct GameSoundStructure
{
    public GameSounds Sound;
    public List<AudioClip> AudioClips;
}
public enum GameSounds { Step, GameOver, ButtomClicked, ButtonSlide, SceneTransition ,Pickup , Hit ,StartGame,EndGame}
public class BAHMANSoundManager : MonoBehaviour
{
    public static BAHMANSoundManager _Instance;
    [SerializeField] GameSoundStructure[] _sounds;
    [SerializeField] AudioSource _audioSource;
    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);



    }
    private void OnEnable()
    {

        GameSettingInfo.OnSoundFXChange += GameSettingInfo_OnSoundFXChange;
        GameSettingInfo.OnMusicChange += GameSettingInfo_OnMusicChange;
    }

    private void GameSettingInfo_OnMusicChange(bool iEnable)
    {

    }

    private void OnDisable()
    {
        GameSettingInfo.OnSoundFXChange -= GameSettingInfo_OnSoundFXChange;
        GameSettingInfo.OnMusicChange -= GameSettingInfo_OnMusicChange;
    }

    private void GameSettingInfo_OnSoundFXChange(bool iEnable)
    {
        if (iEnable)
        {
            //_audioSource.PlayOneShot(_sounds[(int)GameSounds.FirstMerge].AudioClips[0]);
        }

    }

    public void _PlaySound(GameSounds iSound)
    {
        if (GameSettingInfo.Instance.SoundFX)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Sound == iSound)
                {
                    _audioSource.PlayOneShot(sound.AudioClips[UnityEngine.Random.Range(0, sound.AudioClips.Count)]);
                }
            }
        }

    }

}


