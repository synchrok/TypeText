using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypeTextComponent : MonoBehaviour {
    public delegate void OnComplete();

    [SerializeField]
    private float _defaultSpeed = 0.05f;

    private Text label;
    private string _currentText;
    private string _finalText;
    private Coroutine _typeTextCoroutine;

    private static readonly string[] _uguiSymbols = { "b", "i" }; 
    private static readonly string[] _uguiCloseSymbols = { "b", "i", "size", "color" };
    private OnComplete _onCompleteCallback;

    private void Init() {
        if (label == null)
            label = GetComponent<Text>();
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
        if (_typeTextCoroutine != null)
            StopCoroutine(_typeTextCoroutine);
        _typeTextCoroutine = null;

        label.text = _finalText;

        if (_onCompleteCallback != null)
            _onCompleteCallback();
    }

    public IEnumerator TypeText(string text) {
        _currentText = "";

        var len = text.Length;
        var speed = _defaultSpeed;
        var tagOpened = false;
        var tagType = "";
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
                _currentText += text.Substring(i, 8);
                i += 8 - 1;
                continue;
            }

            var symbolDetected = false;
            for (var j = 0; j < _uguiSymbols.Length; j++) {
                var symbol = string.Format("<{0}>", _uguiSymbols[j]);
                if (text[i] == '<' && i + (1 + _uguiSymbols[j].Length) < len && text.Substring(i, 2 + _uguiSymbols[j].Length).Equals(symbol)) {
                    _currentText += symbol;
                    i += (2 + _uguiSymbols[j].Length) - 1;
                    symbolDetected = true;
                    tagOpened = true;
                    tagType = _uguiSymbols[j];
                    break;
                }
            }

            if (text[i] == '<' && i + (1 + 15) < len && text.Substring(i, 2 + 6).Equals("<color=#") && text[i+16] == '>') {
                _currentText += text.Substring(i, 2 + 6 + 8);
                i += (2 + 14) - 1;
                symbolDetected = true;
                tagOpened = true;
                tagType = "color";
            }

            if (text[i] == '<' && i + 5 < len && text.Substring(i, 6).Equals("<size=")) {
                var parseSize = "";
                var size = (float) label.fontSize;
                for (var j = i + 6; j < len; j++) {
                    if (text[j] == '>') break;
                    parseSize += text[j];
                }

                if (float.TryParse(parseSize, out size)) {
                    _currentText += text.Substring(i, 7 + parseSize.Length);
                    i += (7 + parseSize.Length) - 1;
                    symbolDetected = true;
                    tagOpened = true;
                    tagType = "size";                    
                }
            }

            // exit symbol
            for (var j = 0; j < _uguiCloseSymbols.Length; j++) {
                var symbol = string.Format("</{0}>", _uguiCloseSymbols[j]);
                if (text[i] == '<' && i + (2 + _uguiCloseSymbols[j].Length) < len && text.Substring(i, 3 + _uguiCloseSymbols[j].Length).Equals(symbol)) {
                    _currentText += symbol;
                    i += (3 + _uguiCloseSymbols[j].Length) - 1;
                    symbolDetected = true;
                    tagOpened = false;
                    break;
                }
            }

            if (symbolDetected) continue;

            _currentText += text[i];
            label.text = _currentText + (tagOpened? string.Format("</{0}>", tagType) : "");
            yield return new WaitForSeconds(speed);
        }

        _typeTextCoroutine = null;

        if (_onCompleteCallback != null)
            _onCompleteCallback();
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

    public void SetOnComplete(OnComplete onComplete) {
        _onCompleteCallback = onComplete;
    }

}

public static class TypeTextComponentUtility {

    public static void TypeText(this Text label, string text, float speed = 0.05f, TypeTextComponent.OnComplete onComplete = null) {
        var typeText = label.GetComponent<TypeTextComponent>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<TypeTextComponent>();
        }

        typeText.SetText(text, speed);
        typeText.SetOnComplete(onComplete);
    }

    public static bool IsSkippable(this Text label) {
        var typeText = label.GetComponent<TypeTextComponent>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<TypeTextComponent>();
        }

        return typeText.IsSkippable();
    }

    public static void SkipTypeText(this Text label) {
        var typeText = label.GetComponent<TypeTextComponent>();
        if (typeText == null) {
            typeText = label.gameObject.AddComponent<TypeTextComponent>();
        }

        typeText.SkipTypeText();
    }

}