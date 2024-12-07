using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharactorController : MonoBehaviour
{
    [SerializeField] private char _char; 
    private TextMeshPro _txt;

    void Start()
    {
        _txt = GetComponent<TextMeshPro>();
        _char = _txt.text[0];
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down on: " + _char);
        CharactorManager.Instance.StartTracking(_char, transform); 
    }

    private void OnMouseEnter()
    {
        if (CharactorManager.Instance.IsTracking)
        {
            CharactorManager.Instance.AddChar(_char, transform); 
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up on: " + _char);
        CharactorManager.Instance.EndTracking();
    }
}
