using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebriesController : MonoBehaviour
{
    Text _nameText;
    public bool _IsBarrier;
    public int _Score;
    private void Awake()
    {
        _nameText = GetComponentInChildren<Text>();
        _nameText.text = string.Format("{0} {1}",_nameText.text , _IsBarrier ? "-1 HP" : "+" + _Score);
        _nameText.color = _IsBarrier ? Color.red : Color.green;
    }
}
