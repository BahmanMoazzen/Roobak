using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKUReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string result = string.Empty;
        foreach (var DI in GameSettingInfo.Instance.DanceClips)
        {
            result += string.Format("{0};20000;{1};true \n", DI.SKU, BAHMANLanguageManager._Instance._Translate(DI.DanceName));

        }
        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
