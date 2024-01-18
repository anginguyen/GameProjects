using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueIndicator : MonoBehaviour
{
    public bool isSprite = false; 

    Renderer renderer;
    Material originalMaterial;
    SpriteRenderer sprite;
    Color glowColor; 

    Color spriteColor = new Color(1, 1, 1);
    Color darkSpriteColor = new Color(0.075f, 0.075f, 0.075f);
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
        sprite = GetComponent<SpriteRenderer>(); 

        if (!isSprite) {
            glowColor = originalMaterial.GetColor("_EmissionColor");
        }
    }

    // Indicator when player is/is not looking at clue 
    public void EnableIndicator(Material indicator) {
        renderer.material = indicator;
    }

    public void DisableIndicator() {
        renderer.material = originalMaterial;
        if (isSprite) sprite.color = spriteColor;                           // Regular sprite (white)
        else renderer.material.SetColor("_EmissionColor", glowColor);       // White glow 
    }

    // No glow when blacklight is on 
    public void NoGlow() {
        renderer.material = originalMaterial;
        if (isSprite) sprite.color = darkSpriteColor;                       // Dark sprite (no glow)
        else renderer.material.SetColor("_EmissionColor", Color.black);     // Regular (no glow) 
    }

    // Normal clue glow when in view 
    public void NormalClue() {
        renderer.material = originalMaterial;
        if (isSprite) sprite.color = spriteColor;                           // Regular sprite (white)
        else renderer.material.SetColor("_EmissionColor", Color.black);     // Regular (no glow)
    }
}
