using UnityEngine.UI;

namespace Framework
{
    public class EmptyGraphic: Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
