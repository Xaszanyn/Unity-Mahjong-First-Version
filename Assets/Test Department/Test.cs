using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    RaycastHit2D[] raycastHit;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        raycastHit = Physics2D.RaycastAll(transform.position, Vector2.zero, 1.5f);
        for (int i = 0; i < raycastHit.Length; i++)
        {
            Debug.Log(raycastHit[i].collider.gameObject.name);
        }
    }
}
