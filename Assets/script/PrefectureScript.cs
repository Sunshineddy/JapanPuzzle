using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;    // StringReader
using System.Collections.Generic; // Dictionary class


public static class DefineGeneralImage
{
    public const float INIT_ALPHA = 1f;
    public const float INIT_TEXT_OFFSET_X = 0f;
    public const float INIT_TEXT_OFFSET_Y = -20f;
}


public class PrefectureScript : MonoBehaviour {

    public GameObject prefab;
    public Dictionary<string, Prefecture> prefTable = new Dictionary<string, Prefecture>();

    public GeneralImage puzzle;
    int tick = 0;


    public class GeneralImage {
        public GameObject obj;
        public SpriteRenderer spriterenderer;
        public Transform transform;

        public string name {
            get {
                return obj.name;
            }
            set {
                obj.name = value;
            }
        }
        public Vector2 pos {
            get {
                return transform.localPosition;
            }
            set {
                transform.localPosition = value;
            }
        }
        public Vector2 scale {
            get {
                return transform.localScale;
            }
            set {
                transform.localScale = value;
            }
        }
        string pathBack;
        public string path {
            get {
                return pathBack;
            }
            set {
                pathBack = value;
                spriterenderer.sprite = Resources.Load(value, typeof(Sprite)) as Sprite;
            }
        }
        public float alpha {
            get {
                return spriterenderer.color.a;
            }
            set {
                Color tmp = spriterenderer.color;
                tmp.a = value;
                if (tmp.a >= 1) {
                    tmp.a = 1;
                    deltaAlpha = 0;
                }
                else if(tmp.a <= 0){
                    tmp.a = 0;
                    deltaAlpha = 0;
                }
                spriterenderer.color = tmp;
            }
        }
        public float deltaAlpha;
        public int order {
            get {
                return spriterenderer.sortingOrder;
            }
            set {
                spriterenderer.sortingOrder = value;
            }
        }

        public GeneralImage(GameObject prefab,
                        Transform parent,
                        string path,
                        string name,
                        Vector2 pos,
                        Vector2 scale,
                        int order)
        {
            obj = (GameObject)Instantiate(prefab);
            transform = obj.GetComponent<Transform>();
            spriterenderer = obj.GetComponent<SpriteRenderer>();
            transform.SetParent(parent, false);
            this.path = path;
            this.name = name;
            this.pos = pos;
            this.scale = scale;
            alpha = DefineGeneralImage.INIT_ALPHA;
            deltaAlpha = 0f;
            this.order = order;
        }
        
    }

    public class Prefecture {
        public GeneralImage image;
        public GeneralImage text;

        public Prefecture(GameObject prefab,
                          Transform parent,
                          string imagePath,
                          string textPath,
                          string name,
                          Vector2 pos,
                          Vector2 imageScale,
                          Vector2 textScale) {
            image = new GeneralImage(prefab,
                                     parent,
                                     imagePath,
                                     name,
                                     pos,
                                     imageScale,
                                     200);
            text = new GeneralImage(prefab,
                                    parent,
                                    textPath,
                                    name + "Text",
                                    pos + new Vector2(DefineGeneralImage.INIT_TEXT_OFFSET_X, DefineGeneralImage.INIT_TEXT_OFFSET_Y),
                                    textScale,
                                    100);
        }
    }

	// Use this for initialization
	void Start () {
        puzzle = new GeneralImage(
                            prefab,
                            this.transform,
                            "Texture/japan4",
                            "japan",
                            new Vector2(-0f, 0f),
                            new Vector2(110f, 110f),
                            0);
        puzzle.spriterenderer.sortingOrder = 0;
        initPrefecture();
	}
	
	// Update is called once per frame
	void Update () {
        tick++;
        puzzle.alpha += puzzle.deltaAlpha;

        test();

    }
    
    void initPrefecture()
    {
        TextAsset csv = Resources.Load("CSV/prefectureData") as TextAsset;
        StringReader reader = new StringReader(csv.text);
        Vector2 imageSize = new Vector2(3f, 3f);
        Vector2 textSize = new Vector2(30f, 30f);

        reader.ReadLine();
        for (int i = 0; i < 47; i++)
        {

            string line = reader.ReadLine();
            string[] values = line.Split(',');

            Prefecture tmp = new Prefecture(
                prefab,
                this.transform,
                "Texture/Prefecture/Image/" + values[0] + "1",
                "Texture/Prefecture/text/" + values[0],
                values[0],
                new Vector2(int.Parse(values[1]), int.Parse(values[2])),
                imageSize,
                textSize
                );

            prefTable.Add(values[0], tmp);
        }
    }
    
    void test() {
        Vector2 u = new Vector2(1f, 1f);

        //puzzle.pos += u;
        //puzzle.scale -= u;
        if (tick == 100)
        {
            //puzzle.path = "Texture/Prefecture/Image/saitama1";
            //puzzle.name = "hoge";
        }
        if (tick % 200 == 0)
        {
            //puzzle.deltaAlpha = 0.02f;
        }
        else if (tick % 200 == 100) {
            //puzzle.deltaAlpha = -0.02f;
        }

    }
}
