using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkle;

    public void Sparkle()
    {
        StartCoroutine(SparkleAsync());
    }

    public IEnumerator SparkleAsync()
    {
        yield return new WaitForSecondsRealtime(.5F);
        sparkle.Play();
    }
}
