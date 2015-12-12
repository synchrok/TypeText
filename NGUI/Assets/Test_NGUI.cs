using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test_NGUI : MonoBehaviour {

    public UILabel label;
    private Queue<string> scripts = new Queue<string>();

    public void Start() {
        scripts.Enqueue("Hello! My name is[speed=0.2]... [speed=0.4]NPC");
        scripts.Enqueue("You can [b]use[/b] [i]NGUI[/i] [u]text[/u] [s]tag[/s] and [FF0000]color[-] tag [00FF00]like this[-].");
        scripts.Enqueue("Other tags: [sub]sub[/sub] and [sup]sup[/sup]");
        ShowScript();
    }

    private void ShowScript() {
        if (scripts.Count <= 0)
            return;
        label.TypeText(scripts.Dequeue());
    }

    public void OnClickWindow() {
        if (label.IsSkippable()) {
            label.SkipTypeText();
        } else {
            ShowScript();
        }
    }

}
