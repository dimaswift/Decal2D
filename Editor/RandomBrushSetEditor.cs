using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(RandomBrushSet))]
    public class RandomBrushSetEditor : BrushSetEditor<BrushContainer>
    {
        public override BrushSet<BrushContainer> brushSet
        {
            get
            {
                return (RandomBrushSet) target;
            }
        }
    }
}
