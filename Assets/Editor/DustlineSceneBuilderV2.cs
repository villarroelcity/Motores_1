#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[InitializeOnLoad]
public static class DustlineSceneBuilderV2
{
    const string ScenePath = "Assets/Scenes/Dustline_Prototype.unity";
    const string RootMarker = "DUSTLINE_LEVEL_V2";
    const string MaterialFolder = "Assets/Materials";
    const string PrefabFolder = "Assets/Prefabs";

    static readonly Dictionary<string, Material> mats = new();

    static DustlineSceneBuilderV2()
    {
        EditorApplication.delayCall += TryAutoBuild;
    }

    [MenuItem("DUSTLINE/Rebuild Estación 01 (V2)")]
    public static void BuildFromMenu() => BuildScene(true);

    static void TryAutoBuild()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (SessionState.GetBool("DUSTLINE_V2_AUTOBUILD_CHECKED", false)) return;
        SessionState.SetBool("DUSTLINE_V2_AUTOBUILD_CHECKED", true);

        bool needsBuild = !File.Exists(ScenePath);
        if (!needsBuild)
        {
            try
            {
                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                needsBuild = !scene.GetRootGameObjects().Any(go => go.name == RootMarker);
            }
            catch { needsBuild = true; }
        }

        if (needsBuild)
            BuildScene(false);
        else
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
    }

    static void BuildScene(bool showDialog)
    {
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory(MaterialFolder);
        Directory.CreateDirectory(PrefabFolder);

        CreateMaterials();
        GameObject batteryPrefab = CreateBatteryPrefab();

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject marker = new GameObject(RootMarker);

        // ------------------------------------------------------------------
        // Global atmosphere
        // ------------------------------------------------------------------
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(.43f, .39f, .32f);
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 42f;
        RenderSettings.fogEndDistance = 115f;
        RenderSettings.ambientIntensity = .72f;
        RenderSettings.ambientSkyColor = new Color(.42f, .45f, .48f);
        RenderSettings.ambientEquatorColor = new Color(.24f, .22f, .19f);
        RenderSettings.ambientGroundColor = new Color(.12f, .105f, .085f);

        GameObject sun = new GameObject("Sun_Dustline");
        sun.transform.SetParent(marker.transform);
        sun.transform.rotation = Quaternion.Euler(42f, -32f, 0f);
        Light sunLight = sun.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.color = new Color(1f, .80f, .60f);
        sunLight.intensity = 1.25f;
        sunLight.shadows = LightShadows.Soft;

        // ------------------------------------------------------------------
        // Systems
        // ------------------------------------------------------------------
        GameObject systems = new GameObject("00_SYSTEMS");
        systems.transform.SetParent(marker.transform);
        DustlineProgress progress = systems.AddComponent<DustlineProgress>();
        WorkshopPowerSystem powerSystem = systems.AddComponent<WorkshopPowerSystem>();

        // ------------------------------------------------------------------
        // Environment roots
        // ------------------------------------------------------------------
        Transform exterior = NewRoot("01_EXTERIOR_FORECOURT", marker.transform);
        Transform workshop = NewRoot("02_WORKSHOP", marker.transform);
        Transform generatorYard = NewRoot("03_GENERATOR_YARD", marker.transform);
        Transform partsRoom = NewRoot("04_PARTS_STORAGE", marker.transform);
        Transform props = NewRoot("05_PHYSICS_PROPS", marker.transform);
        Transform gameplay = NewRoot("06_GAMEPLAY", marker.transform);
        Transform route = NewRoot("07_EXIT_ROUTE", marker.transform);

        // ------------------------------------------------------------------
        // Exterior approach / forecourt
        // ------------------------------------------------------------------
        Cube("ApproachRoad", exterior, new Vector3(0, -.22f, -29), new Vector3(9, .35f, 26), M("Asphalt"));
        Cube("Forecourt", exterior, new Vector3(0, -.18f, -12), new Vector3(30, .3f, 14), M("Concrete"));
        Cube("LeftDirt", exterior, new Vector3(-20, -.27f, -12), new Vector3(10, .25f, 55), M("Dust"));
        Cube("RightDirt", exterior, new Vector3(20, -.27f, -12), new Vector3(10, .25f, 55), M("Dust"));

        WorldText("DUSTLINE", exterior, new Vector3(-9.4f, 4.6f, -6.6f), Quaternion.Euler(0, 0, 0), 1.7f, new Color(1f, .62f, .25f), TextAlignmentOptions.Left);
        WorldText("SERVICE STATION // 01", exterior, new Vector3(-9.35f, 3.65f, -6.55f), Quaternion.identity, .55f, Color.white, TextAlignmentOptions.Left);
        Cube("StationSignBacking", exterior, new Vector3(-5.4f, 4.25f, -6.8f), new Vector3(9.2f, 2.7f, .25f), M("DarkMetal"), false);

        // Gas pump silhouettes / road furniture
        for (int i = 0; i < 2; i++)
        {
            float x = -7f + i * 3.2f;
            Cube($"OldPump_{i}_Body", exterior, new Vector3(x, 1f, -11f), new Vector3(1.15f, 2f, .9f), M("Rust"));
            Cube($"OldPump_{i}_Screen", exterior, new Vector3(x, 1.35f, -11.47f), new Vector3(.62f, .38f, .05f), M("DarkGlass"), false);
            Cube($"OldPump_{i}_Top", exterior, new Vector3(x, 2.1f, -11f), new Vector3(1.35f, .25f, 1.05f), M("SafetyOrange"));
        }
        CreateBarrier(exterior, new Vector3(9.8f, .55f, -12.5f), 0f);
        CreateBarrier(exterior, new Vector3(11.2f, .55f, -15.2f), 20f);

        // ------------------------------------------------------------------
        // Workshop shell: 24m x 25m, front at z=-6, route door at z=18
        // ------------------------------------------------------------------
        Cube("WorkshopFloor", workshop, new Vector3(0, -.08f, 6), new Vector3(24, .16f, 25), M("WorkshopFloor"));
        // left wall has a service opening toward the generator yard
        Cube("Wall_Left_Front", workshop, new Vector3(-12, 2.6f, -2.35f), new Vector3(.4f, 5.2f, 7.3f), M("ConcreteDark"));
        Cube("Wall_Left_Back", workshop, new Vector3(-12, 2.6f, 10.85f), new Vector3(.4f, 5.2f, 14.3f), M("ConcreteDark"));
        Cube("Wall_Left_DoorLintel", workshop, new Vector3(-12, 4.55f, 2.5f), new Vector3(.4f, 1.3f, 2.4f), M("ConcreteDark"));
        Cube("Wall_Right", workshop, new Vector3(12, 2.6f, 6), new Vector3(.4f, 5.2f, 25), M("ConcreteDark"));
        // front wall with personnel entrance
        Cube("FrontWall_Left", workshop, new Vector3(-7.5f, 2.6f, -6.4f), new Vector3(8.5f, 5.2f, .4f), M("ConcreteDark"));
        Cube("FrontWall_Right", workshop, new Vector3(5.5f, 2.6f, -6.4f), new Vector3(13f, 5.2f, .4f), M("ConcreteDark"));
        Cube("FrontLintel", workshop, new Vector3(-2.2f, 4.55f, -6.4f), new Vector3(2.2f, 1.3f, .4f), M("ConcreteDark"));
        // back wall segments around garage opening x=3, width 7
        Cube("BackWall_Left", workshop, new Vector3(-6.3f, 2.6f, 18.4f), new Vector3(11.4f, 5.2f, .4f), M("ConcreteDark"));
        Cube("BackWall_Right", workshop, new Vector3(9.6f, 2.6f, 18.4f), new Vector3(4.8f, 5.2f, .4f), M("ConcreteDark"));
        Cube("BackLintel", workshop, new Vector3(3.1f, 4.8f, 18.4f), new Vector3(7.4f, .8f, .4f), M("ConcreteDark"));
        // roof beams only, leaving open view for camera
        for (int i = 0; i < 5; i++)
            Cube($"RoofBeam_{i}", workshop, new Vector3(0, 5.05f, -3f + i * 5.1f), new Vector3(23.5f, .22f, .28f), M("DarkMetal"), false);

        // floor markings
        Cube("BayStripeLeft", workshop, new Vector3(-.6f, .015f, 7.1f), new Vector3(.12f, .025f, 12.5f), M("SafetyYellow"), false);
        Cube("BayStripeRight", workshop, new Vector3(6.8f, .015f, 7.1f), new Vector3(.12f, .025f, 12.5f), M("SafetyYellow"), false);
        for (int i = 0; i < 4; i++)
            Cube($"HazardStripe_{i}", workshop, new Vector3(-8.8f + i * .75f, .02f, 15.1f), new Vector3(.45f, .03f, 3.4f), i % 2 == 0 ? M("SafetyYellow") : M("DarkMetal"), false, Quaternion.Euler(0, 20f, 0));

        // inspection pit visual
        Cube("PitDark", workshop, new Vector3(3.1f, .01f, 7f), new Vector3(2.2f, .04f, 7.5f), M("PitBlack"), false);
        Cube("PitRailL", workshop, new Vector3(1.85f, .13f, 7f), new Vector3(.18f, .25f, 7.7f), M("SafetyYellow"));
        Cube("PitRailR", workshop, new Vector3(4.35f, .13f, 7f), new Vector3(.18f, .25f, 7.7f), M("SafetyYellow"));

        WorldText("BAY 01", workshop, new Vector3(2.95f, .035f, 12.6f), Quaternion.Euler(90, 0, 0), 1.0f, new Color(1f, .63f, .2f), TextAlignmentOptions.Center);
        WorldText("PARTS", workshop, new Vector3(8.2f, 3.9f, 10.3f), Quaternion.Euler(0, -90, 0), .8f, Color.white, TextAlignmentOptions.Center);
        WorldText("GENERATOR", workshop, new Vector3(-11.75f, 3.7f, 2.4f), Quaternion.Euler(0, 90, 0), .55f, new Color(1f, .62f, .25f), TextAlignmentOptions.Center);

        // work benches / shelves
        CreateWorkbench(workshop, new Vector3(-8.5f, 1f, 8.5f));
        CreateWorkbench(workshop, new Vector3(-8.5f, 1f, 12.2f));
        CreateShelving(workshop, new Vector3(9.7f, 1.65f, 3.2f), Quaternion.Euler(0, 90, 0));
        CreateShelving(workshop, new Vector3(9.7f, 1.65f, 7.2f), Quaternion.Euler(0, 90, 0));

        // ------------------------------------------------------------------
        // Parts room / cage
        // ------------------------------------------------------------------
        // parts cage wall split to leave a walkable doorway near z=11
        Cube("PartsWall_Front", partsRoom, new Vector3(7.4f, 1.8f, 9.75f), new Vector3(.22f, 3.6f, 1.5f), M("DarkMetal"));
        Cube("PartsWall_Back", partsRoom, new Vector3(7.4f, 1.8f, 14.85f), new Vector3(.22f, 3.6f, 4.7f), M("DarkMetal"));
        Cube("PartsDoorLintel", partsRoom, new Vector3(7.4f, 3.1f, 11.55f), new Vector3(.22f, 1.0f, 2.1f), M("DarkMetal"));
        Cube("PartsBack", partsRoom, new Vector3(9.7f, 1.8f, 17.2f), new Vector3(4.6f, 3.6f, .22f), M("DarkMetal"));
        // opening around x~9.6, z=9
        CreateShelving(partsRoom, new Vector3(9.4f, 1.65f, 15.2f), Quaternion.identity);
        CreateShelving(partsRoom, new Vector3(9.4f, 1.65f, 11.9f), Quaternion.identity);

        GameObject batterySpawnStand = Cube("BatterySpawnStand", partsRoom, new Vector3(9.6f, .45f, 14.2f), new Vector3(1.3f, .9f, 1.2f), M("DarkMetal"));
        Transform batterySpawnPoint = new GameObject("BatterySpawnPoint").transform;
        batterySpawnPoint.SetParent(partsRoom);
        batterySpawnPoint.position = new Vector3(9.6f, 1.25f, 14.2f);
        BatterySpawner spawner = batterySpawnStand.AddComponent<BatterySpawner>();
        spawner.Configure(batteryPrefab, batterySpawnPoint);
        Renderer partsStatus = Cube("PartsStatusLight", partsRoom, new Vector3(7.25f, 2.5f, 10.4f), new Vector3(.08f, .45f, .65f), M("OfflineRed"), false).GetComponent<Renderer>();

        // ------------------------------------------------------------------
        // Generator yard and generator
        // ------------------------------------------------------------------
        Cube("GeneratorPad", generatorYard, new Vector3(-15.5f, -.02f, 3f), new Vector3(6.5f, .18f, 8f), M("Concrete"));
        // fence posts
        for (int i = 0; i < 5; i++)
        {
            Cube($"FencePostN_{i}", generatorYard, new Vector3(-18.4f + i * 1.5f, 1.2f, 6.8f), new Vector3(.1f, 2.4f, .1f), M("DarkMetal"));
            Cube($"FencePostS_{i}", generatorYard, new Vector3(-18.4f + i * 1.5f, 1.2f, -.8f), new Vector3(.1f, 2.4f, .1f), M("DarkMetal"));
        }
        Cube("FenceRailN", generatorYard, new Vector3(-15.4f, 1.2f, 6.8f), new Vector3(6.2f, .08f, .08f), M("DarkMetal"));
        Cube("FenceRailS", generatorYard, new Vector3(-15.4f, 1.2f, -.8f), new Vector3(6.2f, .08f, .08f), M("DarkMetal"));

        GameObject generator = new GameObject("Generator_Interactable");
        generator.transform.SetParent(generatorYard);
        generator.transform.position = new Vector3(-15.2f, 0, 3.1f);
        GameObject genBody = Cube("GeneratorBody", generator.transform, new Vector3(0, .9f, 0), new Vector3(2.6f, 1.8f, 1.5f), M("Rust"));
        genBody.transform.localPosition = new Vector3(0, .9f, 0);
        Cube("GeneratorPanel", generator.transform, new Vector3(0, 1.1f, -.79f), new Vector3(1.2f, .7f, .08f), M("OfflineRed"), false).transform.localPosition = new Vector3(0, 1.1f, -.79f);
        Cube("GeneratorPipe", generator.transform, new Vector3(1.0f, 2.05f, .3f), new Vector3(.22f, 1.1f, .22f), M("DarkMetal"), false).transform.localPosition = new Vector3(1.0f, 2.05f, .3f);
        GeneratorInteractable generatorInteract = generator.AddComponent<GeneratorInteractable>();
        generatorInteract.Configure(progress, M("OnlineGreen"));

        Renderer generatorStatus = Cube("GeneratorStatus", generator.transform, new Vector3(.45f, 1.25f, -.84f), new Vector3(.16f, .16f, .05f), M("OfflineRed"), false).GetComponent<Renderer>();
        generatorStatus.transform.localPosition = new Vector3(.45f, 1.25f, -.84f);

        // ------------------------------------------------------------------
        // Workshop powered lights, off at start
        // ------------------------------------------------------------------
        List<Light> poweredLights = new();
        for (int z = 0; z < 4; z++)
        {
            for (int x = 0; x < 2; x++)
            {
                float px = x == 0 ? -6f : 6f;
                float pz = -1f + z * 5.2f;
                GameObject fixture = Cube($"CeilingFixture_{x}_{z}", workshop, new Vector3(px, 4.75f, pz), new Vector3(3.1f, .10f, .34f), M("LightOff"), false);
                Light l = fixture.AddComponent<Light>();
                l.type = LightType.Point;
                l.range = 9f;
                l.intensity = 5.5f;
                l.color = new Color(.86f, .93f, 1f);
                l.shadows = LightShadows.Soft;
                l.enabled = false;
                poweredLights.Add(l);
            }
        }
        powerSystem.Configure(poweredLights.ToArray(), new[] { generatorStatus, partsStatus }, M("OnlineGreen"));
        UnityEventTools.AddPersistentListener(generatorInteract.OnActivated, powerSystem.RestorePower);
        UnityEventTools.AddPersistentListener(generatorInteract.OnActivated, spawner.Spawn);

        // ------------------------------------------------------------------
        // Vehicle bay / placeholder car with readable silhouette
        // ------------------------------------------------------------------
        GameObject vehicle = new GameObject("Vehicle_RecoveryTarget");
        vehicle.transform.SetParent(gameplay);
        vehicle.transform.position = new Vector3(3.1f, .05f, 7f);
        vehicle.transform.rotation = Quaternion.Euler(0, 0, 0);
        VehicleInteractable vehicleInteract = vehicle.AddComponent<VehicleInteractable>();
        VehicleRecoveryVisual vehicleVisual = vehicle.AddComponent<VehicleRecoveryVisual>();

        GameObject chassis = Cube("Chassis", vehicle.transform, new Vector3(0, .72f, 0), new Vector3(2.7f, .55f, 5.2f), M("VehicleBody"));
        chassis.transform.localPosition = new Vector3(0, .72f, 0);
        GameObject cabin = Cube("Cabin", vehicle.transform, new Vector3(0, 1.35f, -.35f), new Vector3(2.3f, .75f, 2.25f), M("VehicleBody"));
        cabin.transform.localPosition = new Vector3(0, 1.35f, -.35f);
        GameObject windshield = Cube("Windshield", vehicle.transform, new Vector3(0, 1.55f, .82f), new Vector3(2.05f, .48f, .06f), M("DarkGlass"), false);
        windshield.transform.localPosition = new Vector3(0, 1.55f, .82f);
        // hood open-ish visual
        GameObject hood = Cube("Hood", vehicle.transform, new Vector3(0, 1.06f, 1.75f), new Vector3(2.45f, .12f, 1.45f), M("VehicleBody"), false, Quaternion.Euler(-12f, 0, 0));
        hood.transform.localPosition = new Vector3(0, 1.08f, 1.75f);
        // wheels
        for (int side = -1; side <= 1; side += 2)
            for (int axle = -1; axle <= 1; axle += 2)
            {
                GameObject wheel = Cylinder($"Wheel_{side}_{axle}", vehicle.transform, new Vector3(side * 1.45f, .55f, axle * 1.65f), new Vector3(.62f, .28f, .62f), M("Tire"), false, Quaternion.Euler(0, 0, 90));
                wheel.transform.localPosition = new Vector3(side * 1.45f, .55f, axle * 1.65f);
            }
        // battery socket visual
        Renderer vehicleStatus = Cube("BatterySocketStatus", vehicle.transform, new Vector3(-.65f, 1.12f, 1.62f), new Vector3(.45f, .16f, .35f), M("OfflineRed"), false).GetComponent<Renderer>();
        vehicleStatus.transform.localPosition = new Vector3(-.65f, 1.12f, 1.62f);

        Light leftHead = CreateSpotLight("Headlight_L", vehicle.transform, new Vector3(-.82f, .83f, 2.63f), Quaternion.Euler(5f, 0, 0), new Color(1f, .88f, .62f), 11f, 32f, false);
        Light rightHead = CreateSpotLight("Headlight_R", vehicle.transform, new Vector3(.82f, .83f, 2.63f), Quaternion.Euler(5f, 0, 0), new Color(1f, .88f, .62f), 11f, 32f, false);
        vehicleVisual.Configure(new[] { leftHead, rightHead }, vehicleStatus, M("OnlineGreen"));

        // ------------------------------------------------------------------
        // Garage door and exit route
        // ------------------------------------------------------------------
        GameObject door = Cube("GarageDoor_Route", gameplay, new Vector3(3.1f, 2.55f, 18.05f), new Vector3(7.1f, 5.1f, .28f), M("GarageDoor"));
        GarageDoor garageDoor = door.AddComponent<GarageDoor>();
        garageDoor.Configure(new Vector3(0, 5.6f, 0), 3.7f);
        // segmented door strips
        for (int i = 0; i < 6; i++)
            Cube($"DoorStripe_{i}", door.transform, new Vector3(0, -2.05f + i * .82f, -.16f), new Vector3(6.75f, .05f, .05f), M("DarkMetal"), false).transform.localPosition = new Vector3(0, -2.05f + i * .82f, -.16f);

        Cube("ExitRoad", route, new Vector3(3.1f, -.18f, 31f), new Vector3(8.5f, .28f, 26f), M("Asphalt"));
        Cube("ExitDirtL", route, new Vector3(-4.4f, -.25f, 31f), new Vector3(6.5f, .24f, 27f), M("Dust"));
        Cube("ExitDirtR", route, new Vector3(10.6f, -.25f, 31f), new Vector3(6.5f, .24f, 27f), M("Dust"));
        for (int i = 0; i < 7; i++)
            Cube($"RoadDash_{i}", route, new Vector3(3.1f, .01f, 21f + i * 3.4f), new Vector3(.18f, .025f, 1.35f), M("RoadLine"), false);
        CreateRoadSign(route, new Vector3(8.3f, 1.9f, 23f), "ROUTE 17\nNORTH", 180f);
        CreateBarrier(route, new Vector3(-.5f, .55f, 22.5f), 90f);

        // finish trigger beyond door
        GameObject finish = new GameObject("FinishTrigger_RouteUnlocked");
        finish.transform.SetParent(route);
        finish.transform.position = new Vector3(3.1f, 1.2f, 24.5f);
        BoxCollider finishCollider = finish.AddComponent<BoxCollider>();
        finishCollider.isTrigger = true;
        finishCollider.size = new Vector3(7.5f, 2.4f, 2.2f);
        DustlineFinishTrigger finishTrigger = finish.AddComponent<DustlineFinishTrigger>();

        // ------------------------------------------------------------------
        // Physics props / obstacle cluster
        // ------------------------------------------------------------------
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos = new Vector3(-6.5f + (i % 2) * 1.15f, .5f + (i / 2) * .15f, 1.3f + (i / 2) * 1.25f);
            GameObject crate = Cube($"PhysicsCrate_{i + 1}", props, pos, new Vector3(1f, 1f, 1f), M("Crate"));
            Rigidbody rb = crate.AddComponent<Rigidbody>();
            rb.mass = 4f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            crate.AddComponent<CollisionFeedback>();
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject barrel = Cylinder($"PhysicsBarrel_{i + 1}", props, new Vector3(-9f + i * 1.15f, .65f, -1.0f), new Vector3(.72f, .65f, .72f), M("Barrel"), true);
            Rigidbody rb = barrel.AddComponent<Rigidbody>();
            rb.mass = 7f;
            barrel.AddComponent<CollisionFeedback>();
        }

        // tool trolley visual
        Cube("ToolTrolley", props, new Vector3(7.7f, .55f, 1f), new Vector3(1.2f, 1.1f, .75f), M("SafetyOrange"));
        Cube("ToolTrolleyTop", props, new Vector3(7.7f, 1.17f, 1f), new Vector3(1.28f, .08f, .82f), M("DarkMetal"));

        // ------------------------------------------------------------------
        // Player + camera
        // ------------------------------------------------------------------
        GameObject player = new GameObject("Player");
        player.transform.SetParent(gameplay);
        player.transform.position = new Vector3(-2.2f, .05f, -13.8f);
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.radius = .34f;
        cc.center = new Vector3(0, .9f, 0);
        PlayerMovement movement = player.AddComponent<PlayerMovement>();
        PlayerCargo cargo = player.AddComponent<PlayerCargo>();
        DustlinePlayerInteraction interaction = player.AddComponent<DustlinePlayerInteraction>();

        GameObject body = Capsule("PlayerVisual", player.transform, Vector3.zero, new Vector3(.68f, .9f, .68f), M("WorkerOrange"), false);
        body.transform.localPosition = new Vector3(0, .9f, 0);
        GameObject helmet = Sphere("Helmet", player.transform, Vector3.zero, new Vector3(.52f, .34f, .52f), M("Helmet"), false);
        helmet.transform.localPosition = new Vector3(0, 1.78f, 0);
        GameObject backpack = Cube("Backpack", player.transform, Vector3.zero, new Vector3(.5f, .7f, .28f), M("DarkMetal"), false);
        backpack.transform.localPosition = new Vector3(0, 1.0f, -.38f);
        Transform carrySocket = new GameObject("CarrySocket").transform;
        carrySocket.SetParent(player.transform);
        carrySocket.localPosition = new Vector3(0, 1.05f, .58f);
        cargo.Configure(carrySocket, M("BatteryCyan"));

        GameObject cameraGO = new GameObject("Main Camera");
        cameraGO.tag = "MainCamera";
        cameraGO.transform.SetParent(marker.transform);
        Camera cam = cameraGO.AddComponent<Camera>();
        cam.fieldOfView = 64f;
        cam.nearClipPlane = .12f;
        cam.farClipPlane = 220f;
        cameraGO.AddComponent<AudioListener>();
        ThirdPersonCamera thirdCam = cameraGO.AddComponent<ThirdPersonCamera>();
        thirdCam.Configure(player.transform);

        InputActionAsset actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
        movement.Configure(actions, cameraGO.transform);
        vehicleInteract.Configure(cargo, progress);

        // ------------------------------------------------------------------
        // UI / HUD
        // ------------------------------------------------------------------
        GameObject canvasGO = new GameObject("HUD");
        canvasGO.transform.SetParent(marker.transform);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        TMP_Text titleUI = UI_Text(canvasGO.transform, "Title", "DUSTLINE  //  ESTACIÓN DE SERVICIO 01", 25, TextAlignmentOptions.TopRight,
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(-42, -35), new Vector2(650, 60), new Color(1f, .65f, .3f));
        TMP_Text objectiveUI = UI_Text(canvasGO.transform, "Objective", "", 29, TextAlignmentOptions.TopLeft,
            new Vector2(0, 1), new Vector2(0, 1), new Vector2(48, -55), new Vector2(880, 100), Color.white);
        TMP_Text promptUI = UI_Text(canvasGO.transform, "InteractPrompt", "", 31, TextAlignmentOptions.Bottom,
            new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(0, 85), new Vector2(900, 80), new Color(1f, .76f, .36f));
        TMP_Text crosshair = UI_Text(canvasGO.transform, "Crosshair", "+", 27, TextAlignmentOptions.Center,
            new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, new Vector2(40, 40), new Color(1, 1, 1, .82f));
        _ = titleUI; _ = crosshair;

        GameObject objectiveBack = UI_Panel(canvasGO.transform, "ObjectiveBack", new Vector2(0, 1), new Vector2(0, 1), new Vector2(38, -42), new Vector2(900, 118), new Color(.02f, .025f, .03f, .70f));
        objectiveBack.transform.SetSiblingIndex(0);

        DustlineObjectiveHUD objectiveHUD = canvasGO.AddComponent<DustlineObjectiveHUD>();
        objectiveHUD.Configure(progress, objectiveUI);
        interaction.Configure(cam, actions, promptUI);

        // completion overlay
        GameObject completionPanel = UI_Panel(canvasGO.transform, "CompletionPanel", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, new Color(.02f, .025f, .03f, .92f), stretch: true);
        TMP_Text completionText = UI_Text(completionPanel.transform, "CompletionText", "", 54, TextAlignmentOptions.Center,
            new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, new Vector2(1050, 360), Color.white);
        CompletionUI completionUI = canvasGO.AddComponent<CompletionUI>();
        completionUI.Configure(completionPanel, completionText);
        finishTrigger.Configure(progress, completionUI);

        // ------------------------------------------------------------------
        // Wire vehicle recovery through UnityEvent (Class 7 evidence)
        // ------------------------------------------------------------------
        UnityEventTools.AddPersistentListener(vehicleInteract.OnRecovered, vehicleVisual.Recover);
        UnityEventTools.AddPersistentListener(vehicleInteract.OnRecovered, garageDoor.Open);

        // environmental teaser sign near garage
        WorldText("ROUTE ACCESS", workshop, new Vector3(3.1f, 4.25f, 17.75f), Quaternion.Euler(0, 180, 0), .58f, new Color(1f, .62f, .25f), TextAlignmentOptions.Center);
        WorldText("VEHICLE REQUIRED", workshop, new Vector3(3.1f, 3.62f, 17.74f), Quaternion.Euler(0, 180, 0), .31f, Color.white, TextAlignmentOptions.Center);

        // Save
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = player;
        SceneView.lastActiveSceneView?.FrameSelected();
        Debug.Log("DUSTLINE V2: Estación 01 reconstruida con taller, generador, parts, vehículo, físicas, eventos y ruta final.");

        if (showDialog)
            EditorUtility.DisplayDialog("DUSTLINE", "Estación 01 V2 reconstruida.\n\nAbrí Play y seguí: vehículo → generador → PARTS → batería → vehículo → ruta.", "OK");
    }

    // ----------------------------------------------------------------------
    // Creation helpers
    // ----------------------------------------------------------------------
    static Transform NewRoot(string name, Transform parent)
    {
        Transform t = new GameObject(name).transform;
        t.SetParent(parent);
        return t;
    }

    static Material M(string name) => mats[name];

    static void CreateMaterials()
    {
        mats.Clear();
        AddMat("Concrete", new Color(.37f, .36f, .33f), 0f, .13f);
        AddMat("ConcreteDark", new Color(.20f, .21f, .21f), 0f, .16f);
        AddMat("WorkshopFloor", new Color(.19f, .20f, .19f), 0f, .18f);
        AddMat("Asphalt", new Color(.075f, .075f, .07f), 0f, .08f);
        AddMat("Dust", new Color(.39f, .27f, .16f), 0f, .05f);
        AddMat("DarkMetal", new Color(.09f, .105f, .11f), .65f, .34f);
        AddMat("Rust", new Color(.38f, .15f, .065f), .45f, .19f);
        AddMat("SafetyOrange", new Color(.91f, .35f, .07f), .15f, .25f);
        AddMat("SafetyYellow", new Color(.95f, .58f, .06f), .1f, .22f);
        AddMat("OfflineRed", new Color(.42f, .035f, .025f), .1f, .2f, new Color(.55f, .015f, .006f));
        AddMat("OnlineGreen", new Color(.04f, .55f, .22f), .1f, .25f, new Color(.02f, 1.4f, .25f));
        AddMat("LightOff", new Color(.19f, .21f, .19f), .2f, .4f);
        AddMat("VehicleBody", new Color(.46f, .105f, .055f), .5f, .32f);
        AddMat("GarageDoor", new Color(.17f, .19f, .19f), .65f, .3f);
        AddMat("DarkGlass", new Color(.035f, .07f, .08f), .15f, .8f);
        AddMat("Tire", new Color(.022f, .022f, .019f), 0f, .08f);
        AddMat("PitBlack", new Color(.018f, .018f, .018f), 0f, .05f);
        AddMat("Crate", new Color(.34f, .22f, .11f), 0f, .12f);
        AddMat("Barrel", new Color(.15f, .24f, .27f), .4f, .2f);
        AddMat("WorkerOrange", new Color(.92f, .31f, .06f), 0f, .25f);
        AddMat("Helmet", new Color(.83f, .78f, .55f), .1f, .25f);
        AddMat("BatteryCyan", new Color(.03f, .52f, .62f), .25f, .35f, new Color(0f, .52f, .65f));
        AddMat("RoadLine", new Color(.85f, .76f, .50f), 0f, .12f);
    }

    static Material AddMat(string name, Color color, float metallic, float smoothness, Color? emission = null)
    {
        string path = $"{MaterialFolder}/{name}.mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            mat = new Material(shader) { name = name };
            AssetDatabase.CreateAsset(mat, path);
        }
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        else if (mat.HasProperty("_Color")) mat.color = color;
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
        if (emission.HasValue && mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emission.Value);
        }
        EditorUtility.SetDirty(mat);
        mats[name] = mat;
        return mat;
    }

    static GameObject CreateBatteryPrefab()
    {
        string path = $"{PrefabFolder}/Battery.prefab";
        GameObject root = new GameObject("Battery_Module");
        root.AddComponent<BoxCollider>().size = new Vector3(.9f, .55f, .55f);
        Rigidbody rb = root.AddComponent<Rigidbody>();
        rb.mass = 2.2f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        root.AddComponent<BatteryPickup>();
        root.AddComponent<CollisionFeedback>();

        GameObject body = Cube("Body", root.transform, Vector3.zero, new Vector3(.9f, .55f, .55f), M("BatteryCyan"), false);
        body.transform.localPosition = Vector3.zero;
        GameObject band = Cube("Band", root.transform, Vector3.zero, new Vector3(.16f, .61f, .61f), M("DarkMetal"), false);
        band.transform.localPosition = Vector3.zero;
        GameObject terminalA = Cube("TerminalA", root.transform, Vector3.zero, new Vector3(.14f, .16f, .14f), M("SafetyOrange"), false);
        terminalA.transform.localPosition = new Vector3(-.22f, .36f, 0);
        GameObject terminalB = Cube("TerminalB", root.transform, Vector3.zero, new Vector3(.14f, .16f, .14f), M("DarkMetal"), false);
        terminalB.transform.localPosition = new Vector3(.22f, .36f, 0);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        UnityEngine.Object.DestroyImmediate(root);
        return prefab;
    }

    static GameObject Cube(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider = true, Quaternion? rotation = null)
        => Primitive(name, PrimitiveType.Cube, parent, pos, scale, mat, collider, rotation ?? Quaternion.identity);
    static GameObject Sphere(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider = true, Quaternion? rotation = null)
        => Primitive(name, PrimitiveType.Sphere, parent, pos, scale, mat, collider, rotation ?? Quaternion.identity);
    static GameObject Capsule(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider = true, Quaternion? rotation = null)
        => Primitive(name, PrimitiveType.Capsule, parent, pos, scale, mat, collider, rotation ?? Quaternion.identity);
    static GameObject Cylinder(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider = true, Quaternion? rotation = null)
        => Primitive(name, PrimitiveType.Cylinder, parent, pos, scale, mat, collider, rotation ?? Quaternion.identity);

    static GameObject Primitive(string name, PrimitiveType type, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider, Quaternion rotation)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = pos;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;
        Renderer r = go.GetComponent<Renderer>();
        if (r != null && mat != null) r.sharedMaterial = mat;
        if (!collider)
        {
            Collider c = go.GetComponent<Collider>();
            if (c != null) UnityEngine.Object.DestroyImmediate(c);
        }
        return go;
    }

    static void CreateWorkbench(Transform parent, Vector3 pos)
    {
        Cube("WorkbenchTop", parent, pos + Vector3.up * .95f, new Vector3(3.2f, .18f, 1.1f), M("DarkMetal"));
        foreach (float x in new[] { -1.35f, 1.35f })
            foreach (float z in new[] { -.38f, .38f })
                Cube("BenchLeg", parent, pos + new Vector3(x, .45f, z), new Vector3(.14f, .9f, .14f), M("DarkMetal"));
        Cube("ToolBox", parent, pos + new Vector3(.8f, 1.25f, 0), new Vector3(.7f, .42f, .45f), M("SafetyOrange"));
    }

    static void CreateShelving(Transform parent, Vector3 pos, Quaternion rot)
    {
        GameObject root = new GameObject("IndustrialShelf");
        root.transform.SetParent(parent);
        root.transform.SetPositionAndRotation(pos, rot);
        foreach (float x in new[] { -1.4f, 1.4f })
            foreach (float z in new[] { -.42f, .42f })
            {
                GameObject leg = Cube("ShelfPost", root.transform, Vector3.zero, new Vector3(.10f, 3.2f, .10f), M("DarkMetal"));
                leg.transform.localPosition = new Vector3(x, 0, z);
            }
        foreach (float y in new[] { -1.35f, -.35f, .65f, 1.45f })
        {
            GameObject shelf = Cube("Shelf", root.transform, Vector3.zero, new Vector3(3f, .10f, 1f), M("DarkMetal"));
            shelf.transform.localPosition = new Vector3(0, y, 0);
        }
    }

    static void CreateBarrier(Transform parent, Vector3 pos, float yaw)
    {
        GameObject root = new GameObject("RoadBarrier");
        root.transform.SetParent(parent);
        root.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, yaw, 0));
        GameObject bar = Cube("BarrierBar", root.transform, Vector3.zero, new Vector3(3.1f, .42f, .28f), M("SafetyOrange"));
        bar.transform.localPosition = new Vector3(0, .75f, 0);
        foreach (float x in new[] { -1.15f, 1.15f })
        {
            GameObject leg = Cube("BarrierLeg", root.transform, Vector3.zero, new Vector3(.22f, 1.0f, .22f), M("DarkMetal"));
            leg.transform.localPosition = new Vector3(x, .35f, 0);
        }
    }

    static void CreateRoadSign(Transform parent, Vector3 pos, string text, float yaw)
    {
        Cube("SignPost", parent, pos + new Vector3(0, -.9f, 0), new Vector3(.12f, 2.4f, .12f), M("DarkMetal"));
        Cube("SignBacking", parent, pos, new Vector3(2.5f, 1.25f, .12f), M("DarkMetal"), false, Quaternion.Euler(0, yaw, 0));
        WorldText(text, parent, pos + Quaternion.Euler(0, yaw, 0) * new Vector3(0, 0, -.07f), Quaternion.Euler(0, yaw, 0), .32f, Color.white, TextAlignmentOptions.Center);
    }

    static Light CreateSpotLight(string name, Transform parent, Vector3 localPos, Quaternion localRot, Color color, float range, float angle, bool enabled)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        go.transform.localPosition = localPos;
        go.transform.localRotation = localRot;
        Light l = go.AddComponent<Light>();
        l.type = LightType.Spot;
        l.color = color;
        l.intensity = 8f;
        l.range = range;
        l.spotAngle = angle;
        l.shadows = LightShadows.Soft;
        l.enabled = enabled;
        return l;
    }

    static TextMeshPro WorldText(string text, Transform parent, Vector3 pos, Quaternion rot, float size, Color color, TextAlignmentOptions alignment)
    {
        GameObject go = new GameObject("SignText_" + text.Replace("\n", "_"));
        go.transform.SetParent(parent);
        go.transform.position = pos;
        go.transform.rotation = rot;
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = size * 10f;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.rectTransform.sizeDelta = new Vector2(9f, 3f);
        return tmp;
    }

    static GameObject UI_Panel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 size, Color color, bool stretch = false)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        if (stretch)
        {
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
        else
        {
            rt.pivot = anchorMin;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = size;
        }
        go.GetComponent<Image>().color = color;
        return go;
    }

    static TMP_Text UI_Text(Transform parent, string name, string text, float fontSize, TextAlignmentOptions alignment,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 size, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = anchorMin == anchorMax ? anchorMin : new Vector2(.5f, .5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.enableWordWrapping = true;
        return tmp;
    }
}
#endif
