using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TileSet : MonoBehaviour
{
    public List<Tilee> allTiles;
    List<Tilee> selectedTiles = new List<Tilee>();
    int topLayer = 0;
    int stackTopLayer = 0;

    [SerializeField] List<GameObject> slots;
    int slotPoint = 0;

    int sum = 0;

    [SerializeField] Text text;

    [SerializeField] GameObject sparkle;

    public void Initialize()
    {
        //TopLayer();
        //StackTopLayer();
        topLayer = 4;
        stackTopLayer = 4;

        //UnlockTop();
        //UnlockStack();
    }

    /*
    void TopLayer()
    {
        foreach(Tile tile in allTiles) // NEDENSE YENÝSÝNDE ÇALIÞMADI
        {
            Debug.Log("döngü");
            if (!tile.stacked && topLayer < tile.layer)
            {
                Debug.Log("Found superior " + tile.name + " " + topLayer);
                topLayer = tile.layer;
            }
        }
    }

    void StackTopLayer()
    {
        foreach (Tile tile in allTiles)
        {
            if (tile.stacked && stackTopLayer < tile.layer)
            {
                stackTopLayer = tile.layer;
            }
        }

        Debug.Log("Stack Top Layer is " + topLayer);
    }

    void UnlockTop()
    {
        foreach(Tile tile in allTiles)
        {
            if (tile.stacked) continue;

            if (tile.layer == topLayer)
            {
                tile.locked = false;
                continue;
            }

            tile.locked = false;
            foreach(Tile above in allTiles)
            {
                if (above.layer == tile.layer + 1)
                {
                    //Debug.Log(tile.name + " - " + above.name);
                    bool LR = above.x == tile.x - .5F || above.x == tile.x + .5F;
                    bool TB = above.z == tile.z - .5F || above.z == tile.z + .5F;
                    //Debug.Log("lr: " + LR + " tb: " + TB);
                    if (LR && TB) tile.locked = true;
                }
            }
        }
    }

    void UnlockStack()
    {
        foreach(Tile tile in allTiles)
        {
            if (!tile.stacked) continue;

            if (tile.layer == stackTopLayer)
            {
                tile.locked = false;
                continue;
            }

            tile.locked = false;
            foreach (Tile above in allTiles)
            {
                if (above.layer == tile.layer + 1)
                {
                    if (above.x == tile.x && above.z == tile.z) tile.locked = true;
                }
            }
        }
    }

    */

    public void Clicked(Tilee tile)
    {
        if (slotPoint == slots.Count) return;

        if (!tile.locked)
        {
            allTiles.Remove(tile);
            selectedTiles.Add(tile);

            tile.gameObject.transform.DOMove(slots[slotPoint++].transform.position + Vector3.up * .15F, .5F);
            sum += tile.type;
            Debug.Log(sum);
            text.text = sum.ToString();
            CheckSlots();

            if (tile.stacked)
            {
                var x = tile.x;
                var z = tile.z;
                var layer = tile.layer;

                foreach (Tilee other in allTiles)
                {
                    if (other.stacked && x == other.x && z == tile.z && layer - 1 == other.layer)
                    {
                        other.locked = false;
                    }
                }
            }
            else
            {
                var x = tile.x;
                var z = tile.z;
                var layer = tile.layer;

                bool leftAllowed = true;
                bool rightAllowed = true;
                bool topAllowed = true;
                bool bottomAllowed = true;

                foreach (Tilee other in allTiles)
                {
                    if (leftAllowed)
                    {
                        if (other.x == x - 1 && other.layer == layer && other.z == z)
                        {
                            leftAllowed = false;
                        }
                    }
                    if (rightAllowed)
                    {
                        if (other.x == x + 1 && other.layer == layer && other.z == z)
                        {
                            rightAllowed = false;
                        }
                    }
                    if (topAllowed)
                    {
                        if (other.x == x && other.layer == layer && other.z == z - 1)
                        {
                            topAllowed = false;
                        }
                    }
                    if (bottomAllowed)
                    {
                        if (other.x == x && other.layer == layer && other.z == z + 1)
                        {
                            bottomAllowed = false;
                        }
                    }
                }

                foreach (Tilee other in allTiles)
                {
                    if (other.layer == layer - 1)
                    {
                        bool left = other.x == x - .5F && leftAllowed;
                        bool right = other.x == x + .5F && rightAllowed;
                        bool top = other.z == z - .5F && topAllowed;
                        bool bottom = other.z == z + .5F && bottomAllowed;

                        //isLeftRight && isTopBottom
                        if ((left || right) && (bottom || top))
                        {
                            other.locked = false;
                            //Debug.Log("Unlocked tile at " + other.x + "-" + other.z + "-" + other.layer);
                        }
                    }
                }
            }
        }
    }

    void CheckSlots()
    {
        if (sum == 21)
        {
            Debug.Log("!21!");
            foreach(Tilee tile in selectedTiles)
            {
                StartCoroutine(Disappear(tile.gameObject.transform));
                Destroy(tile.gameObject, 1);
            }
            selectedTiles.Clear();
            slotPoint = 0;
            sum = 0;
        }
        else if (sum > 21)
        {
            Time.timeScale = 0;
            Debug.LogError("YOU'RE DEAD BOI!");
        }

        IEnumerator Disappear(Transform transform)
        {
            yield return new WaitForSecondsRealtime(.5F);
            transform.DOScale(Vector3.zero, .25F);
            var sparkles = Instantiate(sparkle, transform.position, Quaternion.identity);
            Destroy(sparkles, 2);
        }
    }
}
