using UnityEngine;
using System.Collections;
using UnityEngine.UI;       // 追加

public class dbg : MonoBehaviour {

    public GameController gc;
    string dbgText;
    
	// Use this for initialization
	void Start ()
    {
        dbgText = this.GetComponent<Text>().text;
    }
	
	// Update is called once per frame
	void Update () {

        dbgText = "tick : " + gc.get_tick().ToString() + "\n";
        dbgText += "texPath : " + gc.get_texPath();

        this.GetComponent<Text>().text = dbgText;
	}
}
