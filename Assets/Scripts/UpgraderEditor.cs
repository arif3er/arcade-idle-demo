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
    SerializedProperty playerCollectRateButtonAds;

    SerializedProperty playerCapacityButton;
    SerializedProperty playerCapacityButtonAds;

    SerializedProperty playerSpeedButton;
    SerializedProperty playerSpeedButtonAds;


    // Workers
    SerializedProperty workerToSpawnList;

    SerializedProperty workerSpawnPrice;
    SerializedProperty workerCollectRatePrice;
    SerializedProperty workerCapacityPrice;
    SerializedProperty workerSpeedPrice;

    SerializedProperty workerCollectRateButton;
    SerializedProperty workerCollectRateButtonAds;

    SerializedProperty workerCapacityButton;
    SerializedProperty workerCapacityButtonAds;

    SerializedProperty workerSpawnButton;
    SerializedProperty workerSpawnButtonAds;

    SerializedProperty workerSpeedButton;
    SerializedProperty workerSpeedButtonAds;


    // Generators
    SerializedProperty generatorList;

    SerializedProperty generatorRatePrice;
    SerializedProperty generatorCapacityPrice;

    SerializedProperty generatorRateButton;
    SerializedProperty generatorRateButtonAds;

    SerializedProperty generatorCapacityButton;
    SerializedProperty generatorCapacityButtonAds;


    // Converters
    SerializedProperty converterList;

    SerializedProperty converterCapacityPrice;
    SerializedProperty converterConvertRatePrice;
    SerializedProperty converterConsumeRatePrice;

    SerializedProperty converterConsumeRateButton;
    SerializedProperty converterConsumeRateButtonAds;

    SerializedProperty converterCapacityButton;
    SerializedProperty converterCapacityButtonAds;

    SerializedProperty converterConvertRateButton;
    SerializedProperty converterConvertRateButtonAds;

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
        playerCollectRateButtonAds = serializedObject.FindProperty("playerCollectRateButtonAds");

        playerCapacityButton = serializedObject.FindProperty("playerCapacityButton");
        playerCapacityButtonAds = serializedObject.FindProperty("playerCapacityButtonAds");

        playerSpeedButton = serializedObject.FindProperty("playerSpeedButton");
        playerSpeedButtonAds = serializedObject.FindProperty("playerSpeedButtonAds");


        // Worker
        workerToSpawnList = serializedObject.FindProperty("workerToSpawnList");

        workerSpawnPrice = serializedObject.FindProperty("workerSpawnPrice");
        workerCollectRatePrice = serializedObject.FindProperty("workerCollectRatePrice");
        workerCapacityPrice = serializedObject.FindProperty("workerCapacityPrice");
        workerSpeedPrice = serializedObject.FindProperty("workerSpeedPrice");

        workerCollectRateButton = serializedObject.FindProperty("workerCollectRateButton");
        workerCollectRateButtonAds = serializedObject.FindProperty("workerCollectRateButtonAds");

        workerCapacityButton = serializedObject.FindProperty("workerCapacityButton");
        workerCapacityButtonAds = serializedObject.FindProperty("workerCapacityButtonAds");

        workerSpawnButton = serializedObject.FindProperty("workerSpawnButton");
        workerSpawnButtonAds = serializedObject.FindProperty("workerSpawnButtonAds");

        workerSpeedButton = serializedObject.FindProperty("workerSpeedButton");
        workerSpeedButtonAds = serializedObject.FindProperty("workerSpeedButtonAds");


        // Generator
        generatorList = serializedObject.FindProperty("generatorList");

        generatorRatePrice = serializedObject.FindProperty("generatorRatePrice");
        generatorCapacityPrice = serializedObject.FindProperty("generatorCapacityPrice");

        generatorRateButton = serializedObject.FindProperty("generatorRateButton");
        generatorRateButtonAds = serializedObject.FindProperty("generatorRateButtonAds");

        generatorCapacityButton = serializedObject.FindProperty("generatorCapacityButton");
        generatorCapacityButtonAds = serializedObject.FindProperty("generatorCapacityButtonAds");


        // Converter
        converterList = serializedObject.FindProperty("converterList");

        converterCapacityPrice = serializedObject.FindProperty("converterCapacityPrice");
        converterConvertRatePrice = serializedObject.FindProperty("converterConvertRatePrice");
        converterConsumeRatePrice = serializedObject.FindProperty("converterConsumeRatePrice");

        converterConsumeRateButton = serializedObject.FindProperty("converterConsumeRateButton");
        converterConsumeRateButtonAds = serializedObject.FindProperty("converterConsumeRateButtonAds");

        converterCapacityButton = serializedObject.FindProperty("converterCapacityButton");
        converterCapacityButtonAds = serializedObject.FindProperty("converterCapacityButtonAds");

        converterConvertRateButton = serializedObject.FindProperty("converterConvertRateButton");
        converterConvertRateButtonAds = serializedObject.FindProperty("converterConvertRateButtonAds");

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
            EditorGUILayout.PropertyField(playerCollectRateButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(playerCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerCapacityButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(playerSpeedButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(playerSpeedButtonAds);
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
            EditorGUILayout.PropertyField(workerSpawnButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(workerCollectRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCollectRateButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(workerCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerCapacityButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(workerSpeedButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(workerSpeedButtonAds);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(workerToSpawnList);

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
            EditorGUILayout.PropertyField(generatorRateButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(generatorCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(generatorCapacityButtonAds);
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
            EditorGUILayout.PropertyField(converterConsumeRateButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(converterCapacityButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterCapacityButtonAds);
            EditorGUILayout.Space(3);

            EditorGUILayout.PropertyField(converterConvertRateButton);
            EditorGUILayout.Space(1);
            EditorGUILayout.PropertyField(converterConvertRateButtonAds);
            EditorGUILayout.Space(1);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(converterList);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif