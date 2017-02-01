using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic; // Dictionary class


public static class Define {
    public const float INIT_IMAGE_ALPHA = 1f;
    public const float INIT_TEXT_ALPHA = 1f;
    public const float INIT_TEXT_OFFSET_X = 0f;
    public const float INIT_TEXT_OFFSET_Y = -20f;
}


public class canvasScript : MonoBehaviour {

    /* Attach "Assets/prefab/CanvasImage" */
    public GameObject CanvasImage;
    /* Hashtable ( key : prefecture name ) */
    public Dictionary<string, prefecture> prefTable = new Dictionary<string, prefecture>();

    /* for debug variables */
    int tick = 0;
    GameObject puzzle;

    public class prefecture
    {
        
        public GameObject imageObject;
        public Transform imageTransform;
        public Image imageImage;
        Vector2 posBack;
        public Vector2 pos
        {
            get
            {
                return posBack;
            }
            set
            {
                posBack = value;
                imageTransform.localPosition = new Vector3(value.x, value.y, 0);
                textTransform.localPosition = new Vector3(value.x + textOffset.x, value.y + textOffset.y, 0);
            }
        }
        public string imageName
        {
            get
            {
                return imageObject.name;
            }
            set
            {
                imageObject.name = value;
            }
        }
        public float imageAlpha
        {
            get
            {
                return imageObject.GetComponent<Image>().color.a;
            }
            set
            {
                Color tmp = imageObject.GetComponent<Image>().color;
                tmp.a = value;
                if (tmp.a >= 1)
                {
                    tmp.a = 1;
                    deltaImageAlpha = 0;
                }
                else if (tmp.a <= 0)
                {
                    tmp.a = 0;
                    deltaImageAlpha = 0;
                }
                imageObject.GetComponent<Image>().color = tmp;
            }
        }
        public float deltaImageAlpha;
        string imagePathBack;
        public string imagePath
        {
            get
            {
                return imagePathBack;
            }
            set
            {
                imagePathBack = value;
                imageObject.GetComponent<Image>().sprite = Resources.Load(value, typeof(Sprite)) as Sprite;
            }
        }
        public Vector2 imageSize {
            get {
                return imageTransform.localScale;
            }
            set {
                imageTransform.localScale = value;
            }
        }

        public GameObject textObject;
        public Transform textTransform;
        public Image textImage;
        Vector2 textOffsetBack;
        public Vector2 textOffset
        {
            get
            {
                return textOffsetBack;
            }
            set
            {
                textOffsetBack = value;
                textTransform.localPosition = imageTransform.localPosition + new Vector3(textOffsetBack.x, textOffsetBack.y);
            }
        }
        public string textName {
            get {
                return textObject.name;
            }
            set {
                textObject.name = value;
            }
        }
        public float textAlpha
        {
            get
            {
                return textObject.GetComponent<Image>().color.a;
            }
            set
            {
                Color tmp = textObject.GetComponent<Image>().color;
                tmp.a = value;
                if (tmp.a > 1)
                {
                    tmp.a = 1;
                    deltaTextAlpha = 0;
                }
                else if (tmp.a < 0)
                {
                    tmp.a = 0;
                    deltaTextAlpha = 0;
                }
                textObject.GetComponent<Image>().color = tmp;
            }
        }
        public float deltaTextAlpha;
        string textPathBack;
        public string textPath
        {
            get
            {
                return textPathBack;
            }
            set
            {
                textPathBack = value;
                textObject.GetComponent<Image>().sprite = Resources.Load(value, typeof(Sprite)) as Sprite;
            }
        }
        public Vector2 textSize
        {
            get
            {
                return imageTransform.localScale;
            }
            set
            {
                imageTransform.localScale = value;
            }
        }

        /* constructor */
        public prefecture(GameObject prefab,
                          Transform parent, 
                          string imagePath,
                          string textPath,
                          string name,
                          Vector2 pos,
                          Vector2 imageSize,
                          Vector2 textSize) {
            initObject(prefab, parent);
            this.pos = pos;
            textOffset = new Vector2(Define.INIT_TEXT_OFFSET_X, Define.INIT_TEXT_OFFSET_Y);

            imageName = name;
            imageAlpha = Define.INIT_IMAGE_ALPHA;
            deltaImageAlpha = 0f;
            this.imagePath = imagePath;
            imageTransform.localScale = imageSize;
            

            textName = name + "Text";
            textAlpha = Define.INIT_TEXT_ALPHA;
            deltaImageAlpha = 0f;
            this.textPath = textPath;
            textTransform.localScale = textSize;
        }

        /* for initPrefecture */
        void initObject(GameObject prefab, Transform parent)
        {
            this.imageObject = (GameObject)Instantiate(prefab);
            this.imageTransform = this.imageObject.GetComponent<Transform>();
            this.imageImage = this.imageObject.GetComponent<Image>();
            this.imageTransform.SetParent(parent, false);

            this.textObject = (GameObject)Instantiate(prefab);
            this.textTransform = this.textObject.GetComponent<Transform>();
            this.textImage = this.textObject.GetComponent<Image>();
            this.textTransform.SetParent(parent, false);
        }
    }
    
    
    void Start ()
    {
        puzzle = addImage("japan", "Texture/japan4", new Vector2(841f, 595f), new Vector2(-40, 0));
        initPrefecture();
    }
    
    void Update ()
    {
        tick++;
        updatePrefecture();
    }
    
    void updatePrefecture()
    {
        foreach (KeyValuePair<string, prefecture> item in prefTable) {
            item.Value.imageAlpha += item.Value.deltaImageAlpha;
            item.Value.textAlpha += item.Value.deltaTextAlpha;
            // debug
            /*
            if ((tick % 200) == 0)
            {
                item.Value.deltaTextAlpha = -0.02f;
            }
            else if ((tick % 200) == 100)
            {
                item.Value.deltaTextAlpha = 0.02f;
            }*/
        }
    }

    void initPrefecture()
    {
        TextAsset csv = Resources.Load("CSV/prefectureData") as TextAsset;
        StringReader reader = new StringReader(csv.text);
        Vector2 textSize = new Vector2(35f, 15f);
        Vector2 imageSize = new Vector2(26f, 26f);
        
        reader.ReadLine();
        for(int i=0; i<47; i++) {

            string line = reader.ReadLine();
            string[] values = line.Split(',');

            prefecture tmp = new prefecture(
                CanvasImage,
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


    /* for general image */
    GameObject addImage(string objectName, string path, Vector2 size, Vector2 pos)
    {
        GameObject pic = (GameObject)Instantiate(CanvasImage);
        Transform picTransform = pic.GetComponent<Transform>();
        Image picImage = pic.GetComponent<Image>();
        Sprite spriteImage = Resources.Load(path, typeof(Sprite)) as Sprite;
                
        picImage.sprite = spriteImage;
        picTransform.SetParent(this.transform, false);

        pic.name = objectName;
        picTransform.localPosition = new Vector3(pos.x, pos.y, 0);
        picTransform.localScale = new Vector3(size.x, size.y, 0);
        
        return pic;
    }

}