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
    Vector2 a1 = new Vector2(841f, 595f);


    public class prefecture
    {
        public Vector2 pos;
        public string name;
        public GameObject image;
        public GameObject text;
        public float deltaAlpha;
        public float alpha {
            get {
                return image.GetComponent<Image>().color.a;
            }
            set {
                Color tmp = image.GetComponent<Image>().color;
                tmp.a = value;
                if (tmp.a > 1)
                {
                    tmp.a = 1;
                    deltaAlpha = 0;
                }
                else if (tmp.a < 0)
                {
                    tmp.a = 0;
                    deltaAlpha = 0;
                }
                image.GetComponent<Image>().color = tmp;
                text.GetComponent<Image>().color = tmp;
            }
        }
    }

    
    void Start () {        
        initPrefecture();
    }
    
    void Update () {
        tick++;
        updatePrefecture();
    }

    void updatePrefecture()
    {
        foreach (KeyValuePair<string, prefecture> item in prefTable) {
            item.Value.alpha += item.Value.deltaAlpha;

            if ((tick % 200) == 0)
            {
                item.Value.deltaAlpha = -0.02f;
            }
            else if ((tick % 200) == 100)
            {
                item.Value.deltaAlpha = 0.02f;
            }
        }
    }

    void initPrefecture()
    {
        //puzzle = addImage("japan", "Texture/japan4", new Vector2(-40, 0), new Vector2(841f, 595f));

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
            tmp.pos.x = int.Parse(values[1]);
            tmp.pos.y = int.Parse(values[2]);
            tmp.image = addImage(
                 tmp.name,
                 "Texture/Prefecture/Image/" + tmp.name + "1",
                 tmp.pos,
                 imageSize);
            tmp.text = addImage(
                tmp.name + "Text",
                "Texture/Prefecture/text/" + tmp.name,
                tmp.pos + new Vector2(0f, -20f),
                textSize);
            tmp.alpha = 0f;
            prefTable.Add(values[0], tmp);
        }
    }
    
    GameObject addImage(string objectName, string path, Vector2 pos, Vector2 size) {
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