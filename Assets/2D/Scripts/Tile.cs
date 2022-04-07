using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public UnityAction<Tile> OnPop;
    public const float cleanupDelay = 0.1f;

    public int value { get { return Value; } }
    [SerializeField] int Value;

    public bool isLocked { get { return IsLocked; } }
    [SerializeField] bool IsLocked;

    [SerializeField] private Transform[] corners;
    List<Tile> blockerTiles;

    [SerializeField] private SpriteRenderer sr;

    void Start()
    {

        blockerTiles = new List<Tile>();
    }

    public void SetTileValue(int value, Sprite image)
    {
        sr.sprite = image;
        Value = value;
    }

    public void Unlock()
    {
        GetComponent<SpriteRenderer>().DOColor(Color.white, .5F);
        IsLocked = false;
    }

    public void Move(Transform position)
    {
        IsLocked = true;
        transform.position += Vector3.back * 10;
        transform.DOMove(position.position, .5F).SetEase(Ease.OutQuart);
        transform.DOScale(new Vector2(.65F, .65F), .5F).SetEase(Ease.OutQuart);
    }

    public IEnumerator Disappear(int delayIndex)
    {
        yield return new WaitForSecondsRealtime(.05F);
        transform.DOScale(Vector2.zero, .5F)
            .SetEase(Ease.InBack)
            .SetDelay(cleanupDelay * (delayIndex + 1))
            .OnComplete(() => Destroy(gameObject));
    }

    void OnMouseDown()
    {
        //TileManager.instance.Clicked(this);
        OnPop?.Invoke(this);
    }

    public void FindLockTiles()
    {
        GetAllHits();
        SubscribeToBlockers();
        CheckUnlock();
    }
    private void GetAllHits()
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(corners[i].transform.position, Vector3.forward);
            foreach (var element in hits)
            {
                if (element.collider.transform.position.z < transform.position.z - 0.1f && element.collider.transform.position.z > transform.position.z - 1.2f)
                {
                    var tile = element.collider.GetComponent<Tile>();
                    if (tile != null && !blockerTiles.Contains(tile))
                    {
                        blockerTiles.Add(tile);
                    }
                }
            }
        }
    }

    private void CheckUnlock()
    {
        if (blockerTiles.Count == 0)
        {
            Unlock();
        }
    }

    private void SubscribeToBlockers()
    {
        foreach (var element in blockerTiles)
        {
            element.OnPop += OnBlockerPop;
        }
    }

    private void OnBlockerPop(Tile tile)
    {
        blockerTiles.Remove(tile);
        if (blockerTiles.Count == 0)
        {
            Unlock();
        }
    }
}
