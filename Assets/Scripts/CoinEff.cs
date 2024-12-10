using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinEff : MonoBehaviour
{
    private Vector3 originPos;
    [SerializeField] private Transform targetPos;
    [SerializeField] private TextMeshPro coinTxt;
    private void Awake() {
        targetPos = GameObject.Find("TargetPos").transform;
    }
    void Start()
    {
        originPos = this.transform.position;
        StartCoroutine(MoveEffect());
    }
    IEnumerator MoveEffect(){
        this.transform.DOScale(0.4f,0.5f).SetEase(Ease.Linear);
        this.transform.DOMove(originPos + Vector3.right*0.5f,0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        this.transform.DOScale(0.5f,0.5f).SetEase(Ease.Linear);
        this.transform.DOMove(targetPos.position,0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.IncreaseCoin(10);
        Destroy(this.gameObject);
    }
    public void SetText(int amount){
        coinTxt.text = "+" + amount.ToString();
    }

}
