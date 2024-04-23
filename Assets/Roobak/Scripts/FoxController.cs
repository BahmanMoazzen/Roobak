using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    public static Transform _MyTransform;

    [SerializeField] Transform[] _Places;
    [SerializeField] float _SlideSpeed;
    [SerializeField] float _MinSlideDistance;
    [SerializeField] int _currentPosition = 2;
    [SerializeField] int _MaxLife;
    [SerializeField] GameManager _manager;
    int _currentLife;
    [SerializeField] Animator _animator;
    Transform _newPos;
    [SerializeField] GameObject _hitParticle, _collectParticle;

    private void Awake()
    {
        if (_MyTransform == null) { _MyTransform = transform; }
        _animator = GetComponent<Animator>();
        //GameManager.OnStatChanged += GameManager_OnStatChanged;
    }

    //private void GameManager_OnStatChanged(GameStat iFromStat, GameStat iToStat)
    //{
    //    switch (iToStat)
    //    {
    //        case GameStat.PreStart:
    //            break;
    //        case GameStat.Started:
    //            break;
    //        case GameStat.TimeUp:
    //            break;
    //        case GameStat.Win:
    //            _animator.Play(GameSettingInfo.Instance.CurrentDanceClipName);
    //            break;
    //        case GameStat.Loose:
    //            break;
    //    }
    //}

    private void Start()
    {
        _newPos = transform;
        _currentLife = _MaxLife;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _GoRight();

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _GoLeft();

        }
        if (Mathf.Abs(transform.position.x - _newPos.position.x) > _MinSlideDistance)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, _newPos.position.x, _SlideSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
    }

    public void _PlayFootstepSound()
    {
        BAHMANSoundManager._Instance._PlaySound(GameSounds.Step);
    }

    public void _GoRight()
    {
        _currentPosition = Mathf.Clamp(_currentPosition + 1, 0, _Places.Length - 1);
        _newPos = _Places[_currentPosition];
    }

    public void _GoLeft()
    {
        _currentPosition = Mathf.Clamp(_currentPosition - 1, 0, _Places.Length - 1);
        _newPos = _Places[_currentPosition];
    }


    private void OnTriggerEnter(Collider other)
    {
        DebriesController collided = other.GetComponent<DebriesController>();
        if (collided != null)
        {
            if (collided._IsBarrier)
            {
                BAHMANSoundManager._Instance._PlaySound(GameSounds.Hit);
                _hitParticle.SetActive(true);
                _currentLife--;
                _manager._updateHealth(_currentLife);
                if (_currentLife == 0)
                {
                    _manager._FoxDied();
                    //_animator.enabled = false;
                    //GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(0, 10));

                }
            }
            else
            {
                BAHMANSoundManager._Instance._PlaySound(GameSounds.Pickup);
                _collectParticle.SetActive(true);
                _manager._ChangeScore(collided._Score);
            }

            other.gameObject.SetActive(false);
        }
        else
        {

            if (other.CompareTag("EndLine"))
            {
                Debug.Log("End Touched");
                _manager._GameWon();
            }
            else if (other.CompareTag("StartLine"))
            {
                Debug.Log("Start Touched");
                _manager._StartGame();
            }
        }
    }
}
