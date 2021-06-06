using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDApuntadoBases : MonoBehaviour
{
    public GameObject objetivo;
    public Camera camara;
    public RectTransform canvasRect;
    private RectTransform rectTransform;

    public GameObject flecha;
    public GameObject marcador;

    float offsetFuera = 45f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        SetPos();
    }

    void SetPos()
    {
        Vector3 pos = camara.WorldToScreenPoint(objetivo.transform.position);

        if(pos.z >= 0 && pos.x <= canvasRect.rect.width * canvasRect.localScale.x && pos.y <= canvasRect.rect.height * canvasRect.localScale.x && pos.x >= 0f && pos.y >= 0f)
        {
            pos.z = 0f;

            FueraVista(false, pos);
        }
        else if (pos.z >= 0f)
        {
            pos = FueraRango(pos);
            FueraVista(true, pos);
        }
        else
        {
            pos *= -1f;

            pos = FueraRango(pos);
            FueraVista(true, pos);
        }
        rectTransform.position = pos;
    }

    private Vector3 FueraRango(Vector3 pos)
    {
        pos.z = 0f;

        Vector3 centroCanvas = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        pos -= centroCanvas;

        float divX = (canvasRect.rect.width / 2f - offsetFuera) / Mathf.Abs(pos.x);
        float divY = (canvasRect.rect.height / 2f - offsetFuera) / Mathf.Abs(pos.y);

        if(divX < divY)
        {
            float angulo = Vector3.SignedAngle(Vector3.right, pos, Vector3.forward);
            pos.x = Mathf.Sign(pos.x) * (canvasRect.rect.width * 0.5f - offsetFuera) * canvasRect.localScale.x;
            pos.y = Mathf.Tan(Mathf.Deg2Rad * angulo) * pos.x;
        }
        else
        {
            float angulo = Vector3.SignedAngle(Vector3.up, pos, Vector3.forward);
            pos.y = Mathf.Sign(pos.y) * (canvasRect.rect.height / 2f - offsetFuera) * canvasRect.localScale.y;
            pos.x = Mathf.Tan(Mathf.Deg2Rad * angulo) * pos.y;
        }

        pos += centroCanvas;
        return pos;
    }

    void FueraVista(bool fuera, Vector3 pos)
    {
        if(fuera)
        {
            marcador.SetActive(true);
            flecha.transform.rotation = Quaternion.Euler(rotacionFlecha(pos));
        }
        else
        {
            marcador.SetActive(false);
        }
    }
    Vector3 rotacionFlecha(Vector3 pos)
    {
        
        Vector3 centroCanvas = new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f) * canvasRect.localScale.x;
        float angulo = Vector3.SignedAngle(Vector3.down, pos - centroCanvas, Vector3.forward);
        return new Vector3(0f, 0f, angulo);
    }
}
