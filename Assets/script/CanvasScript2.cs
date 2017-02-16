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
        public const float INIT_ICON_SCALE_X = 5f;
        public const float INIT_ICON_SCALE_Y = 5f;
        // text configuration
        public const float INIT_TEXT_OFFSET_X = 0f;
        public const float INIT_TEXT_OFFSET_Y = -5f;
        public const float INIT_TEXT_SCALE_X = 7f;
        public const float INIT_TEXT_SCALE_Y = 7f;
        // puzzle configuration
        public const float INIT_PUZZLE_POSITION_X = 0f;
        public const float INIT_PUZZLE_POSITION_Y = -20f;
        public const float INIT_PUZZLE_SCALE_X = 35f;
        public const float INIT_PUZZLE_SCALE_Y = 35f;
        public const string INIT_PUZZLE_PATH = "Texture/japan5";
        public const string INIT_PUZZLE_NAME = "japan";
    }

    /*
    public static class DefineRipple {
        public const int POP_TIMES = 3;
        public const int POP_TERM = 50;
        public const float INIT_DELTA_ALPHA = -0.005f;
        public const float INIT_DELTA_SCALE_X = 0.17f;
        public const float INIT_DELTA_SCALE_Y = 0.17f;
        public const int INIT_ORDER = 500;
    }
    */

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
        public virtual string path
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
            string path = DefineGeneralImage.PATH_ANIMATION + "Yspin";
            Object tmp = clipStore.get(path);
            AnimationClip clip = (AnimationClip)AnimationClip.Instantiate(tmp);
            animation.AddClip(clip, path);
            animation.Play(path);
        }

        public void spriteResize(float size) {
            float x = spriterenderer.bounds.size.x;
            float y = spriterenderer.bounds.size.y;
            float t;
            t = x > y ? size / x : size / y;
            scale = Vector2.one * t;
        }
    }

    public class Prefecture
    {
        public GeneralImage image;
        public GeneralImage text;
        public GeneralImage edge;
        //public GeneralImage imageback;
        Vector2 textOffset = new Vector2(DefineCanvasScript.INIT_TEXT_OFFSET_X,
                                         DefineCanvasScript.INIT_TEXT_OFFSET_Y);
        
        public Prefecture(GameObject prefab,
                          Transform parent,
                          string imagePath,
                          string textPath,
                          string edgePath,
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
            switch (name) {
                case "gunma":
                case "saitama":
                case "okayama":
                case "okinawa":
                    edge = new GeneralImage(prefab,
                                            parent,
                                            "Texture/Prefecture/edge/kagawa",
                                            "dummyEdge",
                                            new Vector2(0.5f, -22.7f),
                                            35 * Vector2.one,
                                            100);
                    break;
                default:
                    edge = new GeneralImage(prefab,
                                            parent,
                                            edgePath,
                                            name + "Edge",
                                            new Vector2(0.5f, -22.7f),
                                            35 * Vector2.one,
                                            100);
                    break;
            }


            Bounds b = edge.spriterenderer.bounds;
            Vector2 c = new Vector2(b.center.x, b.center.y);
            Vector2 s = b.size;
            Vector2 p = new Vector2(-c.x / s.x + 0.5f, -c.y / s.y + 0.5f);
            edge.pos = new Vector2((p.x - 0.5f) * 1192 * 594 / 1701 + 0.9f,
                                                      (p.y - 0.5f) * 594 -45.5f);
            if (name == "hokkaido") {
                edge.pos += new Vector2(-3.6f, 0.05f);
            }
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

    public List<Effect> effectList = new List<Effect>();
    public class Effect : GeneralImage
    {
        int count = 0;
        int currentImage = 1;
        int interval;
        bool endFlag = false;
        public override string path {
            get;
            set;
        }

        public Effect(GameObject prefab,
                        Transform parent,
                        string path,
                        string name,
                        Vector2 pos,
                        Vector2 scale,
                        int order,
                        int interval) : base(
                        prefab,
                        parent,
                        path,
                        name,
                        pos,
                        scale,
                        order) {
            this.interval = interval;
            
            //Debug.Log(path + "1");
            spriterenderer.sprite = spriteStore.get(path + "1");
            //int i = 1;
            //while (System.IO.File.Exists(path+i.ToString())) i++;
            
        }

        public void updateEffect()
        {
            count++;
            if (count >= interval){
                count = 0;
                currentImage++;
                Sprite tmp = spriteStore.get(path + currentImage.ToString());
                if (tmp == null)
                {
                    Destroy(obj);
                    obj = null;
                    Destroy(objP);
                    objP = null;
                    endFlag = true;
                    return;
                }
                spriterenderer.sprite = spriteStore.get(path + currentImage.ToString());
            }
        }

        public bool getEndFlag() {
            return endFlag;
        }
    }
    
    public List<Ripple> rippleList = new List<Ripple>();
    public class Ripple {
        List<GeneralImage> imgList = new List<GeneralImage>();
        int count = 0;
        int id = 1;
        bool endFlag = false;
        int popTime;
        int popTerm;
        Vector2 deltaScale;
        float deltaAlpha;

        GameObject prefab;
        Transform parent;
        string imagePath;
        string name;
        Vector2 pos;
        Vector2 scale;
        int order;
        
        public Ripple(GameObject prefab,
                      Transform parent,
                      string imagePath,
                      string name,
                      Vector2 pos,
                      Vector2 scale,
                      int order,
                      int popTime,
                      int popTerm,
                      float deltaAlpha,
                      Vector2 deltaScale)
        { 
                      //int popTerm) {
            this.prefab = prefab;
            this.parent = parent;
            this.imagePath = imagePath;
            this.name = "ripple_" + name;
            this.pos = pos;
            this.scale = scale;
            this.order = order;
            this.popTime = popTime;
            this.popTerm = popTerm;
            this.deltaScale = deltaScale;
            this.deltaAlpha = deltaAlpha;
        }

        public void addRippleList() {
            count++;
            GeneralImage template = new GeneralImage(prefab,
                                    parent,
                                    imagePath,
                                    name + id.ToString(),
                                    pos,
                                    scale,
                                    order + id);
            template.alpha = 1f;
            imgList.Add(template);
            id++;
        }

        public void updateRipple() {
            //if (count % popTerm == 0 && id <= DefineRipple.POP_TIMES) { 
            //if (count % DefineRipple.POP_TERM == 0 && id <= DefineRipple.POP_TIMES){
            if (count % popTerm == 0 && id <= popTime)
            {
               addRippleList();
            }
            for (int i = 0; i < imgList.Count; i++) {
                imgList[i].scale += deltaScale;
                imgList[i].alpha += deltaAlpha;
                if ( imgList[i].alpha <= 0f ) {
                    Destroy(imgList[i].obj);
                    imgList[i].obj = null;
                    Destroy(imgList[i].objP);
                    imgList[i].objP = null;
                    imgList.Remove(imgList[i]);
                    i--;
                    if (imgList.Count == 0) {
                        endFlag = true;
                    }
                }
            }
            count++;
        }

        public bool getEndFlag() {
            return endFlag;
        }
    }

    // Use this for initialization
    void Start()
    {
        // init puzzle
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
        
        //init prefecture
        initPrefecture();
    }

    // Update is called once per frame
    void Update()
    {
        tick++;

        // alpha update
        foreach (KeyValuePair<string, Prefecture> pref in prefTable) {
            pref.Value.image.alpha += pref.Value.image.deltaAlpha;
            pref.Value.text.alpha += pref.Value.text.deltaAlpha;
        }
        puzzle.alpha += puzzle.deltaAlpha;

        // ripple update
        for (int i = 0; i < rippleList.Count; i++) {
            rippleList[i].updateRipple();
            if (rippleList[i].getEndFlag())
            {
                rippleList.Remove(rippleList[i]);
            }
        }

        // effect update
        for (int i = 0; i < effectList.Count; i++) {
            effectList[i].updateEffect();
            if (effectList[i].getEndFlag()) {
                effectList.Remove(effectList[i]);
            }
        }

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
                "Texture/Prefecture/edge/" + values[0],
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
        /*
        foreach (KeyValuePair<string, Prefecture> pref in prefTable)
        {
            
        }
        */
    }

    // 毎フレーム呼び出されるテスト用メソッド
    void updateTest()
    {
        if (tick == 30)
        {
            string[] sample = new string[] {"hokkaido", "iwate", "tochigi", "kagawa", "kagoshima" };
            for (int i = 0; i < 5; i++) {
                rippleAdd(prefab,
                          this.transform,
                          "Texture/Prefecture/edge/" + prefTable[sample[i]].image.name,
                          prefTable[sample[i]].edge.name,
                          prefTable[sample[i]].edge.pos,
                          prefTable[sample[i]].edge.scale,
                          600,
                          4,
                          40,
                          -0.01f,
                          0.9f * Vector2.one);
            }
        }
        /*
        if (tick % 400 == 0)
        {
        }
        else if (tick % 400 == 200)
        {
        }
        */
    }

    public void rippleAdd(GameObject prefab,
                          Transform parent,
                          string imagePath,
                          string name,
                          Vector2 pos,
                          Vector2 scale,
                          int order,
                          int popTime,
                          int popTerm,
                          float deltaAlpha,
                          Vector2 deltaScale)
    {
        Ripple tmp = new Ripple(prefab,
                    this.transform,
                    imagePath,
                    name,
                    pos,
                    scale,
                    order,
                    popTime,
                    popTerm,
                    deltaAlpha,
                    deltaScale);
        rippleList.Add(tmp);
    }

    public void effectAdd(GameObject prefab,
                          Transform parent,
                          string imagePath,
                          string name,
                          Vector2 pos,
                          Vector2 scale,
                          int order,
                          int interval)
    {
        Effect tmp = new Effect(prefab,
                                this.transform,
                                imagePath,
                                name,
                                pos,
                                scale,
                                order,
                                4);
        effectList.Add(tmp);
    }
}