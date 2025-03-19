using UnityEngine;
using UnityEngine.UIElements;
using CompilerDestroyer.Editor;
namespace CompilerDestroyer.Editor.UIElements
{
    /// <summary>
    /// A custom visual element that represents a line. This line can be either horizontal or vertical, and its length and color can be customized.
    /// </summary>
    [UxmlElement]
    public partial class DefaultEditorIcon : VisualElement
    {

        public DefaultEditorIcon()
        {
            style.height = 1;
            style.width = Length.Percent(100f);
            style.backgroundColor = GlobalVariables.DefaultLineColor;
        }

        public DefaultEditorIcon(float lineLength = 1f, bool isVertical = false)
        {
            style.backgroundColor = GlobalVariables.DefaultLineColor;

            lineLength = Mathf.Clamp(lineLength, 1f, 200f);

            if (isVertical)
            {
                style.width = lineLength;
            }
            else
            {
                style.height = lineLength;
            }

        }
    }

}
