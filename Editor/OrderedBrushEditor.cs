using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(OrderedBrush))]
    public class OrderedBrushSetEditor : BrushSetEditor<OrderedBrushBinding>
    {
        protected override string headerName
        {
            get
            {
                return "Ordered brushes";
            }
        }

        public override BrushSet<OrderedBrushBinding> brushSet
        {
            get
            {
                return (OrderedBrush) target;
            }
        }
    }
}
