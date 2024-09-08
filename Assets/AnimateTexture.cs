using UnityEngine;

public class AnimateTexture : MonoBehaviour
{
    public Material material; // The material with the texture
    public float speed = 1.0f; // Speed of texture movement

    private Vector2 offset;

    void Start()
    {
        // Initialize offset
        if (material != null)
        {
            offset = material.GetTextureOffset("_MainTex");
        }
    }

    void Update()
    {
        // Update the texture offset over time
        if (material != null)
        {
            offset.x += Time.deltaTime * speed;
            material.SetTextureOffset("_MainTex", offset);
        }
    }
}
