using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkGenerator : MonoBehaviour
{
    public Vector2 chunk_offset;
    public TextAsset[] chunks;
    public Texture2D[] textures;
    public GameObject[] mobs;

    private GameObject container;

    // Start is called before the first frame update
    void Start()
    {
        // create container
        container = new GameObject("chunk");
        container.transform.parent = transform.parent;

        // read file
        string[] data = chunks[0].text.Split("\n");

        // remove comments
        int not_blank = 0;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Split("##")[0];
            if (data[i] != "") not_blank++;
        }

        // create chunk
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].Length > 2)
            {
                string[] words = data[i].Split(" ");
                switch (words[0])
                {
                    case "defineTile":
                        switch (words[1].Split(":")[0])
                        {
                            case "tile":
                                string[] position_string = words[4].Substring(1).Split(")")[0].Split(",");
                                Vector2 position = new Vector2(int.Parse(position_string[0]), -int.Parse(position_string[1]));
                                (new cnk_tile(words[3], words[2].Substring(1, words[2].Length - 2), position, textures, container)).add_collider();
                                break;
                            case "tileArray":
                                string[] start_string = words[4].Substring(1).Split(")")[0].Split(",");
                                Vector2 start = new Vector2(int.Parse(start_string[0]), int.Parse(start_string[1]));
                                string[] end_string = words[5].Substring(1).Split(")")[0].Split(",");
                                Vector2 end = new Vector2(int.Parse(end_string[0]), int.Parse(end_string[1]));
                                new cnk_tileArray(words[3], words[2].Substring(1, words[2].Length - 2), start, end, textures, container);
                                break;
                            case "tileLine":
                                string[] line_position_string = words[4].Substring(1).Split(")")[0].Split(",");
                                Vector2 line_position = new Vector2(int.Parse(line_position_string[0]), -int.Parse(line_position_string[1]));
                                float line_angle = float.Parse(words[1].Split(":")[1]);
                                new cnk_tileLine(words[3], words[2].Substring(1, words[2].Length - 2), int.Parse(words[5]), line_position, line_angle, textures, container);
                                break;
                        }
                        break;
                    case "defineMob":
                        switch (words[1])
                        {
                            case "mob":
                                string[] position_string = words[4].Substring(1).Split(")")[0].Split(",");
                                Vector2 position = new Vector2(int.Parse(position_string[0]), -int.Parse(position_string[1]));
                                new cnk_mob(words[3], words[2].Substring(1, words[2].Length - 2), position, mobs, container);
                                break;
                        }
                        break;
                }
            }
        }
        container.transform.position -= new Vector3(-chunk_offset.x, chunk_offset.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class cnk_tile
{

    private string __tile_type;
    private Vector2 __position;
    private Sprite __sprite;
    private GameObject __obj;


    public cnk_tile(string tile_type, string name, Vector2 position, Texture2D[] textures, GameObject parent)
    {
        __tile_type = tile_type;
        __position = position;
        __obj = new GameObject(name, typeof(SpriteRenderer));
        __obj.transform.parent = parent.transform;
        __obj.transform.position = new Vector2(position.x, position.y) - .5f * Vector2.one;
        string[] tile_id = tile_type.Split(":");
        int id = int.Parse(tile_id[0]);
        float rotation = float.Parse(tile_id[1]);
        __sprite = Sprite.Create(textures[id], new Rect(0, 0, 60, 60), new Vector2(.5f, .5f), 60.0f);
        SpriteRenderer ren = __obj.GetComponent<SpriteRenderer>();
        ren.sprite = __sprite;
        __obj.transform.eulerAngles += new Vector3(0, 0, 90.0f * rotation);
        return;

    }
    public void add_collider()
    {
        BoxCollider2D collider = __obj.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        collider.size = new Vector2(1, 1);
    }
}

public class cnk_tileLine
{
    private string __tile_type;
    private int __length;
    private Vector2 __position;
    private float __angle;
    private GameObject __obj;

    public cnk_tileLine(string tile_type, string name, int length, Vector2 position, float line_angle, Texture2D[] textures, GameObject parent) 
    {
        __tile_type = tile_type;
        __length = length;
        __position = position;
        __angle = line_angle;
        __obj = new GameObject(name + (__position.x * __position.y * length).ToString(), typeof(BoxCollider2D));
        __obj.transform.parent = parent.transform;

        for (int i = 0; i < length; i++) 
        {
            new cnk_tile(__tile_type, name + (100 * (i - length / 2) + __position.y).ToString(), new Vector2(i-length/2, 0), textures, __obj);
        }

        BoxCollider2D collider = __obj.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(length,1);
        collider.offset -= .5f * Vector2.one;

        __obj.transform.position = __position;
        __obj.transform.eulerAngles = new Vector3(0, 0, 90.0f * __angle);
        
    }

}

public class cnk_tileArray
{
    private string __tile_type;
    private Vector2 __area_start;
    private Vector2 __area_end;
    private GameObject __container;
    public cnk_tileArray(string tile_type, string name, Vector2 start, Vector2 end, Texture2D[] textures, GameObject parent)
    {
        __tile_type = tile_type;
        __area_start = start;
        __area_end = end;

        __container = new GameObject(name + (__area_start.x * __area_start.y + __area_end.x * __area_end.y).ToString(), typeof(BoxCollider2D));
        __container.transform.parent = parent.transform;
        BoxCollider2D collider = __container.GetComponent<BoxCollider2D>();
        collider.size = __area_end - __area_start + Vector2.one;
        collider.offset = Vector2.Reflect(Vector2.Lerp(__area_start, __area_end, .5f), Vector2.up) - .5f * Vector2.one;

        Debug.Log(__area_start);

        for (float i = __area_start.x; i <= __area_end.x; i++)
        {
            for (float j = __area_start.y; j <= __area_end.y; j++)
            {
                new cnk_tile(__tile_type, name + (100*i+j).ToString(), new Vector2(i,-j), textures, __container);
            }
        }

    }
}
public class cnk_mob
{
    private string __living_type;
    private Vector2 __start_position;
    private GameObject __obj;

    public cnk_mob(string mob_type, string name, Vector2 position, GameObject[] mobs, GameObject parent)
    {
        __living_type = mob_type;
        __start_position = position;

        string[] mob_id = __living_type.Split(":");
        int id = int.Parse(mob_id[0]);
        __obj = GameObject.Instantiate(mobs[id]);
        __obj.transform.parent = parent.transform;
        __obj.transform.position = position;
        __obj.name = name;
        return;
    }
}