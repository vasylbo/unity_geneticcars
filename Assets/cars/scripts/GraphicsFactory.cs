using UnityEngine;
using System.Collections;

public class GraphicsFactory {
    // size for wheel square texture
    private const int WHEEL_TEXTURE_SIZE = 256;
    // cached wheel texture
    private static Texture2D cachedWheelTexture;

    public static Sprite createWheelSprite() {
        // if texture not generated - generate it
        if (cachedWheelTexture == null) {
            cachedWheelTexture = generateWheelTexture();
        }

        int size = WHEEL_TEXTURE_SIZE;
        // creating 
        Sprite sprite = Sprite.Create(
            cachedWheelTexture, // out texture
            new Rect(0, 0, size, size), // rect of texture that will be shown in sprite
            new Vector2(0.5f, 0.5f), // pivot anchor of sprite
            size // pixels per unit (we need out sprite to be 1 on 1, so we need all of "size" pixels in one unit 
        );

        return sprite;
    }

    private static Texture2D generateWheelTexture() {
        int size = WHEEL_TEXTURE_SIZE;

        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp; // clamp texture to edges when scaled

        // clean texture from being grey color filled and draw a wheel on it
        drawOnTexture(texture, size);

        // IMPORTANT: you have to apply changes made by SetPixel
        texture.Apply();

        return texture;
    }

    // https://en.wikipedia.org/wiki/Midpoint_circle_algorithm
    // plots a circle in a square texture 
    private static void drawOnTexture(Texture2D pTexture, int size) {
        // radius of circle
        int r = size / 2;
        // center of texture
        int cx = r;
        int cy = r;

        // fill texture with clear pixels
        for (var i = 0; i < pTexture.width; i++) {
            for (var j = 0; j < pTexture.height; j++) {
                pTexture.SetPixel(i, j, Color.clear);
            }
        }

        int x = r;
        int y = 0;
        int decOver2 = 1 - x;

        Color wheelColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        while (y <= x) {
            for (var i = -x + 2; i < x - 2; i++) {
                pTexture.SetPixel(cx + i, cy + y - 2, wheelColor);
                pTexture.SetPixel(cx + i, cy - y + 2, wheelColor);
            }
            for (var j = -y + 2; j < y - 2; j++) {
                pTexture.SetPixel(cx + j, cy + x - 2, wheelColor); 
                pTexture.SetPixel(cx + j, cy - x + 2, wheelColor);
            }

            pTexture.SetPixel(cx + x, cy + y, Color.black); // 1 octant 
            pTexture.SetPixel(cx + y, cy + x, Color.black); // 2 octant 
            pTexture.SetPixel(cx - x, cy + y, Color.black); // 3 octant 
            pTexture.SetPixel(cx - y, cy + x, Color.black); // 4 octant 

            pTexture.SetPixel(cx - x, cy - y, Color.black); // 5 octant 
            pTexture.SetPixel(cx - y, cy - x, Color.black); // 6 octant 
            pTexture.SetPixel(cx + x, cy - y, Color.black); // 7 octant 
            pTexture.SetPixel(cx + y, cy - x, Color.black); // 8 octant 

            ++y;
            if (decOver2 <= 0) {
                decOver2 += 2 * y + 1;
            } else {
                --x;
                decOver2 += 2 * (y - x) + 1;
            }
        };

        // draw a line
        for (var i = 0; i < r; i++) {
            pTexture.SetPixel(i, cy, Color.black);
        }
    }
}
