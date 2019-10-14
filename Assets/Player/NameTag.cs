using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

abstract public class NameTag : MonoBehaviour
{
    public GameObject autoChild;
    public Camera mainCamera;
    public bool Visible { get => visible; }
    public Vector2 Position { get => position; }
    public bool Selectable;
    public Color BaseColor;
    [ConditionalField("Selectable")] public Color HighlightColor;

    RectTransform rectTransform;
    Text text;
    [SerializeProperty("Visible")]
    bool visible;
    [SerializeProperty("Position")]
    Vector2 position;
    GameObject can;
    string content;

    // These only set visual stuff.
    public void Select() {
        string unselectableMsg = 
"This NameTag has Selectable set to false, yet Select was called.";
        if (!Selectable) throw new System.Exception(unselectableMsg);
        if (BaseColor == null) BaseColor = text.color;
        text.color = HighlightColor;
    }
    public void Unselect() {
        text.color = BaseColor;
    }
    public void SetText(string value) {
        text.text = value;
    }

    void OnEnable() => updateCanvasState();
    void OnDisable() => updateCanvasState();
    void updateCanvasState() {
        if (can == null) can = GetComponentInChildren<Canvas>(true)?.gameObject;
        can?.SetActive(enabled);
    }

    protected virtual void Start()
    {
        GameObject.Instantiate(autoChild, transform, true);
        text = GetComponentInChildren<Text>();
        rectTransform = text.GetComponent<RectTransform>();
        updateCanvasState();
        Unselect();
    }
    void Update()
    {
        updateText();
        drawNameTag();
    }

    void drawNameTag() {
        string noCamMsg = "Mum, get the camera!"; 
        if (mainCamera == null) 
            throw new System.NullReferenceException(noCamMsg);
        Vector3 pos = mainCamera.WorldToScreenPoint(transform.position);
        if (pointVisible(pos, mainCamera)) draw(pos);
        else undraw();
    }
    void draw(Vector2 pos) {
        rectTransform.anchoredPosition = pos;
        text.enabled = true;
        visible = true;
    }
    void undraw() {
        text.enabled = false;
        visible = false;
    }
    bool pointVisible(Vector3 p, Camera by) {
        if (p.z < 0) return false;
        if (p.x < 0 || by.pixelWidth < p.x) return false;
        if (p.y < 0 || by.pixelHeight < p.y) return false;
        return true;
    }

    protected abstract void updateText();
}
