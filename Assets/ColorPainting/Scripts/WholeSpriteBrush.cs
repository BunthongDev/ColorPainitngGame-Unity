using System;
using UnityEngine;

public class WholeSpriteBrush : MonoBehaviour
{
    [Header("Settings ")]
    [SerializeField] private Color color;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            RaycastSprites();
    }
    
    
    // old code of RaycastSprite
    private void RaycastSprite()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dicrection = Vector2.zero;
        
        RaycastHit2D hit = Physics2D.Raycast(origin, dicrection);
        
        // if (hit.collider != null)
        //     return;
            
        ColorSprite(hit.collider);
        
        Debug.Log("Hit: " + hit.collider.name);
    }
    
    // new code of RaycastSprites
    private void RaycastSprites()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dicrection = Vector2.zero;
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dicrection);
        
        //we haven't detected anything 
        if(hits.Length <= 0)
            return;
            
        int highestOrderInLayer = -1000;
        int topIndex = -1;
            
        for (int i = 0; i < hits.Length; i++)
        {
            SpriteRenderer spriteRenderer = hits[i].collider.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                continue;
                
                //At this point we have our sprite renderer
            int orderInLayer = spriteRenderer.sortingOrder;
            
            if (orderInLayer > highestOrderInLayer)
            {
                highestOrderInLayer = orderInLayer;
                topIndex = i;
            }
        }
            
            //At the end of the loop we know the highest order in layer
            Collider2D highestCollider = hits[topIndex].collider;
            
            // Finally we can color the sprite rederer
            ColorSprite(highestCollider);
        
    }
            
        
    
    
    private void ColorSprite(Collider2D collider)
    {
        SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;
        
        //We have detected a sprite renderer
        // We can color it ! 
        
        spriteRenderer.color = color;
        
    }
    
    
}
