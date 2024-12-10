using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Hyb.Utils;

public class BtnManager : ManualSingletonMono<BtnManager>
{
    [SerializeField] private List<Transform>    charactors; 
    [SerializeField] private GameObject         changeBtn;       
    private List<Vector3>                       initialPositions;             
    private bool                                canChange = true;                     
    void Start()
    {
        initialPositions = new List<Vector3>();
        foreach (Transform charactor in charactors)
        {
            initialPositions.Add(charactor.position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == changeBtn && GameManager.Instance.canSelectChar)
                {
                    ChangePosCharactors();
                }
            }
        }
    }

    public void ChangePosCharactors()
    {
        if (!canChange) return;
        canChange = false; 
        int pairCount = Random.Range(1, (charactors.Count / 2) + 1);
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < charactors.Count; i++)
        {
            availableIndices.Add(i);
        }
        for (int i = 0; i < pairCount; i++)
        {
            if (availableIndices.Count < 2) break;
            int index1 = Random.Range(0, availableIndices.Count);
            int charIndex1 = availableIndices[index1];
            availableIndices.RemoveAt(index1);

            int index2 = Random.Range(0, availableIndices.Count);
            int charIndex2 = availableIndices[index2];
            availableIndices.RemoveAt(index2);
            StartCoroutine(ChangeCoroutine(charactors[charIndex1], charactors[charIndex2], charIndex1, charIndex2));
        }
        StartCoroutine(ResetChangeCooldown());
    }

    IEnumerator ChangeCoroutine(Transform char1, Transform char2, int index1, int index2)
    {
        char1.DOScale(1.2f, 0.1f).SetEase(Ease.InOutQuad);
        char2.DOScale(1.2f, 0.1f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.1f);
        Vector3 targetPos1 = initialPositions[index2];
        Vector3 targetPos2 = initialPositions[index1];

        char1.DOMove(targetPos1, 0.25f).SetEase(Ease.InOutQuad);
        char2.DOMove(targetPos2, 0.25f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.25f);
        char1.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad);
        char2.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad);
        charactors[index1] = char2;
        charactors[index2] = char1;
    }

    IEnumerator ResetChangeCooldown()
    {
        yield return new WaitForSeconds(.5f); 
        canChange = true; 
    }
}
