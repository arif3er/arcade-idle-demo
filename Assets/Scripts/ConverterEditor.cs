using UnityEditor;

[CustomEditor(typeof(Converter))]
public class ConverterEditor : Editor
{
    #region SerializedProperties
    SerializedProperty endProductList;
    SerializedProperty workers;
    SerializedProperty consumeWayPoint;
    SerializedProperty convertWayPoint;

    SerializedProperty endProduct;
    SerializedProperty spawnPoint;
    SerializedProperty spawnPoints;

    SerializedProperty sourceText1;
    SerializedProperty sourceText2;
    SerializedProperty sourceText3;

    SerializedProperty sourceNeed;
    SerializedProperty sourceType1;
    SerializedProperty sourceType2;
    SerializedProperty sourceType3;

    SerializedProperty currentSource1;
    SerializedProperty currentSource2;
    SerializedProperty currentSource3;

    SerializedProperty collectorList;

    SerializedProperty m_collider;

    SerializedProperty convertRate;
    SerializedProperty consumeRate;

    SerializedProperty capacity;
    SerializedProperty stackLimit;
    SerializedProperty paddingY;
    SerializedProperty paddingX;
    SerializedProperty paddingZ;

    bool quality,positioning, sourceSetup, waypointSetup = false;
    #endregion

    private void OnEnable()
    {
        endProductList = serializedObject.FindProperty("endProductList");
        workers = serializedObject.FindProperty("workers");
        consumeWayPoint = serializedObject.FindProperty("consumeWayPoint");
        convertWayPoint = serializedObject.FindProperty("convertWayPoint");

        spawnPoint = serializedObject.FindProperty("spawnPoint");
        spawnPoints = serializedObject.FindProperty("spawnPoints");
        m_collider = serializedObject.FindProperty("m_collider");

        sourceText1 = serializedObject.FindProperty("sourceText1");
        sourceText2 = serializedObject.FindProperty("sourceText2");
        sourceText3 = serializedObject.FindProperty("sourceText3");

        endProduct = serializedObject.FindProperty("endProduct");

        sourceNeed = serializedObject.FindProperty("sourceNeed");
        sourceType1 = serializedObject.FindProperty("sourceType1");
        sourceType2 = serializedObject.FindProperty("sourceType2");
        sourceType3 = serializedObject.FindProperty("sourceType3");

        currentSource1 = serializedObject.FindProperty("currentSource1");
        currentSource2 = serializedObject.FindProperty("currentSource2");
        currentSource3 = serializedObject.FindProperty("currentSource3");

        collectorList = serializedObject.FindProperty("collectorList");

        convertRate = serializedObject.FindProperty("convertRate");
        consumeRate = serializedObject.FindProperty("consumeRate");

        capacity = serializedObject.FindProperty("capacity");
        stackLimit = serializedObject.FindProperty("stackLimit");
        paddingY = serializedObject.FindProperty("paddingY");
        paddingX = serializedObject.FindProperty("paddingX");
        paddingZ = serializedObject.FindProperty("paddingZ");
    }

    public override void OnInspectorGUI()
    {
        Converter _converter = (Converter)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(endProduct);
        EditorGUILayout.Space(1);
        EditorGUILayout.PropertyField(endProductList);
        EditorGUILayout.Space(1);

        quality = EditorGUILayout.BeginFoldoutHeaderGroup(quality, "Quality");
        if (quality)
        {
            EditorGUILayout.PropertyField(capacity);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(convertRate);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(consumeRate);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(3);

        sourceSetup = EditorGUILayout.BeginFoldoutHeaderGroup(sourceSetup, "Sources");
        if (sourceSetup)
        {
            EditorGUILayout.PropertyField(sourceNeed);
            EditorGUILayout.Space(3);

            if (_converter.sourceNeed == Converter.SourceNeed.One)
            {
                EditorGUILayout.PropertyField(sourceType1);
                EditorGUILayout.PropertyField(sourceText1);
            }

            if (_converter.sourceNeed == Converter.SourceNeed.Two)
            {
                EditorGUILayout.PropertyField(sourceType1);
                EditorGUILayout.PropertyField(sourceText1);
                EditorGUILayout.Space(1);
                EditorGUILayout.PropertyField(sourceType2);
                EditorGUILayout.PropertyField(sourceText2);
            }

            if (_converter.sourceNeed == Converter.SourceNeed.Three)
            {
                EditorGUILayout.PropertyField(sourceType1);
                EditorGUILayout.PropertyField(sourceText1);
                EditorGUILayout.Space(1);
                EditorGUILayout.PropertyField(sourceType2);
                EditorGUILayout.PropertyField(sourceText2);
                EditorGUILayout.Space(1);
                EditorGUILayout.PropertyField(sourceType3);
                EditorGUILayout.PropertyField(sourceText3);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(5);

        positioning = EditorGUILayout.BeginFoldoutHeaderGroup(positioning, "Positioning");
        if (positioning)
        {
            EditorGUILayout.PropertyField(spawnPoint);
            EditorGUILayout.Space(1);

            EditorGUILayout.PropertyField(stackLimit);

            EditorGUILayout.PropertyField(paddingX);
            EditorGUILayout.PropertyField(paddingY);
            EditorGUILayout.PropertyField(paddingZ);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(5);

        waypointSetup = EditorGUILayout.BeginFoldoutHeaderGroup(waypointSetup, "Waypoints");
        if (waypointSetup)
        {
            EditorGUILayout.PropertyField(consumeWayPoint);
            EditorGUILayout.PropertyField(convertWayPoint);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(workers);
        EditorGUILayout.Space(1);
        EditorGUILayout.PropertyField(spawnPoints);


        serializedObject.ApplyModifiedProperties();
    }
}
