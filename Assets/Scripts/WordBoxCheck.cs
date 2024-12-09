using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBoxCheck : MonoBehaviour
{
    private void OnMouseDown() {
        if(CharactorManager.Instance.canSelectBox){
            GameObject child = this.gameObject.transform.GetChild(0).gameObject;
            if(!child.activeSelf){
                CharactorManager.Instance.ActivateSelectedCharactor(child);
                CharactorManager.Instance.canSelectBox = false;
            }
        }
    }
}
