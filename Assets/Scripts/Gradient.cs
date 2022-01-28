using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] [RequireComponent(typeof(CanvasRenderer))]
public class Gradient : Graphic
{
    [SerializeField] private bool isSlider;
    private enum GradientType
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp,
    }

    [SerializeField] private GradientType type;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        var pivot = rectTransform.pivot;
        var rect = rectTransform.rect;

        var corner1 = new Vector3(-pivot.x * rect.width, -pivot.y * rect.height, 0);
        var corner2 = new Vector3((1 - pivot.x) * rect.width, (1 - pivot.y) * rect.height, 0);
        
        vh.Clear();

        var vertex = UIVertex.simpleVert;
        
        AddVertex(ref vertex, ref vh, corner1.x, corner1.y, type == GradientType.RightToLeft || type == GradientType.UpToDown);
        AddVertex(ref vertex, ref vh, corner1.x, corner2.y, type == GradientType.RightToLeft || type == GradientType.DownToUp);
        AddVertex(ref vertex, ref vh, corner2.x, corner2.y, type == GradientType.LeftToRight || type == GradientType.DownToUp);
        AddVertex(ref vertex, ref vh, corner2.x, corner1.y, type == GradientType.LeftToRight || type == GradientType.UpToDown);
        
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }

    private void AddVertex(ref UIVertex vertex, ref VertexHelper helper, float x, float y, bool alpha)
    {
        vertex.position = new Vector3(x, y, 0);
        vertex.color = color;

        if (alpha && !isSlider)
        {
            vertex.color = new Color32(210, 198, 9, 255);
        }
        else if (alpha && isSlider)
        {
            vertex.color.a = 0;
        }
        
        helper.AddVert(vertex);
    }
}
