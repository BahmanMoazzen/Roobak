using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{

    public static CameraManager _Instance;

    [SerializeField] Camera _camera;
    [SerializeField] CameraInformations[] _allCameras;


    // Start is called before the first frame update

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        _camera = Camera.main;
        _allCameras[0].Cam.gameObject.SetActive(true);

    }

    //public void _MakeOnLine(GameCameras iOnlineCamera)
    //{
    //    foreach (var cam in _allCameras)
    //    {
            
    //        if (cam.CamName == iOnlineCamera)
    //        {
    //            cam.Cam.gameObject.SetActive(true); 
    //            _camera.cullingMask = cam.ShowMask;
                
    //        }
    //        else
    //        {
    //            cam.Cam.gameObject.SetActive(false);
    //            cam.Cam.Priority = 0;
    //        }

    //    }

    //}
}
[System.Serializable]
public struct CameraInformations
{
    public CinemachineVirtualCamera Cam;
    public GameCameras CamName;
    public LayerMask ShowMask;

}

public enum GameCameras { MainCam, DanceCam }
