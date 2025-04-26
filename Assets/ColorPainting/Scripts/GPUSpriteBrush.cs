using System.Collections.Generic;
using UnityEngine;

public class GPUSpriteBrush : MonoBehaviour
{
    public static GPUSpriteBrush instance;

    [Header(" Elements ")]
    private SpriteRenderer currentSpriteRenderer;

    [Header(" Settings ")]
    [SerializeField] private Color color;
    [Range(0, 2)]
    [SerializeField] private float brushSize;
    [SerializeField] private Material brushMaterial;
    private Dictionary<int, Texture2D> originalTextures = new Dictionary<int, Texture2D>();
    private Dictionary<int, Texture2D> editedTextures = new Dictionary<int, Texture2D>();
    private Dictionary<int, RenderTexture> renderTextures = new Dictionary<int, RenderTexture>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            RaycastSprites();
        else if (Input.GetMouseButton(0))
            RaycastCurrentSprite();
    }

    private void RaycastSprites()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = Vector2.zero;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);

        // We haven't detected anything
        if (hits.Length <= 0)
            return;

        int highestOrderInLayer = -1000;
        int topIndex = -1;

        for (int i = 0; i < hits.Length; i++)
        {
            SpriteRenderer spriteRenderer = hits[i].collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
                continue;

            // At this point, we have our sprite renderer
            int orderInLayer = spriteRenderer.sortingOrder;

            if (orderInLayer > highestOrderInLayer)
            {
                highestOrderInLayer = orderInLayer;
                topIndex = i;
            }
        }

        // At the end of the loop, we know the highest order in layer
        Collider2D highestCollider = hits[topIndex].collider;

        // Store the current sprite renderer
        currentSpriteRenderer = highestCollider.GetComponent<SpriteRenderer>();

        // Check if we have this current sprite original texture or not
        int key = currentSpriteRenderer.transform.GetSiblingIndex();

        int width = currentSpriteRenderer.sprite.texture.width;
        int height = currentSpriteRenderer.sprite.texture.height;

        if (!originalTextures.ContainsKey(key))
        {
            originalTextures.Add(key, currentSpriteRenderer.sprite.texture);
            editedTextures.Add(key, new Texture2D(width, height));

            RenderTexture rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, 10);
            rt.useMipMap = true;

            renderTextures.Add(key, rt);
        }

        // Finally color the Sprite Renderer !
        ColorSpriteAtPosition(highestCollider, hits[topIndex].point);
    }

    private void RaycastCurrentSprite()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = Vector2.zero;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.TryGetComponent(out SpriteRenderer spriteRenderer))
                continue;

            if (spriteRenderer == currentSpriteRenderer)
            {
                ColorSpriteAtPosition(hits[i].collider, hits[i].point);
                break;
            }
        }
    }

    private void ColorSpriteAtPosition(Collider2D collider, Vector2 hitPoint)
    {
        SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return;

        // Convert our hitPoint (World Space) to a texture point
        Vector2 texturePoint = WorldToTexturePoint(spriteRenderer, hitPoint);

        Sprite sprite = spriteRenderer.sprite;

        int key = currentSpriteRenderer.transform.GetSiblingIndex();
        Texture2D originalTexture = originalTextures[key];

        Texture2D tex = editedTextures[key];

        if(sprite.texture != tex)
            Graphics.CopyTexture(sprite.texture, tex);

        brushMaterial.SetTexture("_MainTex", tex);
        brushMaterial.SetTexture("_Original", originalTexture);
        brushMaterial.SetColor("_Color", color);
        brushMaterial.SetFloat("_BrushSize", brushSize);
        brushMaterial.SetVector("_UVPosition", texturePoint / sprite.texture.width);

        RenderTexture rt = renderTextures[key];

        Graphics.Blit(tex, rt, brushMaterial);

        // At this point, we have a colored render texture

        Graphics.CopyTexture(rt, tex);

        //tex.Apply();

        Sprite newSprite = Sprite.Create(tex, sprite.rect, Vector2.one / 2, sprite.pixelsPerUnit);
        spriteRenderer.sprite = newSprite;
    }

    private Vector2 WorldToTexturePoint(SpriteRenderer sr, Vector2 worldPos)
    {
        Vector2 texturePoint = sr.transform.InverseTransformPoint(worldPos);

        // Position between -.5 & .5
        texturePoint.x /= sr.bounds.size.x;
        texturePoint.y /= sr.bounds.size.y;

        // Position between 0 & 1
        texturePoint += Vector2.one / 2;

        // Offset in Texture space
        texturePoint.x *= sr.sprite.rect.width;
        texturePoint.y *= sr.sprite.rect.height;

        // Position in Texture Space
        texturePoint.x += sr.sprite.rect.x;
        texturePoint.y += sr.sprite.rect.y;

        return texturePoint;
    }

    private void ColorSprite(Collider2D collider)
    {
        SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return;

        //Texture2D tex = spriteRenderer.sprite.texture;
        Sprite sprite = spriteRenderer.sprite;

        Texture2D tex = new Texture2D(sprite.texture.width, sprite.texture.height);

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                Color pixelColor = color;
                pixelColor.a = sprite.texture.GetPixel(x, y).a;

                pixelColor = pixelColor * sprite.texture.GetPixel(x, y);

                tex.SetPixel(x, y, pixelColor);
            }
        }

        tex.Apply();

        Sprite newSprite = Sprite.Create(tex, sprite.rect, Vector2.one / 2, sprite.pixelsPerUnit);
        spriteRenderer.sprite = newSprite;
    }

    public void SetBrushSize(float brushSize)
    {
        this.brushSize = brushSize;
    }
    
    public void SetColor(Color color)
    {
        this.color = color;
    }
}
