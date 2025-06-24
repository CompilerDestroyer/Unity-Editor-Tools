﻿using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CompilerDestroyer.Runtime.Attributes;

namespace CompilerDestroyer.Editor.Attributes
{
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
