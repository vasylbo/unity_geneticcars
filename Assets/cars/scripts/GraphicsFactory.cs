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

    private static void drawOnTexture(Texture2D pTexture, int size) {
        int r = size / 2;

        // fill texture with clear pixels
        for (var x = 0; x < pTexture.width; x++) {
            for (var y = 0; y < pTexture.height; y++) {
                pTexture.SetPixel(x, y, Color.clear);
            }
        }

        // sending 640 rays from center of texture each 0.5 degree 
        for (float i = 0; i < 360; i+= 0.5f) {
            // calculate angle in radians 
            var angle = Mathf.Deg2Rad * i;
            // draw points on line of this angle from r - 30 to r
            for (var j = r - 30; j < r; j++) {
                // calculate x and y 
                int x = (int) (Mathf.Cos(angle) * j + r);
                int y = (int) (Mathf.Sin(angle) * j + r);

                // setting needed color to texture
                pTexture.SetPixel(x, y, Color.black);
            }
        }
    }
}
