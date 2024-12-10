using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharactorController : MonoBehaviour
{
    [SerializeField] private char _char; 
    private TextMeshPro _txt;
    private GameObject circle;

    void Start()
    {
        circle = transform.GetChild(0).gameObject;
        _txt = GetComponent<TextMeshPro>();
        _char = _txt.text[0];
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.canSelectChar)
        {
            circle.SetActive(true); 
            CharactorManager.Instance.StartTracking(_char, transform); 
        }
    }

    private void OnMouseEnter()
    {
        if (CharactorManager.Instance.IsTracking)
        {
            CharactorManager.Instance.AddChar(_char, transform); 
            circle.SetActive(true); 
        }
    }

    private void OnMouseUp()
    {
        circle.SetActive(false); 
        CharactorManager.Instance.EndTracking();
    }
}
