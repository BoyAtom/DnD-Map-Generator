using UnityEngine;

public class Redactor : MonoBehaviour
{
    public MeshRenderer mapTexture;

    public void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            Texture2D texture = (Texture2D)mapTexture.materials[0].mainTexture;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            try
            {
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= texture.width;
                    pixelUV.y *= texture.height;
                    Vector2 position = new Vector2(pixelUV.x, pixelUV.y);

                    Debug.Log("Drawing: " + (int)position.x + " / " + (int)position.y);
                    texture.SetPixel((int)position.x, (int)position.y, Color.red);
                    texture.Apply();
                }
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log("Tried to draw on missing texture: "+e.ToString());
            }
            /*
            Vector2 pixelUV = hit.textureCoord;
            Debug.Log(hit.textureCoord);
            pixelUV.x *= texture.width;
            pixelUV.y *= texture.height;

            Debug.Log(pixelUV.x +" / "+ pixelUV.y);
            texture.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
            texture.Apply();

            if (hit.collider != null)
            {
                Debug.Log(hit.transform.name);
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;
                Vector2 position = new Vector2(pixelUV.x, pixelUV.y);
                tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
                tex.Apply();
            }
            */
        }
    }

    public static void SetPoint(Texture2D texture, int x, int y, Color color)
    {
        texture.SetPixel(x, y, color);
    }
}