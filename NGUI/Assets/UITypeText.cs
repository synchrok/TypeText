using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class UITypeText : MonoBehaviour {

    [SerializeField]
    private float _defaultSpeed = 0.05f;

    private UILabel label;
    private string _finalText;
    private Coroutine _typeTextCoroutine;
    private static readonly string[] _nguiSymbols = { "b", "u", "i", "s", "sup", "sub", "-" };

    private void Init() {
        if (label == null)
            label = GetComponent<UILabel>();
    }

    public void Awake() {
        Init();
    }

    public void SetText(string text, float speed = -1) {
        Init();

        _defaultSpeed = speed > 0 ? speed : _defaultSpeed;
        _finalText = ReplaceSpeed(text);
        label.text = "";

        if (_typeTextCoroutine != null) {
            StopCoroutine(_typeTextCoroutine);
        }

        _typeTextCoroutine = StartCoroutine(TypeText(text));
    }

    public void SkipTypeText() {
        StopCoroutine(_typeTextCoroutine);
        _typeTextCoroutine = null;
        label.text = _finalText;
    }

    public IEnumerator TypeText(string text) {
        var len = text.Length;
        var speed = _defaultSpeed;
        for (var i = 0; i < len; i++) {
            if (text[i] == '[' && i + 6 < len && text.Substring(i, 7).Equals("[speed=")) {
                var parseSpeed = "";
                for (var j = i + 7; j < len; j++) {
                    if (text[j] == ']')
                        break;
                    parseSpeed += text[j];
                }

                if (!float.TryParse(parseSpeed, out speed))
                    speed = 0.05f;

                i += 8 + parseSpeed.Length - 1;
                continue;
            }

            // ngui color tag
            if (text[i] == '[' && i + 7 < len && text[i + 7] == ']') {
                label.text += text.Substring(i, 8);
                i += 8 - 1;
                continue;
            }

            var symbolDetected = false;
            for (var j = 0; j < _nguiSymbols.Length; j++) {
                var symbol = string.Format("[{0}]", _nguiSymbols[j]);
                if (text[i] == '[' && i + (1 + _nguiSymbols[j].Length) < len && text.Substring(i, 2 + _nguiSymbols[j].Length).Equals(symbol)) {
                    label.text += symbol;
                    i += (2 + _nguiSymbols[j].Length) - 1;
                    symbolDetected = true;
                    break;
                }

                // exit symbol
                symbol = string.Format("[/{0}]", _nguiSymbols[j]);
                if (text[i] == '[' && i + (2 + _nguiSymbols[j].Length) < len && text.Substring(i, 3 + _nguiSymbols[j].Length).Equals(symbol)) {
                    label.text += symbol;
                    i += (3 + _nguiSymbols[j].Length) - 1;
                    symbolDetected = true;
                    break;
                }
            }

            if (symbolDetected) continue;

            label.text += text[i];
            yield return new WaitForSeconds(speed);
        }

        _typeTextCoroutine = null;
    }

    private string ReplaceSpeed(string text) {
        var result = "";
        var len = text.Length;
        for (var i = 0; i < len; i++) {
            if (text[i] == '[' && i + 6 < len && text.Substring(i, 7).Equals("[speed=")) {
                var speedLength = 0;
                for (var j = i + 7; j < len; j++) {
                    if (text[j] == ']')
                        break;
                    speedLength++;
                }

                i += 8 + speedLength - 1;
                continue;
            }

            result += text[i];
        }

        return result;
    }

    public bool IsSkippable() {
        return _typeTextCoroutine != null;
    }

}

public static class UITypeTextUtility {

    public static void TypeText(this UILabel label, string text, float speed = 0.05f) {
        var typeText = label.GetComponent<UITypeText>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<UITypeText>();
        }

        typeText.SetText(text, speed);
    }

    public static bool IsSkippable(this UILabel label) {
        var typeText = label.GetComponent<UITypeText>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<UITypeText>();
        }

        return typeText.IsSkippable();
    }

    public static void SkipTypeText(this UILabel label) {
        var typeText = label.GetComponent<UITypeText>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<UITypeText>();
        }

        typeText.SkipTypeText();
    }

}