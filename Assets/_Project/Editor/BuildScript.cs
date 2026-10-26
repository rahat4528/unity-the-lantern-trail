using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LanternTrail.Editor
{
    public static class BuildScript
    {
        private const string OutputDirectory = "Builds/Android";
        private const string OutputApkName = "LanternTrail.apk";

        [MenuItem("Tools/Lantern Trail/Build Android APK")]
        public static void BuildAndroid()
        {
            Debug.Log("[BuildScript] Starting Android APK build...");

            // Ensure destination directory exists
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("[BuildScript] No enabled scenes found in EditorBuildSettings!");
                if (Application.isBatchMode) EditorApplication.Exit(1);
                return;
            }

            string destinationPath = Path.Combine(OutputDirectory, OutputApkName);

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = destinationPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"<color=green>[BuildScript] Android Build SUCCEEDED: {summary.totalSize} bytes in {summary.totalTime.TotalSeconds:F1}s</color>");
                Debug.Log($"[BuildScript] Output APK location: {destinationPath}");
                if (Application.isBatchMode) EditorApplication.Exit(0);
            }
            else
            {
                Debug.LogError($"[BuildScript] Android Build FAILED: {summary.totalErrors} errors.");
                if (Application.isBatchMode) EditorApplication.Exit(1);
            }
        }
    }
}
