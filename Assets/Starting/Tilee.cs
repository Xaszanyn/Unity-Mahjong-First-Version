using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilee : MonoBehaviour
{
    public float x, z;
    public int layer;

    public int type;

    public bool locked;

    public bool stacked;

    void Start()
    {
        transform.localPosition = new Vector3(x, layer * .3F, -z * 1.25F);
    }

    void OnMouseDown()
    {
        gameObject.transform.parent.gameObject.GetComponent<TileSet>().Clicked(this);
    }
}
