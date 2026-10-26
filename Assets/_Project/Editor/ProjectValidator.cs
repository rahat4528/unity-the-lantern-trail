using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using LanternTrail.Interaction;

namespace LanternTrail.Editor
{
    public static class ProjectValidator
    {
        private static readonly string[] RequiredScenes = new string[]
        {
            "MainMenu",
            "Location_01",
            "Location_02",
            "Location_03",
            "Ending"
        };

        [MenuItem("Tools/Lantern Trail/Validate Project")]
        public static bool ValidateProject()
        {
            int errorCount = 0;
            Debug.Log("<color=cyan>[ProjectValidator] Starting project validation...</color>");

            // 1. Check Build Settings Scenes
            errorCount += ValidateBuildSettings();

            // 2. Check Player / Android Settings
            errorCount += ValidatePlayerSettings();

            // 3. Check Collectibles in project
            errorCount += ValidateCollectibles();

            // 4. Check Interactables
            errorCount += ValidateInteractables();

            if (errorCount == 0)
            {
                Debug.Log("<color=green>[ProjectValidator] All validation checks passed successfully! (0 errors)</color>");
                return true;
            }
            else
            {
                Debug.LogError($"[ProjectValidator] Validation FAILED with {errorCount} errors!");
                return false;
            }
        }

        public static void ValidateProjectBatch()
        {
            bool success = ValidateProject();
            if (!success)
            {
                EditorApplication.Exit(1);
            }
            else
            {
                EditorApplication.Exit(0);
            }
        }

        private static int ValidateBuildSettings()
        {
            int errors = 0;
            var scenesInBuild = EditorBuildSettings.scenes;
            var sceneNamesFound = new HashSet<string>();

            foreach (var scene in scenesInBuild)
            {
                if (scene.enabled)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                    sceneNamesFound.Add(sceneName);
                }
            }

            foreach (var required in RequiredScenes)
            {
                if (!sceneNamesFound.Contains(required))
                {
                    Debug.LogError($"[ProjectValidator] Missing required scene in Build Settings: {required}");
                    errors++;
                }
            }

            return errors;
        }

        private static int ValidatePlayerSettings()
        {
            int errors = 0;

            if (PlayerSettings.defaultInterfaceOrientation != UIOrientation.LandscapeLeft &&
                PlayerSettings.defaultInterfaceOrientation != UIOrientation.LandscapeRight &&
                PlayerSettings.defaultInterfaceOrientation != UIOrientation.AutoRotation)
            {
                Debug.LogWarning("[ProjectValidator] Android orientation should be Landscape.");
            }

            string appId = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);
            if (string.IsNullOrEmpty(appId) || appId.StartsWith("com.Company"))
            {
                Debug.LogError($"[ProjectValidator] Invalid Android Application Identifier: '{appId}'");
                errors++;
            }

            return errors;
        }

        private static int ValidateCollectibles()
        {
            int errors = 0;
            var collectibles = UnityEngine.Object.FindObjectsOfType<Collectible>();
            var seenIds = new HashSet<string>();

            foreach (var c in collectibles)
            {
                if (string.IsNullOrEmpty(c.CollectibleId))
                {
                    Debug.LogError($"[ProjectValidator] Collectible on '{c.gameObject.name}' has empty CollectibleId!");
                    errors++;
                }
                else if (seenIds.Contains(c.CollectibleId))
                {
                    Debug.LogError($"[ProjectValidator] Duplicate CollectibleId '{c.CollectibleId}' found on '{c.gameObject.name}'!");
                    errors++;
                }
                else
                {
                    seenIds.Add(c.CollectibleId);
                }
            }

            return errors;
        }

        private static int ValidateInteractables()
        {
            int errors = 0;
            var interactables = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();

            foreach (var mb in interactables)
            {
                if (mb is IInteractable)
                {
                    var col = mb.GetComponent<Collider>() ?? mb.GetComponentInChildren<Collider>();
                    if (col == null)
                    {
                        Debug.LogError($"[ProjectValidator] Interactable on '{mb.gameObject.name}' is missing a Collider!");
                        errors++;
                    }
                }
            }

            return errors;
        }
    }
}
