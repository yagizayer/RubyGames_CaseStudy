using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Root.Scripts.Helpers.Components
{
    public class InverseMask : Image
    {
        private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering
        {
            get
            {
                var material = new Material(base.materialForRendering);
                material.SetInt(StencilComp, (int)CompareFunction.NotEqual);
                return material;
            }
        }

        [ContextMenu("Align")]
        private void AlignToParent()
        {
            if (transform.root == transform) return;
            if (transform.parent.GetComponent<RectTransform>() == null) return;

            GetComponent<RectTransform>().anchoredPosition =
                transform.parent.GetComponent<RectTransform>().anchoredPosition * -1;
        }
    }
}