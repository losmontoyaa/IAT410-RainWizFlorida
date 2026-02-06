using UnityEngine;

public class PlayerShaderController : MonoBehaviour
{

    public float radius = 5f;

    void Update()
    {
        Shader.SetGlobalVector("_PlayerPos", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }
}
