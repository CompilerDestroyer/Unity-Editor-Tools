using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CompilerDestroyer.Editor.Attributes
{
    /// <summary>
    /// ReadOnly attribute to make properties non-editable in the Inspector.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }

    /// <summary>
    /// Custom UIElements PropertyDrawer for ReadOnlyAttribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            PropertyField propertyField = new PropertyField(property);

            propertyField.SetEnabled(false);

            container.Add(propertyField);

            return container;
        }
    }
}
