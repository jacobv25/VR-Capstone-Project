using UnityEngine;
using UnityEditor;
using Pixelplacement;

public class RaceEditorWindow : EditorWindow
{
    private Spline _spline;
    private Vector2 _scrollPos;
    private bool _anchorPlacementMode;
    public GameObject IndicatorPrefab;


    [MenuItem("Window/Race Editor")]
    public static void ShowWindow()
    {
        GetWindow<RaceEditorWindow>("Race Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Race Editor", EditorStyles.boldLabel);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        _spline = EditorGUILayout.ObjectField("Spline Object", _spline, typeof(Spline), true) as Spline;
        IndicatorPrefab = EditorGUILayout.ObjectField("Indicator Prefab", IndicatorPrefab, typeof(GameObject), true) as GameObject;

        if (_spline != null)
        {
            _anchorPlacementMode = EditorGUILayout.Toggle("Anchor Placement Mode", _anchorPlacementMode);
            if (_anchorPlacementMode)
            {
                SceneView.duringSceneGui -= OnSceneGUI;
                SceneView.duringSceneGui += OnSceneGUI;
            }
            else
            {
                SceneView.duringSceneGui -= OnSceneGUI;
            }

            if (GUILayout.Button("Calculate Spline Length"))
            {
                float splineLength = CalculateSplineLength(_spline);
                EditorUtility.DisplayDialog("Spline Length", $"Total spline length: {splineLength} units", "OK");
            }

            if (GUILayout.Button("Place Indicators"))
            {
                PlaceIndicators();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(worldRay, out hit))
            {
                Vector3 hitPoint = hit.point;
                AddAnchorAtPoint(hitPoint);
                e.Use();
            }
        }
    }

    private void AddAnchorAtPoint(Vector3 point)
    {
        if (_spline != null)
        {
            GameObject anchor = _spline.AddAnchors(1)[0];
            anchor.transform.position = point;
        }
    }

    private float CalculateSplineLength(Spline spline, int samples = 1000)
    {
        float totalDistance = 0;

        if (samples < 2) samples = 2;

        Vector3 previousPoint = spline.GetPosition(0);

        for (int i = 1; i <= samples; i++)
        {
            float t = (float)i / (samples - 1);
            Vector3 currentPoint = spline.GetPosition(t);

            totalDistance += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        return totalDistance;
    }

    private void PlaceIndicators()
    {
        if (_spline != null && IndicatorPrefab != null)
        {
            int anchorCount = _spline.Anchors.Length;

            for (int i = 0; i < anchorCount; i++)
            {
                GameObject indicator = Instantiate(IndicatorPrefab, _spline.Anchors[i].transform.position, Quaternion.identity);
                indicator.transform.SetParent(_spline.transform);

                if (i < anchorCount - 1)
                {
                    Vector3 direction = (_spline.Anchors[i + 1].transform.position - _spline.Anchors[i].transform.position).normalized;
                    indicator.transform.rotation = Quaternion.LookRotation(direction);
                }
                else
                {
                    // Make the last indicator point towards the first anchor if needed
                    Vector3 direction = (_spline.Anchors[0].transform.position - _spline.Anchors[i].transform.position).normalized;
                    indicator.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }
}
