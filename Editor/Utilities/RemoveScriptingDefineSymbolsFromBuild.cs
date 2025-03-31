using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;

namespace CompilerDestroyer.Editor.Utilities
{
    public class RemoveScriptingDefineSymbolsFromBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public static List<string> defines = new List<string>();
        private string allDefineSymbols;


        // Called BEFORE the build starts
        public void OnPreprocessBuild(BuildReport report)
        {
            if (defines.Count == 0) return;

            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(report.summary.platformGroup);
            string defineSymbols = PlayerSettings.GetScriptingDefineSymbols(buildTarget);
            allDefineSymbols = defineSymbols;

            for (int i = 0; i < defines.Count; i++)
            {
                string currentDefine = defines[i];

                if (defineSymbols.Contains(currentDefine))
                {
                    defineSymbols = defineSymbols.Replace(currentDefine, "").Replace(";;", ";").Trim(';');
                }
            }
            
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defineSymbols);
        }

        // Called AFTER the build starts
        public void OnPostprocessBuild(BuildReport report)
        {
            if (defines.Count == 0) return;

            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(report.summary.platformGroup);

            EditorApplication.delayCall += () =>
            {
                PlayerSettings.SetScriptingDefineSymbols(buildTarget, allDefineSymbols);
            };
        }
    }
}