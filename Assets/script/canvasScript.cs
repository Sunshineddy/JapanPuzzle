using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic; // Dictionary class

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
        Vector2 posBack;
        string imagePathBack;
        Vector2 textOffsetBack;
        public GameObject imageObject;
        public GameObject textObject;
        public Transform imageTransform;
        public Transform textTransform;
        public Image imageImage;
        public Image textImage;

        public Vector2 pos {
            get {
                return posBack;
            }
            set {
                posBack = value;
                imageTransform.localPosition = new Vector3(value.x, value.y, 0);
                textTransform.localPosition = new Vector3(value.x+textOffset.x, value.y+textOffset.y, 0);
            }
        }
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
        public string name;
        public float deltaImageAlpha;
        public float imageAlpha {
            get {
                return imageObject.GetComponent<Image>().color.a;
            }
            set {
                Color tmp = imageObject.GetComponent<Image>().color;
                tmp.a = value;
                if (tmp.a > 1)
                {
                    tmp.a = 1;
                    deltaImageAlpha = 0;
                }
                else if (tmp.a < 0)
                {
                    tmp.a = 0;
                    deltaImageAlpha = 0;
                }
                imageObject.GetComponent<Image>().color = tmp;
            }
        }
        public float deltaTextAlpha;
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
        public string imagePath {
            get {
                return imagePathBack;
            }
            set {
                imagePathBack = value;
                imageObject.GetComponent<Image>().sprite = Resources.Load(value, typeof(Sprite)) as Sprite;
            }
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
            }
            */
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

            prefecture tmp = new prefecture();
            tmp.name = values[0];
            tmp.imageObject = addImage(
                 tmp.name,
                 "Texture/Prefecture/Image/" + tmp.name + "1",
                 imageSize);
            tmp.textObject = addImage(
                tmp.name + "Text",
                "Texture/Prefecture/text/" + tmp.name,
                textSize);
            tmp.imageImage = tmp.imageObject.GetComponent<Image>();
            tmp.textImage = tmp.textObject.GetComponent<Image>();
            tmp.imageTransform = tmp.imageObject.GetComponent<Transform>();
            tmp.textTransform = tmp.textObject.GetComponent<Transform>();
            tmp.pos = new Vector2(int.Parse(values[1]), int.Parse(values[2]));
            tmp.textOffset = new Vector2(0f, -20f);
            tmp.imageAlpha = 1f;

            prefTable.Add(values[0], tmp);
        }
    }

    
    /* for initPrefecture */
    GameObject addImage(string objectName, string path, Vector2 size)
    {
        GameObject pic = (GameObject)Instantiate(CanvasImage);
        Transform picTransform = pic.GetComponent<Transform>();
        Image picImage = pic.GetComponent<Image>();
        Sprite spriteImage = Resources.Load(path, typeof(Sprite)) as Sprite;

        picImage.sprite = spriteImage;
        picTransform.SetParent(this.transform, false);

        pic.name = objectName;
        picTransform.localScale = new Vector3(size.x, size.y, 0);

        return pic;
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