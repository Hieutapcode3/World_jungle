using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WordBoxCheck : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Coroutine scaleCoroutine;
    private Vector3 originScale;
    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        originScale = this.transform.localScale;
    }
    private void OnMouseDown() {
        if(CharactorManager.Instance.canSelectBox){
            GameObject child = this.gameObject.transform.GetChild(0).gameObject;
            if(!child.activeSelf){
                CharactorManager.Instance.ActivateSelectedCharactor(child);
                CharactorManager.Instance.canSelectBox = false;
                GameManager.Instance.DecreaseCoin(150);
                GameManager.Instance.canSelectChar = true;
                CharactorManager.Instance.UnActiveSelectEffect();
            }
        }
    }
    public void ActiveEfftect()
    {
        GameObject child = this.gameObject.transform.GetChild(0).gameObject;
        if (!child.activeSelf)
        {
            sprite.color = Color.green;
            scaleCoroutine = StartCoroutine(ScaleEffect()); 
        }
    }

    public void EndEffect()
    {
        sprite.color = Color.white;
        this.transform.DOKill();
        this.transform.localScale = originScale;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }
    }


    IEnumerator ScaleEffect(){
        while (true)
        {
            this.transform.DOScale(0.8f * originScale,1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
            this.transform.DOScale(1.05f * originScale,1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
        }
    }
}
