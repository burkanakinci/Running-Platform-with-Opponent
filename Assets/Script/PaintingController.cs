using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaintingController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Texture2D brush;
    [SerializeField] private Vector2Int textureArea;
    Camera cam;
    Texture2D texture;
    Color32[] textureColor;
    Color32[] brushColor;
    Vector2Int halfBrush;
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] private GameObject rayPoint;
    float rotatingAxis;

    int percent = 0, paintedPixel = 0, totalPixel, maxValue = 100;
    void Start()
    {
        rotatingAxis = 45f;

        cam = Camera.main;
        texture = new Texture2D(textureArea.x, textureArea.y, TextureFormat.ARGB32, false);
        meshRenderer.material.mainTexture = texture;

        fill.fillAmount = Normalise();
        amount.text = $"{percent}/{maxValue}";

        InvokeRepeating("ChangeAxis", 1f, 1f);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(rayPoint.transform.position, rayPoint.transform.forward, out hit))
            {
                Paint(hit.textureCoord);
            }
        }

        rayPoint.transform.Rotate(rotatingAxis * Time.deltaTime, 0f, 0f, Space.Self);

    }
    private void Paint(Vector2 coordinate)
    {

        coordinate.x *= texture.width;
        coordinate.y *= texture.height;

        textureColor = texture.GetPixels32();
        brushColor = brush.GetPixels32();

        halfBrush = new Vector2Int(brush.width / 2, brush.height / 2);

        for (int x = 0; x < brush.width; x++)
        {
            int xPos = x - halfBrush.x + (int)coordinate.x;
            if (xPos < 0 || xPos >= texture.width)
                continue;

            for (int y = 0; y < brush.height; y++)
            {

                int yPos = y - halfBrush.y + (int)coordinate.y;
                if (yPos < 0 || yPos >= texture.height)
                    continue;

                if (brushColor[x + (y * brush.width)].a == 255)
                {
                    int tPos = xPos + (texture.width * yPos);

                    if (textureColor[tPos].g != 0)
                    {
                        textureColor[tPos] = brushColor[x + (y * brush.width)];
                        paintedPixel++;
                    }
                }
            }
        }
        texture.SetPixels32(textureColor);
        texture.Apply();
        PaintingPercent();
    }

    void PaintingPercent()
    {
        totalPixel = texture.height * texture.width;

        percent = (paintedPixel * 100) / totalPixel;
        fill.fillAmount = Normalise();
        amount.text = $"{percent}/{maxValue}";
    }
    float Normalise()
    {
        return (float)percent / maxValue;
    }
    void ChangeAxis(){

        rotatingAxis*=-1f;
    }
}
