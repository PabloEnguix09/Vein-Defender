using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MejoraItem : MonoBehaviour
{
    public string nombre;
    public string nombreMejora;
    public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = transform.GetComponentInParent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
