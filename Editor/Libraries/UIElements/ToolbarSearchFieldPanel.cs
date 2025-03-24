using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace CompilerDestroyer.Editor.UIElements
{
    /// <summary>
    /// Exact same UIElement with ToolbarSearchField but search algorithm implemented.
    /// </summary>
    [UxmlElement]
    public partial class ToolbarSearchPanel : ToolbarSearchField
    {

        public ToolbarSearchPanel()
        {
            //resultList.Clear();
            //this.RegisterValueChangedCallback();

        }
        public ToolbarSearchPanel(List<string> searchList, List<string> resultList, Action OnListViewEmpty = null, Action OnListViewFilled = null)
        {
            resultList.Clear();
            this.RegisterValueChangedCallback(evt =>
            {
                if (!string.IsNullOrEmpty(this.value))
                {
                    string searchQuery = this.value;
                    searchQuery.Trim();

                    resultList.Clear();

                    for (int i = 0; i < searchList.Count; i++)
                    {
                        string searchString = searchList[i];
                        if (searchString.ToLower().Contains(searchQuery.ToLower()))
                        {
                            if (!resultList.Contains(searchString))
                            {
                                resultList.Add(searchString);
                            }

                        }
                    }

                    OnListViewFilled?.Invoke();
                }
                else
                {
                    OnListViewEmpty?.Invoke();
                }
            });

        }
    }
}
