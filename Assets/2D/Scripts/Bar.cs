using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bar : MonoBehaviour
{
    public bool multiply;

    public void Shrink()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        multiply = true;

        Sequence sequence = DOTween.Sequence();
        sequence.SetTarget(transform);

        sequence.Append(transform.DOScaleX(9, .25F));
        sequence.Append(transform.DOScaleY(.25F, .25F));
        sequence.Append(transform.DOScaleX(0, 4).SetEase(Ease.Linear).OnComplete(NormalizeIt));

        sequence.Play();
    }

    public void Kill()
    {
        transform.DOKill();
        Normalize(false);
    }

    void Normalize(bool start = true)
    {
        multiply = false;
        transform.localScale = new Vector3(9, start ? 0 : .25F, 1);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void NormalizeIt()
    {
        Normalize();
    }
}
