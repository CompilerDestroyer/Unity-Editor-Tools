using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CompilerDestroyer.Editor.EditorVisual
{
    public class RemoveScriptingDefineSymbolsFromBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private static readonly string[] DEFINESTOREMOVE = { };
        private string allDefineSymbols;

        // Called BEFORE the build starts
        public void OnPreprocessBuild(BuildReport report)
        {
            if (DEFINESTOREMOVE.Length == 0) return;

            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(report.summary.platformGroup);
            string defineSymbols = PlayerSettings.GetScriptingDefineSymbols(buildTarget);
            allDefineSymbols = defineSymbols;

            foreach (var define in DEFINESTOREMOVE)
            {
                if (defineSymbols.Contains(define))
                {
                    defineSymbols = defineSymbols.Replace(define, "").Replace(";;", ";").Trim(';');
                }
            }
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defineSymbols);
        }

        // Called AFTER the build starts
        public void OnPostprocessBuild(BuildReport report)
        {
            if (DEFINESTOREMOVE.Length == 0) return;

            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(report.summary.platformGroup);

            EditorApplication.delayCall += () =>
            {
                PlayerSettings.SetScriptingDefineSymbols(buildTarget, allDefineSymbols);
            };
        }
    }
}
