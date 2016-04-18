using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Test_UGUI : MonoBehaviour {

    public Text text;
    private Queue<string> scripts = new Queue<string>();

    public void Start() {
        scripts.Enqueue("Hello! My name is[speed=0.2]... [speed=0.4]NPC");
        scripts.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
        scripts.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
        scripts.Enqueue("You can <size=40>size 40</size> and <size=20>size 20</size>");
        scripts.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
        ShowScript();
    }

    private void ShowScript() {
        if (scripts.Count <= 0) {
            return;
        }

        text.TypeText(scripts.Dequeue(), onComplete: () => Debug.Log("TypeText Complete"));
    }

    public void OnClickWindow() {
        if (text.IsSkippable()) {
            text.SkipTypeText();
        } else {
            ShowScript();
        }
    }

}
