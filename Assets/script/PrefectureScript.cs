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
    public const string PATH_ANIMATION = "Animation/";
}

public static class DefinePrefectureScript
{
    public const string PATH_CSV_FILE = "CSV/prefectureData";
    public const string PATH_PREFECTURE_IMAGE = "Texture/Prefecture/Image/";
    public const string PATH_PREFECTURE_TEXT = "Texture/Prefecture/Text/";
}


public class PrefectureScript : MonoBehaviour {

    public GameObject prefab;
    public Dictionary<string, Prefecture> prefTable = new Dictionary<string, Prefecture>();

    public GeneralImage puzzle;
    int tick = 0;
    
    /* Resource */
    public class ResourceStore <T> where T : class{
        Dictionary<string, T> resourceTable = new Dictionary<string, T>();

        public T get(string path) {
            //if (resourceTable[path] == null) {
            T value;
            if (!resourceTable.TryGetValue(path, out value)) {
                resourceTable.Add(path, Resources.Load(path) as T);
                //Debug.Log("hoge");
            }
            //Debug.Log(resourceTable.Keys);
            resourceTable.TryGetValue(path, out value);
            //Debug.Log(value);
            //return value;
            return resourceTable[path];
            //return null;
        }
    }
    
    public class GeneralImage {
        // components
        public GameObject objP;
        public GameObject obj;
        public SpriteRenderer spriterenderer;
        public Transform transform;
        public Animation animation;
        // cache
        //public ResourceStore<Sprite> spriteStore = new ResourceStore<Sprite>();
        //public ResourceStore<Object> clipStore = new ResourceStore<Object>();

        // parameter
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
                //spriterenderer.sprite = spriteStore.get(value);
                //Debug.Log(spriterenderer.sprite);
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
            objP = new GameObject(name+"P");
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

        public void PlayAnimation(string pathAnimationClip) {
            AnimationClip clip = (AnimationClip)AnimationClip.Instantiate(Resources.Load(DefineGeneralImage.PATH_ANIMATION));
            //Object tmp = clipStore.get(DefineGeneralImage.PATH_ANIMATION);
            //AnimationClip clip = (AnimationClip)AnimationClip.Instantiate(tmp);
            animation.AddClip(clip, DefineGeneralImage.PATH_ANIMATION);
            animation.Play(DefineGeneralImage.PATH_ANIMATION);
        }
    }

    public class Prefecture {
        public GeneralImage image;
        public GeneralImage text;
        public GeneralImage imageback;

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
            imageback = new GeneralImage(prefab,
                                         parent,
                                         "Texture/tofu",
                                         name+"tofu",
                                         pos,
                                         new Vector2(30f,30f),
                                         10);
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
        puzzle.alpha = 0f;
        puzzle.deltaAlpha = 0.002f;
        puzzle.order = 0;
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
        TextAsset csv = Resources.Load(DefinePrefectureScript.PATH_CSV_FILE) as TextAsset;
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
                DefinePrefectureScript.PATH_PREFECTURE_IMAGE + values[0] + "1",
                DefinePrefectureScript.PATH_PREFECTURE_TEXT + values[0],
                values[0],
                new Vector2(int.Parse(values[1]), int.Parse(values[2])),
                imageSize,
                textSize
                );
            tmp.imageback.alpha = 0.5f;
            prefTable.Add(values[0], tmp);
        }
    }
    
    void test() {
        //prefTable["iwate"].image.pos += Vector2.right;

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