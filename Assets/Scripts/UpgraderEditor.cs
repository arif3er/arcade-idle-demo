using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Upgrader))]
public class UpgraderEditor : Editor
{
    #region Serialized Properties

    // Colliding
    SerializedProperty collideRange;
    SerializedProperty playerUpgradeShop;
    SerializedProperty workerUpgradeShop;
    SerializedProperty generatorUpgradeShop;
    SerializedProperty converterUpgradeShop;

    // Panels
    SerializedProperty playerPanel;
    SerializedProperty workerPanel;
    SerializedProperty generatorPanel;
    SerializedProperty converterPanel;

    // Effects
    SerializedProperty moneyNeedWarnEffect;
    SerializedProperty successfulBuyEffect;

    // Player
    SerializedProperty player;
    SerializedProperty playerCollectRatePrice;
    SerializedProperty playerCapacityPrice;
    SerializedProperty playerSpeedPrice;

    SerializedProperty playerCollectRateButton;
    SerializedProperty playerCapacityButton;
    SerializedProperty playerSpeedButton;

    // Workers
    SerializedProperty workerList;

    SerializedProperty workerSpawnPrice;
    SerializedProperty workerCollectRatePrice;
    SerializedProperty workerCapacityPrice;
    SerializedProperty workerSpeedPrice;

    SerializedProperty workerCollectRateButton;
    SerializedProperty workerCapacityButton;
    SerializedProperty workerSpawnButton;
    SerializedProperty workerSpeedButton;

    // Generators
    SerializedProperty generatorList;

    SerializedProperty generatorRatePrice;
    SerializedProperty generatorCapacityPrice;

    SerializedProperty generatorRateButton;
    SerializedProperty generatorCapacityButton;

    // Converters
    SerializedProperty converterList;

    SerializedProperty converterCapacityPrice;
    SerializedProperty converterConvertRatePrice;
    SerializedProperty converterConsumeRatePrice;

    SerializedProperty converterConsumeRateButton;
    SerializedProperty converterCapacityButton;
    SerializedProperty converterConvertRateButton;

    bool playerBool, workerBool, generatorBool, converterBool, effectBool = false;
    #endregion

    private void OnEnable()
    {
        // Colliding
        collideRange = serializedObject.FindProperty("collideRange");
        playerUpgradeShop = serializedObject.FindProperty("playerUpgradeShop");
        workerUpgradeShop = serializedObject.FindProperty("workerUpgradeShop");
        generatorUpgradeShop = serializedObject.FindProperty("generatorUpgradeShop");
        converterUpgradeShop = serializedObject.FindProperty("converterUpgradeShop");

        // Panels
        playerPanel = serializedObject.FindProperty("playerPanel");
        workerPanel = serializedObject.FindProperty("workerPanel");
        generatorPanel = serializedObject.FindProperty("generatorPanel");
        converterPanel = serializedObject.FindProperty("converterPanel");


        // Effects
        moneyNeedWarnEffect = serializedObject.FindProperty("moneyNeedWarnEffect");
        successfulBuyEffect = serializedObject.FindProperty("successfulBuyEffect");

        // Player
        player = serializedObject.FindProperty("player");
        playerCollectRatePrice = serializedObject.FindProperty("playerCollectRatePrice");
        playerCapacityPrice = serializedObject.FindProperty("playerCapacityPrice");
        playerSpeedPrice = serializedObject.FindProperty("playerSpeedPrice");

        playerCollectRateButton = serializedObject.FindProperty("playerCollectRateButton");
        playerCapacityButton = serializedObject.FindProperty("playerCapacityButton");
        playerSpeedButton = serializedObject.FindProperty("playerSpeedButton");

        // Worker
        workerList = serializedObject.FindProperty("workerList");

        workerSpawnPrice = serializedObject.FindProperty("workerSpawnPrice");
        workerCollectRatePrice = serializedObject.FindProperty("workerCollectRatePrice");
        workerCapacityPrice = serializedObject.FindProperty("workerCapacityPrice");
        workerSpeedPrice = serializedObject.FindProperty("workerSpeedPrice");

        workerCollectRateButton = serializedObject.FindProperty("workerCollectRateButton");
        workerCapacityButton = serializedObject.FindProperty("workerCapacityButton");
        workerSpawnButton = serializedObject.FindProperty("workerSpawnButton");
        workerSpeedButton = serializedObject.FindProperty("workerSpeedButton");

        // Generator
        generatorList = serializedObject.FindProperty("generatorList");

        generatorRatePrice = serializedObject.FindProperty("generatorRatePrice");
        generatorCapacityPrice = serializedObject.FindProperty("generatorCapacityPrice");

        generatorRateButton = serializedObject.FindProperty("generatorRateButton");
        generatorCapacityButton = serializedObject.FindProperty("generatorCapacityButton");

        // Converter
        converterList = serializedObject.FindProperty("converterList");

        converterCapacityPrice = serializedObject.FindProperty("converterCapacityPrice");
        converterConvertRatePrice = serializedObject.FindProperty("converterConvertRatePrice");
        converterConsumeRatePrice = serializedObject.FindProperty("converterConsumeRatePrice");

        converterConsumeRateButton = serializedObject.FindProperty("converterConsumeRateButton");
        converterCapacityButton = serializedObject.FindProperty("converterCapacityButton");
        converterConvertRateButton = serializedObject.FindProperty("converterConvertRateButton");
    }

    public override void OnInspectorGUI()
    {
        Upgrader _upgrade = (Upgrader)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(collideRange);

        effectBool = EditorGUILayout.BeginFoldoutHeaderGroup(effectBool, "Feedback Effects");
        if (effectBool)
        {
            EditorGUILayout.PropertyField(moneyNeedWarnEffect);
            EditorGUILayout.PropertyField(successfulBuyEffect);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        playerBool = EditorGUILayout.BeginFoldoutHeaderGroup(playerBool, "Player");
        if (playerBool)
        {
            EditorGUILayout.PropertyField(playerUpgradeShop);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerPanel);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(player);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerCollectRatePrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerCapacityPrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerSpeedPrice);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(playerCollectRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerSpeedButton);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        workerBool = EditorGUILayout.BeginFoldoutHeaderGroup(workerBool, "Worker");
        if (workerBool)
        {
            EditorGUILayout.PropertyField(workerUpgradeShop);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerPanel);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerSpawnPrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCollectRatePrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCapacityPrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerSpeedPrice);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(workerSpawnButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCollectRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerSpeedButton);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(workerList);

        generatorBool = EditorGUILayout.BeginFoldoutHeaderGroup(generatorBool, "Generator");
        if (generatorBool)
        {
            EditorGUILayout.PropertyField(generatorUpgradeShop);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(generatorPanel);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(generatorRatePrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(generatorCapacityPrice);
            EditorGUILayout.Space(1);

            EditorGUILayout.PropertyField(generatorRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(generatorCapacityButton);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(generatorList);

        converterBool = EditorGUILayout.BeginFoldoutHeaderGroup(converterBool, "Converter");
        if (converterBool)
        {
            EditorGUILayout.PropertyField(converterUpgradeShop);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterPanel);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterCapacityPrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterConvertRatePrice);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterConsumeRatePrice);
            EditorGUILayout.Space(1);

            EditorGUILayout.PropertyField(converterConsumeRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterConvertRateButton);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(converterList);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif