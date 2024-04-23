using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Dictionary<string, List<GameObject>> _pool;
    private void Start()
    {
        if (_pool == null) _pool = new Dictionary<string, List<GameObject>>();
    }

    public GameObject _Instantiate(GameObject iObject, Vector3 iPosition, Quaternion iRotation)
    {
        if (_pool.ContainsKey(iObject.name))
            foreach (GameObject go in _pool[iObject.name])
            {
                if (!go.activeInHierarchy && go.name.StartsWith(iObject.name))
                {
                    go.transform.position = iPosition;
                    go.transform.rotation = iRotation;
                    go.SetActive(true);
                    return go;
                }
            }
        else
            _pool[iObject.name] = new List<GameObject>();

        GameObject newGo = Instantiate(iObject, iPosition, iRotation);
        newGo.AddComponent<ObjectMover>();
        _pool[iObject.name].Add(newGo);
        //int counter = 0;
        //foreach(var s in _pool.Keys)
        //{
        //    string debugstring = "";
        //    debugstring = s+": ";
        //    foreach(GameObject go in _pool[s])
        //    {
        //        counter++;
        //    }
        //    debugstring+=counter.ToString();
        //    Debug.Log(debugstring);
        //}
        //print(_pool.Count);
        return newGo;
    }
}
