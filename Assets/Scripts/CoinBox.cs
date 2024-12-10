using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{
    [SerializeField] private GameObject coinPref;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Char")){
            GameObject coin =  Instantiate(coinPref,this.transform.position,Quaternion.identity);
            coin.GetComponent<CoinEff>().SetText(10);
            Destroy(this.gameObject);
        }
    }
}
