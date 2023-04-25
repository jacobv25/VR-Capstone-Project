using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialController))]
public class TutorialControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TutorialController tutorialController = (TutorialController)target;

        // Draw default inspector properties
        DrawDefaultInspector();

        // Draw currentStep when debug is true
        if (tutorialController.debug)
        {
            tutorialController.currentStep = EditorGUILayout.IntField("Current Step", tutorialController.currentStep);
        }
    }
}
