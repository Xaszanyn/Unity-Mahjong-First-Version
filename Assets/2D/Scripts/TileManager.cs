using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class TileManager : MonoSingleton<TileManager>
{
    [SerializeField] List<Tile> tiles;
    [SerializeField] private List<Slot> slots;
    int slotPointer;

    [SerializeField] int layers;

    int sum;

    List<Tile> selecteds = new List<Tile>();
    [SerializeField] Sprite[] tileSprites;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameManager GM;

    private List<int> levelNumbers = new List<int>();

    [SerializeField] int difficulty;

    [SerializeField] Bar bar;

    public int score;
    float multiplier;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        sum = 0;
        slotPointer = 0;
        text.text = "0";
        scoreText.text = "0";
        multiplier = 1;
    }

    public void Initialize(List<Tile> levelTiles)
    {
        tiles = levelTiles;
        StartCoroutine(FindLockTiles());
        CreateLevel();
    }

    private void CreateLevel()
    {
        CreateRandoms();

        for (int i = 0; i < tiles.Count; i++)
        {
            var number = levelNumbers[i];
            tiles[i].SetTileValue(number, tileSprites[number - 1]);
            tiles[i].OnPop += Clicked;
        }

        void CreateRandoms()
        {
            levelNumbers.Clear();
            int count = 0;
            for (int i = 0; i < tiles.Count; i++)
            {
                int num = Random.Range(1, 12);
                levelNumbers.Add(num);
                count += num;
            }

            if (count % 21 == 0) return;
            else CreateRandoms();
        }
    }

    private IEnumerator FindLockTiles()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var element in tiles)
            element.FindLockTiles();
    }

    public void Clicked(Tile tile)
    {
        if (slotPointer != slots.Count && !tile.isLocked)
        {
            tiles.Remove(tile);
            selecteds.Add(tile);

            sum += tile.value;
            text.text = sum.ToString();

            tile.Move(slots[slotPointer++].transform);

            Check();
            //UnlockOthers(); :(
        }
    }

    void Check()
    {
        if (slotPointer == slots.Count)
        {
            StartCoroutine(FailCheck());
        }

        if (sum == 21)
        {
            Hit21();

            for (int i = 0; i < selecteds.Count; i++)
            {
                StartCoroutine(selecteds[i].Disappear(i));
                slots[i].Sparkle();
            }

            selecteds.Clear();
            slotPointer = 0;
            sum = 0;
        }
        else if (sum > 21) GM.Fail();

        void Hit21()
        {
            text.text = "21";
            text.color = Color.green;
            RectTransform textRT = text.gameObject.GetComponent<RectTransform>();

            float plus = 10;

            if (bar.multiply)
            {
                multiplier += .5F;
                plus *= multiplier;
                bar.Kill();
                bar.Shrink();
            }
            else bar.Shrink();

            score += (int)plus;

            scoreText.text = score.ToString();


            textRT.DOScale(new Vector2(1.5F, 1.5F), .25F)
                .OnComplete(() => textRT.DOScale(new Vector2(1, 1), .25F)
                .OnComplete(Normalize));

            void Normalize()
            {
                text.text = "0";
                text.color = Color.white;
            }
        }

        IEnumerator FailCheck()
        {
            yield return new WaitForSecondsRealtime(.5F);
            if (slotPointer == slots.Count) GM.Fail();
        }
    }

    void GenerateRandomNumbers()
    {
        /*                           KOLAYLAÞTIRMAK ÝÇÝN VARDI ARTIK YOK
           TAM ZOR - [80]
           ZOR [55] [S25]
           ORTA [30] [25] [S25]
           KOLAY [14] [41] [S25]
        */


        List<int> generatedRandomNumbers = new List<int>();

        if (difficulty == 3) GenerateIV();
        else if (difficulty == 2) GenerateIII();
        else if (difficulty == 1) GenerateII();
        if (difficulty == 0) GenerateI();

        levelNumbers = generatedRandomNumbers;


        void GenerateIV()
        {
            Debug.Log("GENERATING WHOLE LEVEL...");
            int count = 0;
            for (int i = 1; i <= 80; i++) // 80 deðiþicek
            {
                int value = Random.Range(1, 12);
                count += value;
                generatedRandomNumbers.Add(value);
                //randomNumbers.Add(value);
            }

            if (count % 21 != 0)  // BURA ZATEN FULL GÝDÝCEK USTA
            {
                //randomNumbers.Clear();
                generatedRandomNumbers.Clear();
                GenerateIV();
            }
        }

        void GenerateIII()
        {
            Debug.Log("GENERATING SECOND PYRAMID PART...");

            int count = 0;
            for (int i = 1; i <= 55; i++)
            {
                int value = Random.Range(1, 12);
                count += value;
                generatedRandomNumbers.Add(value);
            }

            if (count % 21 != 0)
            {
                generatedRandomNumbers.Clear();
                GenerateIII();
            }
            else GenerateStacks();
        }

        void GenerateII()
        {
            Debug.Log("GENERATING FIRST PYRAMID PART...");

            int count = 0;
            for (int i = 1; i <= 30; i++)
            {
                int value = Random.Range(1, 12);
                count += value;
                generatedRandomNumbers.Add(value);
            }

            if (count % 21 != 0)
            {
                generatedRandomNumbers.Clear();
                GenerateII();
            }
            else ContinueGenerateII();

            void ContinueGenerateII()
            {
                Debug.Log("GENERATING SECOND PYRAMID PART...");

                List<int> temporaryList = new List<int>();
                int count = 0;
                for (int i = 31; i <= 55; i++)
                {
                    int value = Random.Range(1, 12);
                    count += value;
                    temporaryList.Add(value);
                }

                if (count % 21 != 0) ContinueGenerateII();
                else
                {
                    foreach (int number in temporaryList)
                    {
                        generatedRandomNumbers.Add(number);
                    }

                    GenerateStacks();
                }
            }
        }

        void GenerateI()
        {
            Debug.Log("GENERATING FIRST BASIC PYRAMID PART...");

            int count = 0;
            for (int i = 1; i <= 14; i++)
            {
                int value = Random.Range(1, 12);
                count += value;
                generatedRandomNumbers.Add(value);
            }

            if (count % 21 != 0)
            {
                generatedRandomNumbers.Clear();
                GenerateI();
            }
            else ContinueGenerateI();

            void ContinueGenerateI()
            {
                Debug.Log("GENERATING SECOND PYRAMID PART...");

                List<int> temporaryList = new List<int>();
                int count = 0;
                for (int i = 15; i <= 55; i++)
                {
                    int value = Random.Range(1, 12);
                    count += value;
                    temporaryList.Add(value);
                }

                if (count % 21 != 0) ContinueGenerateI();
                else
                {
                    foreach (int number in temporaryList)
                    {
                        generatedRandomNumbers.Add(number);
                    }

                    GenerateStacks();
                }
            }
        }

        void GenerateStacks()
        {
            Debug.Log("GENERATING STACKS...");

            List<int> temporaryList = new List<int>();
            int count = 0;
            for (int i = 56; i <= 80; i++)
            {
                int value = Random.Range(1, 12);
                count += value;
                temporaryList.Add(value);
            }

            if (count % 21 != 0) GenerateStacks();
            else
            {
                foreach (int number in temporaryList)
                {
                    generatedRandomNumbers.Add(number);
                }
            }
        }
    } // out of order
}
