using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour
{
    //[SerializeField] GameObject[] tiles;
    //[SerializeField] int width;
    //[SerializeField] int height;

    //public List<string> emptyList;

    public List<int> randomValues = new List<int>();

    List<Sloot> slotList = new List<Sloot>();

    public GameObject[] tiles;

    public Transform tileSet;
    TileSet TS;

    private void Start()
    {
        TS = tileSet.GetComponent<TileSet>();
        Slots();
        Create();
        Insert();
    }

    void Create()
    {
        int count = 0;
        for (int i = 1; i <= 80; i++) // 80 deðiþicek
        {
            int value = Random.Range(1, 12);
            count += value;
            randomValues.Add(value);
        }

        if (count % 24 != 0)
        {
            randomValues.Clear();
            Create();
        }
    }

    void Insert()
    {
        int count = 0;
        foreach(int num in randomValues)
        {
            var toInstantiate = NumTile(num);
            var tile = toInstantiate.GetComponent<Tilee>();

            Sloot slot = slotList[count];

            tile.x = slot.x;
            tile.layer = slot.layer;
            tile.z = slot.z;

            if (count == 54 || count == 59 || count == 64 || count == 69 || count == 74 || count == 79) tile.locked = false;
            else tile.locked = true;

            if (count < 55) tile.stacked = false;
            else tile.stacked = true;

            TS.allTiles.Add(Instantiate(toInstantiate, tileSet).GetComponent<Tilee>());

            count++;
        }

        TS.Initialize();
    }

    GameObject NumTile(int num) {
        return tiles[num - 1];
    }

    public class Sloot
    {
        public float x;
        public float z;
        public int layer;

        public Sloot(float x, float z, int layer)
        {
            this.x = x;
            this.z = z;
            this.layer = layer;
        }
    }

    void Slots()
    {

        for(int i = 0; i <= 4; i++)
        {
            for (int j = 0; j <= 4; j++)
            {
                slotList.Add(new Sloot(i, j, 0));
            }
        }

        for (float i = 0.5F; i <= 3.5F; i++)
        {
            for (float j = 0.5F; j <= 3.5F; j++)
            {
                slotList.Add(new Sloot(i, j, 1));
            }
        }

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                slotList.Add(new Sloot(i, j, 2));
            }
        }

        for (float i = 1.5F; i <= 2.5F; i++)
        {
            for (float j = 1.5F; j <= 2.5F; j++)
            {
                slotList.Add(new Sloot(i, j, 3));
            }
        }

        slotList.Add(new Sloot(2, 2, 4));

        for(int i = 0; i <= 4; i++)
        {
            for(int j = 0; j <= 4; j++)
            {
                slotList.Add(new Sloot(i, 5, j));
            }
        }
    }
}