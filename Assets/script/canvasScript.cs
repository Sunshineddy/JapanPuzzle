using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class canvasScript : MonoBehaviour {

    public GameObject CanvasImage;  // attach "Assets/prefab/CanvasImage"
    
    Vector2 a1 = new Vector2(841f, 595f);
    public prefecture[] imagePrefecture = new prefecture[47];
       
    GameObject puzzle;

    public class prefecture
    {
        public Vector2 pos;
        public string name;
        public GameObject image;
        public GameObject text;
    }

    // Use this for initialization
    void Start () {        
        initPrefecture();
    }

    // Update is called once per frame
    void Update () {
	    
	}

    void initPrefecture()
    {
        puzzle = addImage("japan", "Texture/japan4", new Vector2(-40, 0), a1);
        TextAsset csv = Resources.Load("CSV/prefectureData") as TextAsset;
        StringReader reader = new StringReader(csv.text);
        Vector2 textSize = new Vector2(35f, 15f);
        Vector2 imageSize = new Vector2(26f, 26f);
        
        reader.ReadLine();
        for(int i=0; i<47; i++) {

            string line = reader.ReadLine();
            string[] values = line.Split(',');

            imagePrefecture[i] = new prefecture();
            imagePrefecture[i].name = values[0];
            imagePrefecture[i].pos.x = int.Parse(values[1]);
            imagePrefecture[i].pos.y = int.Parse(values[2]);
            
                imagePrefecture[i].image = addImage(
                    imagePrefecture[i].name,
                    "Texture/Prefecture/Image/" + imagePrefecture[i].name + "1",
                    imagePrefecture[i].pos,
                    imageSize);
                imagePrefecture[i].text = addImage(
                    imagePrefecture[i].name + "Text",
                    "Texture/Prefecture/text/" + imagePrefecture[i].name,
                    imagePrefecture[i].pos + new Vector2(0f, -20f),
                    textSize);
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

    GameObject addtext(string objectName, string path, Vector2 pos, Vector2 size)
    {
        GameObject t = (GameObject)Instantiate(CanvasImage);
        Transform picTransform = t.GetComponent<Transform>();
        Image picImage = t.GetComponent<Image>();
        Sprite spriteImage = Resources.Load(path, typeof(Sprite)) as Sprite;

        picImage.sprite = spriteImage;
        picTransform.SetParent(this.transform, false);

        t.name = objectName;
        picTransform.localPosition = new Vector3(pos.x, pos.y, 0);
        picTransform.localScale = new Vector3(size.x, size.y, 0);

        return t;
    }
}