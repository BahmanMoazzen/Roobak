using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RuralSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _spawnPrefab;
    [SerializeField] bool _randomSpawn = true;
    [SerializeField] float _spawnInterval;
    [SerializeField] bool _copyRotation;
    [SerializeField] SpawnTypes _spawnType;
    [SerializeField] Transform _left, _right;
    [SerializeField] Transform[] _spawnLocations;
    //[SerializeField] Dictionary<string, List<GameObject>> _pool;
    [SerializeField] PoolManager _poolManager;
    float _nextSpawn = 0;
    int _prefabIndex = 0;
    int _spawnLocationIndex = 0;
    private void Awake()
    {
        if (_spawnPrefab.Length <= 0)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        //_pool = new Dictionary<string, List<GameObject>>();
    }

    void Update()
    {
        if (Time.time > _nextSpawn)
        {
            _nextSpawn = Time.time + _spawnInterval;


            Vector3 position = Vector3.zero;
            Quaternion rotation;

            if (_copyRotation)
            {
                rotation = transform.rotation;
            }
            else
            {
                rotation = Quaternion.identity;
            }
            switch (_spawnType)
            {
                case SpawnTypes.RangeSpawn:
                    position = new Vector3(Random.Range(_left.transform.position.x, _right.transform.position.x), transform.position.y, transform.position.z);
                    break;
                case SpawnTypes.SingleLocation:
                    position = transform.position;
                    break;
                case SpawnTypes.CertainLocationRandom:
                    position = new Vector3(_spawnLocations[Random.Range(0, _spawnLocations.Length)].position.x, transform.position.y, transform.position.z); 
                    break;
                case SpawnTypes.CertainLocationsInRow:
                    position = new Vector3(_spawnLocations[_spawnLocationIndex].position.x, transform.position.y, transform.position.z);

                    _spawnLocationIndex++;
                    if (_spawnLocationIndex >= _spawnLocations.Length)
                    {
                        _spawnLocationIndex = 0;
                    }
                    break;
                    

            }


            if (_randomSpawn)
            {
                _poolManager._Instantiate(_spawnPrefab[Random.Range(0, _spawnPrefab.Length)], position, rotation);
            }
            else
            {
                _poolManager._Instantiate(_spawnPrefab[_prefabIndex], position, rotation);
                _prefabIndex++;
                if (_prefabIndex >= _spawnPrefab.Length)
                {
                    _prefabIndex = 0;
                }
            }

        }
    }

    //private void _Instantiate(GameObject iGameObject, Vector3 iPosition, Quaternion iRotation)
    //{
    //    if (_pool.ContainsKey(iGameObject.name))
    //        foreach (GameObject go in _pool[iGameObject.name])
    //        {
    //            if (!go.activeInHierarchy)
    //            {
    //                go.transform.position = iPosition;
    //                go.transform.rotation = iRotation;
    //                go.SetActive(true);
    //                return;
    //            }
    //        }
    //    else
    //    {
    //        _pool[iGameObject.name] = new List<GameObject>();
    //    }
    //    GameObject newGo = Instantiate(iGameObject, iPosition, iRotation);
    //    newGo.AddComponent<ObjectMover>();
    //    _pool[iGameObject.name].Add(newGo);
    //}
}

public enum SpawnTypes { RangeSpawn, SingleLocation, CertainLocationRandom, CertainLocationsInRow }
