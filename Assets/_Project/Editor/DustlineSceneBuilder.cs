#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DustlineSceneBuilder
{
    const string Root = "Assets/_Project";
    const string ScenePath = Root + "/Scenes/Dustline_Prototype.unity";
    const string MaterialsPath = Root + "/Materials";
    const string PrefabsPath = Root + "/Prefabs";

    [MenuItem("DUSTLINE/Build or Rebuild Prototype Scene")]
    public static void BuildScene()
    {
        EnsureFolders();

        Material floorMat = GetOrCreateMaterial("Floor", new Color(.24f, .22f, .18f));
        Material wallMat = GetOrCreateMaterial("Wall", new Color(.34f, .32f, .28f));
        Material metalMat = GetOrCreateMaterial("Metal", new Color(.20f, .22f, .23f));
        Material warningMat = GetOrCreateMaterial("Warning", new Color(.70f, .42f, .08f));
        Material activeMat = GetOrCreateMaterial("Active", new Color(.12f, .65f, .24f));
        Material batteryMat = GetOrCreateMaterial("Battery", new Color(.78f, .62f, .10f));
        Material playerMat = GetOrCreateMaterial("Player", new Color(.18f, .45f, .68f));
        Material finishMat = GetOrCreateMaterial("Finish", new Color(.10f, .65f, .70f));

        GameObject batteryPrefab = CreateBatteryPrefab(batteryMat);
        InputActionAsset inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
        if (inputActions == null)
        {
            Debug.LogError("DUSTLINE: no se encontró Assets/InputSystem_Actions.inputactions.");
            return;
        }

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject systems = new GameObject("00_Systems");
        DustlineProgress progress = systems.AddComponent<DustlineProgress>();

        CreateLighting();
        CreateEnvironment(floorMat, wallMat, metalMat);
        CreatePhysicsProps(metalMat);

        Camera camera = CreateCamera();
        GameObject player = CreatePlayer(camera, inputActions, playerMat, progress);
        ConfigureCameraFollow(camera, player.transform);

        GarageDoor garageDoor = CreateGarageDoor(metalMat, warningMat);
        VehicleInteractable vehicle = CreateVehicle(player.GetComponent<PlayerCargo>(), progress, metalMat, activeMat);
        BatterySpawner spawner = CreateGeneratorArea(batteryPrefab, activeMat, warningMat, progress);
        GeneratorInteractable generator = spawner.GetComponentInParent<GeneratorInteractable>();

        SetObjectReference(generator, "spawner", spawner);
        SetObjectReference(generator, "progress", progress);
        SetObjectReference(vehicle, "garageDoor", garageDoor);

        CreateFinishZone(progress, finishMat);
        CreateHUD(player.GetComponent<DustlinePlayerInteraction>());

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuildSettings(ScenePath);
        Selection.activeGameObject = player;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("DUSTLINE: escena creada en " + ScenePath);
    }

    static void EnsureFolders()
    {
        EnsureFolder("Assets", "_Project");
        EnsureFolder(Root, "Scenes");
        EnsureFolder(Root, "Materials");
        EnsureFolder(Root, "Prefabs");
        EnsureFolder(Root, "Scripts");
        EnsureFolder(Root, "Editor");
    }

    static void EnsureFolder(string parent, string name)
    {
        string path = parent + "/" + name;
        if (!AssetDatabase.IsValidFolder(path)) AssetDatabase.CreateFolder(parent, name);
    }

    static Material GetOrCreateMaterial(string name, Color color)
    {
        string path = MaterialsPath + "/" + name + ".mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        mat = new Material(shader) { name = name, color = color };
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static GameObject CreateBatteryPrefab(Material material)
    {
        string path = PrefabsPath + "/Battery.prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        temp.name = "Battery";
        temp.transform.localScale = new Vector3(.7f, .4f, .45f);
        temp.GetComponent<Renderer>().sharedMaterial = material;
        Rigidbody rb = temp.AddComponent<Rigidbody>();
        rb.mass = 3f;
        temp.AddComponent<BatteryPickup>();
        temp.AddComponent<CollisionFeedback>();
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(temp, path);
        Object.DestroyImmediate(temp);
        return prefab;
    }

    static void CreateLighting()
    {
        GameObject lightGO = new GameObject("Directional Light");
        Light light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightGO.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
    }

    static void CreateEnvironment(Material floor, Material wall, Material metal)
    {
        GameObject root = new GameObject("01_Environment");
        CreateCube("Floor", new Vector3(0, -.25f, 4), new Vector3(22, .5f, 30), floor, root.transform);
        CreateCube("BackWall", new Vector3(0, 2.5f, 18.5f), new Vector3(22, 5.5f, .5f), wall, root.transform);
        CreateCube("LeftWall", new Vector3(-10.75f, 2.5f, 4), new Vector3(.5f, 5.5f, 30), wall, root.transform);
        CreateCube("RightWall", new Vector3(10.75f, 2.5f, 4), new Vector3(.5f, 5.5f, 30), wall, root.transform);
        CreateCube("WorkshopDivider", new Vector3(3.5f, 1.5f, 7.5f), new Vector3(.35f, 3f, 10f), wall, root.transform);
        CreateCube("StorageShelf_A", new Vector3(-6.5f, .9f, 7), new Vector3(3f, 1.8f, .7f), metal, root.transform);
        CreateCube("StorageShelf_B", new Vector3(-6.5f, .9f, 10), new Vector3(3f, 1.8f, .7f), metal, root.transform);
        CreateCube("ServiceDesk", new Vector3(-2f, .65f, 13f), new Vector3(4f, 1.3f, 1f), metal, root.transform);
    }

    static void CreatePhysicsProps(Material material)
    {
        GameObject root = new GameObject("02_PhysicsProps");
        for (int i = 0; i < 3; i++)
        {
            GameObject crate = CreateCube("PhysicsCrate_" + (i + 1), new Vector3(-7f + i * 1.25f, .6f, 3f), Vector3.one, material, root.transform);
            crate.AddComponent<Rigidbody>();
            crate.AddComponent<CollisionFeedback>();
        }
    }

    static Camera CreateCamera()
    {
        GameObject go = new GameObject("Main Camera");
        go.tag = "MainCamera";
        Camera cam = go.AddComponent<Camera>();
        go.AddComponent<AudioListener>();
        go.AddComponent<ThirdPersonCamera>();
        return cam;
    }

    static GameObject CreatePlayer(Camera camera, InputActionAsset inputActions, Material playerMaterial, DustlineProgress progress)
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.layer = LayerMask.NameToLayer("Ignore Raycast");
        player.transform.position = new Vector3(0, 1f, -8f);
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
        player.GetComponent<Renderer>().sharedMaterial = playerMaterial;

        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = .45f;
        cc.center = Vector3.zero;

        PlayerMovement movement = player.AddComponent<PlayerMovement>();
        PlayerCargo cargo = player.AddComponent<PlayerCargo>();
        DustlinePlayerInteraction interaction = player.AddComponent<DustlinePlayerInteraction>();

        SetObjectReference(movement, "inputActions", inputActions);
        SetObjectReference(movement, "cameraTransform", camera.transform);
        SetObjectReference(interaction, "view", camera);
        SetObjectReference(interaction, "inputActions", inputActions);
        SetObjectReference(interaction, "progress", progress);
        return player;
    }

    static void ConfigureCameraFollow(Camera camera, Transform player)
    {
        ThirdPersonCamera controller = camera.GetComponent<ThirdPersonCamera>();
        SetObjectReference(controller, "target", player);
        SerializedObject so = new SerializedObject(controller);
        so.FindProperty("distance").floatValue = 5.5f;
        so.FindProperty("height").floatValue = 1.25f;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static BatterySpawner CreateGeneratorArea(GameObject batteryPrefab, Material active, Material inactive, DustlineProgress progress)
    {
        GameObject root = new GameObject("03_GeneratorArea");
        root.transform.position = new Vector3(-7f, 0f, 14f);
        CreateCube("Generator", new Vector3(0, 1f, 0), new Vector3(2f, 2f, 1.2f), inactive, root.transform);
        GeneratorInteractable generator = root.AddComponent<GeneratorInteractable>();
        SetObjectReference(generator, "activeMaterial", active);

        GameObject spawnPoint = new GameObject("BatterySpawnPoint");
        spawnPoint.transform.SetParent(root.transform);
        spawnPoint.transform.localPosition = new Vector3(2.2f, 1.2f, 0f);

        BatterySpawner spawner = root.AddComponent<BatterySpawner>();
        SetObjectReference(spawner, "batteryPrefab", batteryPrefab);
        SetObjectReference(spawner, "spawnPoint", spawnPoint.transform);
        return spawner;
    }

    static VehicleInteractable CreateVehicle(PlayerCargo cargo, DustlineProgress progress, Material baseMat, Material recoveredMat)
    {
        GameObject root = new GameObject("04_Vehicle");
        root.transform.position = new Vector3(6.3f, 0f, 12f);
        BoxCollider rootCollider = root.AddComponent<BoxCollider>();
        rootCollider.center = new Vector3(0, .8f, 0);
        rootCollider.size = new Vector3(3.4f, 1.6f, 6f);

        CreateCube("Body", new Vector3(0, .8f, 0), new Vector3(3.2f, 1f, 5.5f), baseMat, root.transform, false);
        CreateCube("Cabin", new Vector3(0, 1.55f, -.4f), new Vector3(2.5f, .8f, 2.5f), baseMat, root.transform, false);
        for (int i = 0; i < 4; i++)
        {
            float x = i < 2 ? -1.55f : 1.55f;
            float z = i % 2 == 0 ? -1.6f : 1.6f;
            GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheel.name = "Wheel_" + (i + 1);
            wheel.transform.SetParent(root.transform);
            wheel.transform.localPosition = new Vector3(x, .45f, z);
            wheel.transform.localRotation = Quaternion.Euler(0, 0, 90);
            wheel.transform.localScale = new Vector3(.7f, .3f, .7f);
            wheel.GetComponent<Renderer>().sharedMaterial = baseMat;
            Object.DestroyImmediate(wheel.GetComponent<Collider>());
        }

        VehicleInteractable vehicle = root.AddComponent<VehicleInteractable>();
        SetObjectReference(vehicle, "cargo", cargo);
        SetObjectReference(vehicle, "progress", progress);
        SetObjectReference(vehicle, "recoveredMaterial", recoveredMat);
        return vehicle;
    }

    static GarageDoor CreateGarageDoor(Material metal, Material warning)
    {
        GameObject root = new GameObject("05_GarageExit");
        root.transform.position = new Vector3(0f, 0f, 18f);
        CreateCube("DoorFrameLeft", new Vector3(-4.2f, 2.5f, 0), new Vector3(1f, 5f, .6f), metal, root.transform);
        CreateCube("DoorFrameRight", new Vector3(4.2f, 2.5f, 0), new Vector3(1f, 5f, .6f), metal, root.transform);
        GameObject door = CreateCube("GarageDoor", new Vector3(0, 2.2f, 0), new Vector3(7.4f, 4.4f, .45f), warning, root.transform);
        return door.AddComponent<GarageDoor>();
    }

    static void CreateFinishZone(DustlineProgress progress, Material material)
    {
        GameObject zone = CreateCube("06_FinishTrigger", new Vector3(0, .15f, 21f), new Vector3(7f, .3f, 3f), material, null);
        BoxCollider collider = zone.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        Rigidbody rb = zone.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        DustlineFinishTrigger finish = zone.AddComponent<DustlineFinishTrigger>();
        SetObjectReference(finish, "progress", progress);
    }

    static void CreateHUD(DustlinePlayerInteraction interaction)
    {
        GameObject canvasGO = new GameObject("HUD");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        TMP_Text controls = CreateText("Controls", canvas.transform, "WASD mover | Mouse cámara | E interactuar | Esc liberar cursor", 26, TextAlignmentOptions.TopLeft);
        RectTransform cr = controls.rectTransform;
        cr.anchorMin = new Vector2(0, 1); cr.anchorMax = new Vector2(0, 1); cr.pivot = new Vector2(0, 1);
        cr.anchoredPosition = new Vector2(30, -25); cr.sizeDelta = new Vector2(1000, 80);

        TMP_Text prompt = CreateText("Prompt", canvas.transform, "", 30, TextAlignmentOptions.Bottom);
        RectTransform pr = prompt.rectTransform;
        pr.anchorMin = new Vector2(.5f, 0); pr.anchorMax = new Vector2(.5f, 0); pr.pivot = new Vector2(.5f, 0);
        pr.anchoredPosition = new Vector2(0, 45); pr.sizeDelta = new Vector2(1300, 100);

        TMP_Text crosshair = CreateText("Crosshair", canvas.transform, "+", 32, TextAlignmentOptions.Center);
        RectTransform xr = crosshair.rectTransform;
        xr.anchorMin = xr.anchorMax = new Vector2(.5f, .5f); xr.pivot = new Vector2(.5f, .5f);
        xr.anchoredPosition = Vector2.zero; xr.sizeDelta = new Vector2(60, 60);

        SetObjectReference(interaction, "prompt", prompt);
    }

    static TMP_Text CreateText(string name, Transform parent, string value, float size, TextAlignmentOptions alignment)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.alignment = alignment;
        text.raycastTarget = false;
        return text;
    }

    static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, Transform parent, bool keepCollider = true)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        if (parent != null) go.transform.SetParent(parent);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        if (!keepCollider) Object.DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }

    static void SetObjectReference(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError($"DUSTLINE: no se encontró el campo serializado {field} en {target.GetType().Name}.");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void AddSceneToBuildSettings(string path)
    {
        var old = EditorBuildSettings.scenes;
        var updated = new System.Collections.Generic.List<EditorBuildSettingsScene>
        {
            new EditorBuildSettingsScene(path, true)
        };

        foreach (var scene in old)
        {
            if (scene.path == path) continue;
            updated.Add(new EditorBuildSettingsScene(scene.path, false));
        }

        EditorBuildSettings.scenes = updated.ToArray();
    }
}
#endif
