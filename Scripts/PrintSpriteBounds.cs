using UnityEngine;

[ExecuteAlways]
public class PrintSpriteBounds : MonoBehaviour
{
    void OnEnable()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) { Debug.LogError("SpriteRenderer not found"); return; }
        var b = sr.bounds;
        Debug.Log($"Board world size: {b.size.x:F4} x {b.size.y:F4}; " +
                  $"Cell: {(b.size.x/8f):F4} x {(b.size.y/8f):F4}");
    }
}
