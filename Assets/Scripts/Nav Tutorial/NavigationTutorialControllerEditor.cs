using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavigationTutorialController))]
public class NavigationTutorialControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NavigationTutorialController tutorialController = (NavigationTutorialController)target;

        // Draw default inspector properties
        DrawDefaultInspector();

        // Draw currentStep when debug is true
        if (tutorialController.debug)
        {
            tutorialController.currentStep = EditorGUILayout.IntField("Current Step", tutorialController.currentStep);
        }
    }
}
