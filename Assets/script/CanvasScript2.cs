using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;    // StringReader
using System.Collections.Generic; // Dictionary class

public class CanvasScript2 : MonoBehaviour
{
    public GameObject prefab;
    public Dictionary<string, Prefecture> prefTable = new Dictionary<string, Prefecture>();
    public GeneralImage puzzle;
    int tick = 0;
    // アニメーション1回だけ出して消える画像クラス、未完成
    // public InfoEffect infoTest; 

    public static class DefineGeneralImage
    {
        public const float INIT_ALPHA = 1f;
        public const string PATH_ANIMATION = "Animation/";
    }

    public static class DefineCanvasScript
    {
        // Resources folder
        public const string PATH_CSV_FILE = "CSV/prefectureData";
        public const string PATH_PREFECTURE_IMAGE = "Texture/Prefecture/Image/";
        public const string PATH_PREFECTURE_TEXT = "Texture/Prefecture/Text_HGRGY/";
        // icon configuration
        public const float INIT_ICON_POSITION_X = 30f;
        public const float INIT_ICON_POSITION_Y = 30f;
        public const float INIT_ICON_SCALE_X = 4f;
        public const float INIT_ICON_SCALE_Y = 4f;
        // text configuration
        public const float INIT_TEXT_OFFSET_X = 0f;
        public const float INIT_TEXT_OFFSET_Y = -5f;
        public const float INIT_TEXT_SCALE_X = 7f;
        public const float INIT_TEXT_SCALE_Y = 7f;
        // puzzle configuration
        public const float INIT_PUZZLE_POSITION_X = 0f;
        public const float INIT_PUZZLE_POSITION_Y = -20f;
        public const float INIT_PUZZLE_SCALE_X = 45f;
        public const float INIT_PUZZLE_SCALE_Y = 45f;
        public const string INIT_PUZZLE_PATH = "Texture/japan5";
        public const string INIT_PUZZLE_NAME = "japan";
    }

    public class ResourceStore<T> where T : class
    {
        Dictionary<string, T> resourceTable = new Dictionary<string, T>();

        public T get(string path)
        {
            //if (resourceTable[path] == null) {
            T value;
            if (!resourceTable.TryGetValue(path, out value))
            {
                //T tmp = Resources.Load(path, typeof(T)) as T;
                //resourceTable.Add(path, tmp);
                resourceTable.Add(path, Resources.Load(path, typeof(T)) as T);
                //Debug.Log(resourceTable[path]);
                return resourceTable[path];
            }
            else
            {
                return value;
            }
        }
    }

    public class GeneralImage
    {
        // components
        public GameObject objP;
        public GameObject obj;
        public SpriteRenderer spriterenderer;
        public Transform transform;
        public Animation animation;
        // cache
        public ResourceStore<Sprite> spriteStore = new ResourceStore<Sprite>();
        public ResourceStore<Object> clipStore = new ResourceStore<Object>();
        // parameter
        public string name
        {
            get
            {
                return obj.name;
            }
            set
            {
                obj.name = value;
            }
        }
        public Vector2 pos
        {
            get
            {
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }
        }
        public Vector2 scale
        {
            get
            {
                return transform.localScale;
            }
            set
            {
                transform.localScale = value;
            }
        }
        string pathBack;
        public string path
        {
            get
            {
                return pathBack;
            }
            set
            {
                pathBack = value;
                //spriterenderer.sprite = Resources.Load(value, typeof(Sprite)) as Sprite;

                spriterenderer.sprite = spriteStore.get(value);
                //Debug.Log(spriterenderer.sprite);
            }
        }
        public float alpha
        {
            get
            {
                return spriterenderer.color.a;
            }
            set
            {
                Color tmp = spriterenderer.color;
                tmp.a = value;
                if (tmp.a >= 1)
                {
                    tmp.a = 1;
                    deltaAlpha = 0;
                }
                else if (tmp.a <= 0)
                {
                    tmp.a = 0;
                    deltaAlpha = 0;
                }
                spriterenderer.color = tmp;
            }
        }
        public float deltaAlpha;
        public int order
        {
            get
            {
                return spriterenderer.sortingOrder;
            }
            set
            {
                spriterenderer.sortingOrder = value;
            }
        }
        // constructor
        public GeneralImage(GameObject prefab,
                        Transform parent,
                        string path,
                        string name,
                        Vector2 pos,
                        Vector2 scale,
                        int order)
        {
            // setting parent ( prefab <-> objP <-> obj )
            objP = new GameObject(name + "P");
            obj = (GameObject)Instantiate(prefab);
            Transform transObj = obj.GetComponent<Transform>();
            transObj.SetParent(objP.transform, false);

            transform = objP.GetComponent<Transform>();
            transform.SetParent(parent, false);
            spriterenderer = obj.GetComponent<SpriteRenderer>();
            animation = obj.GetComponent<Animation>();

            this.path = path;
            this.name = name;
            this.pos = pos;
            this.scale = scale;

            transObj.SetParent(transform, false);
            transObj.localPosition = Vector3.zero;
            transObj.localRotation = Quaternion.Euler(Vector3.zero);
            transObj.localScale = Vector3.one;

            alpha = DefineGeneralImage.INIT_ALPHA;
            deltaAlpha = 0f;
            this.order = order;
        }

        public void playAnimation(string pathAnimationClip)
        {
            //AnimationClip clip = (AnimationClip)AnimationClip.Instantiate(Resources.Load(DefineGeneralImage.PATH_ANIMATION));
            string path = DefineGeneralImage.PATH_ANIMATION + "Yspin";
            Object tmp = clipStore.get(path);
            //Debug.Log(tmp);
            AnimationClip clip = (AnimationClip)AnimationClip.Instantiate(tmp);
            animation.AddClip(clip, path);
            animation.Play(path);
        }
    }

    public class Prefecture
    {
        public GeneralImage image;
        public GeneralImage text;
        //public GeneralImage imageback;
        Vector2 textOffset = new Vector2(DefineCanvasScript.INIT_TEXT_OFFSET_X,
                                         DefineCanvasScript.INIT_TEXT_OFFSET_Y);

        public Prefecture(GameObject prefab,
                          Transform parent,
                          string imagePath,
                          string textPath,
                          string name,
                          Vector2 pos,
                          Vector2 imageScale,
                          Vector2 textScale)
        {
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
                                    textOffset,
                                    textScale,
                                    100);
            /*
            imageback = new GeneralImage(prefab,
                                         parent,
                                         "Texture/tofu",
                                         name + "tofu",
                                         pos,
                                         new Vector2(30f, 30f),
                                         10);
                                         */
        }
    }
    
    /* 設計中
    public class InfoEffect : GeneralImage
    {
        
        public InfoEffect(GameObject prefab,
                        Transform parent,
                        string path,
                        string name,
                        Vector2 pos,
                        Vector2 scale,
                        int order) : base(
                        prefab,
                        parent,
                        path,
                        name,
                        pos,
                        scale,
                        order) {
            
        }
    }
    */

    // Use this for initialization
    void Start()
    {
        puzzle = new GeneralImage(
                            prefab,
                            this.transform,
                            DefineCanvasScript.INIT_PUZZLE_PATH,
                            DefineCanvasScript.INIT_PUZZLE_NAME,
                            new Vector2(DefineCanvasScript.INIT_PUZZLE_POSITION_X,
                                        DefineCanvasScript.INIT_PUZZLE_POSITION_Y),
                            new Vector2(DefineCanvasScript.INIT_PUZZLE_SCALE_X,
                                        DefineCanvasScript.INIT_PUZZLE_SCALE_Y),
                            0);
        puzzle.alpha = 1f;
        initPrefecture();
        
    }

    // Update is called once per frame
    void Update()
    {
        tick++;
        puzzle.alpha += puzzle.deltaAlpha;

        updateTest();

    }

    void initPrefecture()
    {
        TextAsset csv = Resources.Load(DefineCanvasScript.PATH_CSV_FILE) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        Vector2 imageSize = new Vector2(DefineCanvasScript.INIT_ICON_SCALE_X,
                                        DefineCanvasScript.INIT_ICON_SCALE_Y);
        Vector2 textSize = new Vector2(DefineCanvasScript.INIT_TEXT_SCALE_X,
                                       DefineCanvasScript.INIT_TEXT_SCALE_Y);

        reader.ReadLine();
        for (int i = 0; i < 47; i++)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            Prefecture tmp = new Prefecture(
                prefab,
                this.transform,
                DefineCanvasScript.PATH_PREFECTURE_IMAGE + values[0] + "1",
                DefineCanvasScript.PATH_PREFECTURE_TEXT + values[0],
                values[0],
                new Vector2(int.Parse(values[1]), int.Parse(values[2])),
                imageSize,
                textSize
                );
            tmp.text.objP.transform.SetParent(tmp.image.objP.GetComponent<Transform>(), false);
            prefTable.Add(values[0], tmp);
        }

        initTest();
    }

    // テスト用メソッド
    void initTest() {
        // アニメーションのテスト
        /*
        foreach (KeyValuePair<string, Prefecture> pref in prefTable)
        {
            pref.Value.image.playAnimation("Animation/Yspin");
            pref.Value.text.alpha = 0f;
        }
        */
        /* 設計中クラスのテスト
        infoTest = new InfoEffect(
            prefab,
            this.transform,
            "Texture/Prefecture/Image/kagoshima2",
            "effectTest",
            Vector2.zero,
            new Vector2(128, 128),
            300
            );
        */
    }

    // 毎フレーム呼び出されるテスト用メソッド
    void updateTest()
    {
        //prefTable["iwate"].image.pos += Vector2.right;
        

        if (tick == 100)
        {
            //puzzle.path = "Texture/Prefecture/Image/saitama1";
            //puzzle.name = "hoge";
        }
        if (tick % 200 == 0)
        {
            /*
                foreach (KeyValuePair<string, Prefecture> pref in prefTable)
                {
                    pref.Value.image.playAnimation("Animation/Yspin");
                }
            */
            //prefTable["iwate"].image.path = "Texture/Prefecture/Image/kagoshima2";
            //puzzle.deltaAlpha = 0.02f;
        }
        else if (tick % 200 == 100)
        {
            //prefTable["iwate"].image.path = "Texture/Prefecture/Image/okinawa2";
            //puzzle.deltaAlpha = -0.02f;
        }

    }
}