using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject failScreen;
    [SerializeField] private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader.LoadLevel();
    }

    public void Fail()
    {
        blackScreen.SetActive(true);
        blackScreen.GetComponent<Image>().DOFade(.5F, .5F);
        failScreen.SetActive(true);
        failScreen.GetComponent<RectTransform>().DOAnchorPosY(0, .5F);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
