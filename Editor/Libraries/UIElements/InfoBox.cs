using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace CompilerDestroyer.Editor.UIElements
{
    /// <summary>
    /// A custom visual element that represents a line. This line can be either horizontal or vertical, and its length and color can be customized.
    /// </summary>
    [UxmlElement]
    public partial class InfoBox : Box
    {
        public Image infoImage { get; private set; }


        /// <summary>
        /// Creates an InfoBox.
        /// </summary>
        public InfoBox()
        {
            Color notSoBlack = new Color(Color.black.r, Color.black.g, Color.black.b, 0.3f);
            style.backgroundColor = notSoBlack;
        }

        /// <summary>
        /// Create an infobox with borderOfBox.
        /// </summary>
        /// <param name="borderOfBox">Border radius.</param>
        public InfoBox(float borderOfBox = 5f)
        {
            // Set up horizontal layout
            style.flexDirection = FlexDirection.Row;
            
            // Create and style the info image
            infoImage = new Image();
            infoImage.style.alignSelf = Align.Center;

            infoImage.image = EditorGUIUtility.IconContent("console.infoicon@2x").image;
            Add(infoImage);

            
            style.backgroundColor = new Color(0f, 0f, 0f, 0.3f);
            style.borderTopLeftRadius = borderOfBox;
            style.borderTopRightRadius = borderOfBox;
            style.borderBottomLeftRadius = borderOfBox;
            style.borderBottomRightRadius = borderOfBox;
        }

        /// <summary>
        /// Create a UIElement box with control of all corners radius with one variable and background color.
        /// </summary>
        /// <param name="borderOfBox">All borders radius of box.</param>
        /// <param name="boxBackgroundColor">Background color of box.</param>
        public InfoBox(float borderOfBox = 5f, Color boxBackgroundColor = default)
        {
            if (boxBackgroundColor == default)
            {
                style.backgroundColor = new Color(0f, 0f, 0f, 0.3f);
            }
            else
            {
                style.backgroundColor = boxBackgroundColor;
            }

            style.borderTopLeftRadius = borderOfBox;
            style.borderTopRightRadius = borderOfBox;
            style.borderBottomLeftRadius = borderOfBox;
            style.borderBottomRightRadius = borderOfBox;
        }

        /// <summary>
        /// Create box with control of four corners and background color.
        /// </summary>
        /// <param name="borderTopLeftRadius">Top left radius of box.</param>
        /// <param name="borderTopRightRadius">Top right radius of box.</param>
        /// <param name="borderBottomLeftRadius">Bottom left radius of box.</param>
        /// <param name="borderBottomRightRadius">Bottom right radius of box.</param>
        /// <param name="boxBackgroundColor">Background color of box.</param>
        public InfoBox(float borderTopLeftRadius = 0f, float borderTopRightRadius = 0f, float borderBottomLeftRadius = 0f, float borderBottomRightRadius = 0f,
            Color boxBackgroundColor = default)
        {
            if (boxBackgroundColor == default)
            {
                style.backgroundColor = new Color(0f, 0f, 0f, 0.3f);
            }
            else
            {
                style.backgroundColor = boxBackgroundColor;
            }

            style.borderTopLeftRadius = borderTopLeftRadius;
            style.borderTopRightRadius = borderTopRightRadius;
            style.borderBottomLeftRadius = borderBottomLeftRadius;
            style.borderBottomRightRadius = borderBottomRightRadius;
        }
    }
}