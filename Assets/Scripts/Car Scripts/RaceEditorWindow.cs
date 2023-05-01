using UnityEngine;
using UnityEditor;
using Pixelplacement;

public class RaceEditorWindow : EditorWindow
{
    private Spline _spline;
    private Vector2 _scrollPos;
    private bool _anchorPlacementMode;
    public GameObject IndicatorPrefab;
    private GameObject CoinPrefab;
    private int CoinFrequency = 10;

    [MenuItem("Window/Race Editor")]
    public static void ShowWindow()
    {
        GetWindow<RaceEditorWindow>("Race Editor");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Race Editor", EditorStyles.boldLabel);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        _spline = EditorGUILayout.ObjectField("Spline Object", _spline, typeof(Spline), true) as Spline;
        IndicatorPrefab = EditorGUILayout.ObjectField("Indicator Prefab", IndicatorPrefab, typeof(GameObject), true) as GameObject;
        CoinPrefab = EditorGUILayout.ObjectField("Coin Prefab", CoinPrefab, typeof(GameObject), true) as GameObject;
        CoinFrequency = EditorGUILayout.IntField("Coin Frequency", CoinFrequency);

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
            if (GUILayout.Button("Place Coins Along Spline"))
            {
                PlaceCoins();
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

        // Draw coin previews
        DrawCoinPreviews();
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

    private void PlaceCoins()
    {
        if (_spline == null || CoinPrefab == null || CoinFrequency <= 0)
        {
            return;
        }

        float stepSize = 1f / CoinFrequency;

        for (float t = 0; t <= 1; t += stepSize)
        {
            Vector3 coinPosition = _spline.GetPosition(t);
            Quaternion coinRotation = Quaternion.Euler(90, 0, 0);

            GameObject coin = Instantiate(CoinPrefab, coinPosition, coinRotation);
            coin.transform.SetParent(_spline.transform);
        }
    }

    private void DrawCoinPreviews()
    {
        if (_spline == null || CoinPrefab == null || CoinFrequency <= 0)
        {
            return;
        }

        float stepSize = 1f / CoinFrequency;

        for (float t = 0; t <= 1; t += stepSize)
        {
            Vector3 coinPosition = _spline.GetPosition(t);
            Quaternion coinRotation = Quaternion.identity;

            // Draw the semi-transparent Gizmo sphere
            Handles.color = new Color(1, 1, 1, 0.5f);
            Handles.SphereHandleCap(0, coinPosition, coinRotation, 0.5f, EventType.Repaint);
            Handles.color = Color.green;
        }
    }


}
