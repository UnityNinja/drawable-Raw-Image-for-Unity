using UnityEngine;
using UnityEngine.UI;

public class DrawOnImage : MonoBehaviour
{
    private RawImage drawImage;
    private int scaleFactor;

    private Texture2D canvasTexture;
    private Color32[] canvasColors;
    private Vector2 previousPosition;

    [SerializeField] private float brushSize = 10f;
    [SerializeField] private Color brushColor = Color.black;

    void Awake()
    {
        drawImage = GetComponent<RawImage>();
    }

    void Start()
    {
        Texture2D originalTexture = drawImage.texture as Texture2D;

        // calculate scaleFactor automatically based on the width of the RawImage and the original texture
        float widthRatio = drawImage.rectTransform.rect.width / (float)originalTexture.width;
        float heightRatio = drawImage.rectTransform.rect.height / (float)originalTexture.height;
        scaleFactor = Mathf.RoundToInt(Mathf.Max(widthRatio, heightRatio));

        canvasTexture = new Texture2D(originalTexture.width * scaleFactor, originalTexture.height * scaleFactor, TextureFormat.RGBA32, false);

        canvasTexture.filterMode = FilterMode.Point;
        canvasTexture.wrapMode = TextureWrapMode.Clamp;

        // Create a new array of size equal to canvasTexture.width multiplied by canvasTexture.height
        canvasColors = new Color32[canvasTexture.width * canvasTexture.height];

        // Copy the pixels from the original texture to the new array
        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {
                Color32 pixel = originalTexture.GetPixel(x, y);
                for (int i = 0; i < scaleFactor; i++)
                {
                    for (int j = 0; j < scaleFactor; j++)
                    {
                        int index = ((y * scaleFactor) + j) * canvasTexture.width + ((x * scaleFactor) + i);
                        canvasColors[index] = pixel;
                    }
                }
            }
        }

        canvasTexture.SetPixels32(canvasColors);
        canvasTexture.Apply();

        drawImage.texture = canvasTexture;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentPosition = GetLocalCursor();
            DrawLineTo(currentPosition);
        }
        else
        {
            previousPosition = Vector2.zero;
        }
    }

    private void DrawLineTo(Vector2 currentPosition)
    {
        if (previousPosition == Vector2.zero)
        {
            previousPosition = currentPosition;
            return;
        }

        float distance = Vector2.Distance(previousPosition, currentPosition);
        int steps = Mathf.CeilToInt(distance / brushSize);
        if (steps == 0) steps = 1;

        for (int i = 0; i < steps; i++)
        {
            Vector2 lerpedPosition = Vector2.Lerp(previousPosition, currentPosition, i / (float)steps);
            DrawPixel((int)lerpedPosition.x, (int)lerpedPosition.y, Color.black, brushSize);
        }

        canvasTexture.SetPixels32(canvasColors);
        canvasTexture.Apply();
        previousPosition = currentPosition;
    }

    private void DrawPixel(int x, int y, Color color, float size)
    {
        int width = canvasTexture.width;
        int height = canvasTexture.height;
        int radius = Mathf.CeilToInt(size / 2);

        for (int i = x - radius; i < x + radius; i++)
        {
            if (i < 0 || i >= width) continue;

            for (int j = y - radius; j < y + radius; j++)
            {
                if (j < 0 || j >= height) continue;

                if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) <= radius)
                {
                    int index = j * width + i;
                    Color32 c = brushColor;
                    c.a = canvasColors[index].a;
                    canvasColors[index] = c;
                }
            }
        }
    }

    private Vector2 GetLocalCursor()
    {
        Vector2 cursor = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawImage.rectTransform, cursor, null, out Vector2 localCursor);
        localCursor += new Vector2(canvasTexture.width / 2f, canvasTexture.height / 2f);
        return localCursor;
    }

}
