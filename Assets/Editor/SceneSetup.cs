using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[InitializeOnLoad]
public static class SceneSetup
{
    private const string Task = "tooltips";
    private const string ReportPath = "Logs/SceneSetup.txt";
    private const string FontPath = "Assets/_Course Library/Fonts/Inter-Regular-slnt=0 SDF.asset";
    private static readonly Color Teal = new Color(0.31f, 0.82f, 0.77f);
    private static TMP_FontAsset font;
    private static Transform group;

    static SceneSetup()
    {
        EditorApplication.delayCall += Run;
    }

    private static void Run()
    {
        if (File.Exists(ReportPath) && File.ReadAllText(ReportPath).Contains("done " + Task))
        {
            return;
        }
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
        {
            EditorApplication.delayCall += Run;
            return;
        }
        Report("start " + Task);
        font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
        Material marker = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        marker.SetColor("_BaseColor", Teal);
        AssetDatabase.CreateAsset(marker, "Assets/Materials/SocketMarker.mat");

        Scene course = Open("Assets/Scenes/Eseosa Room.unity");
        Hover(course, "Tennis Ball", "Grip to grab, let go while moving to throw");
        Hover(course, "Tennis Racket", "Grab the handle and hit the ball");
        Hover(course, "Remote", "Hold it and pull the trigger to play or pause the TV");
        Hover(course, "Captain Hat", "Hang me on the hook or put me on your head");
        Hover(course, "Straw Hat", "Hang me on the hook or put me on your head");
        Sign(course, "Hook", "Hat hook: only hats fit here", 0.3f);
        Save(course);

        Scene room = Open("Assets/Scenes/VR Room.unity");
        Hover(room, "Door Handle", "Hold grip, then pull or push to swing the door");
        Hover(room, "Flashlight", "Only fits back in its dock");
        foreach (string piece in new[] { "KnightLight", "RookDark", "PawnDark", "BishopLight" })
        {
            Hover(room, piece, "Place me on the glowing chess square");
        }
        Transform chess = Find(room, "Chess Socket");
        Transform board = Find(room, "ChessBoard");
        float square = Bounds(board.gameObject).size.x * 0.85f / 8f;
        Marker(marker, chess, chess.position + Vector3.up * 0.002f, square * 0.8f);
        Sign(room, "Chess Socket", "Chess square: chess pieces only", 0.25f);
        Transform dock = Find(room, "Flashlight Dock");
        Marker(marker, dock, dock.position + Vector3.up * 0.452f, 0.24f);
        Sign(room, "Flashlight Dock", "Flashlight dock: flashlight only", 0.85f);
        Save(room);
        Report("done " + Task);
    }

    private static Scene Open(string path)
    {
        Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        Transform ui = Find(scene, "INTERFACE");
        group = new GameObject("Tooltips").transform;
        group.SetParent(ui, false);
        return scene;
    }

    private static void Hover(Scene scene, string name, string text)
    {
        XRBaseInteractable target = null;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (XRGrabInteractable grab in root.GetComponentsInChildren<XRGrabInteractable>(true))
            {
                if (grab.name == name)
                {
                    target = grab;
                }
            }
        }
        float height = Bounds(target.gameObject).max.y - target.transform.position.y + 0.12f;
        GameObject panel;
        GameObject tooltip = Tooltip(name + " Tooltip", target.transform, height, text, out panel);
        HoverTooltipListener listener = tooltip.AddComponent<HoverTooltipListener>();
        SerializedObject data = new SerializedObject(listener);
        data.FindProperty("interactable").objectReferenceValue = target;
        data.FindProperty("panel").objectReferenceValue = panel;
        data.ApplyModifiedPropertiesWithoutUndo();
        panel.SetActive(false);
        Report("hover tooltip on " + name + ": " + text);
    }

    private static void Sign(Scene scene, string name, string text, float height)
    {
        GameObject panel;
        Tooltip(name + " Sign", Find(scene, name), height, text, out panel);
        Report("sign above " + name + ": " + text);
    }

    private static GameObject Tooltip(string name, Transform target, float height, string text, out GameObject panel)
    {
        GameObject root = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(TooltipPlacement));
        root.layer = 5;
        root.transform.SetParent(group, false);
        root.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        RectTransform rect = root.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(460f, 90f);
        rect.localScale = Vector3.one * 0.001f;
        rect.position = target.position + Vector3.up * height;
        SerializedObject placement = new SerializedObject(root.GetComponent<TooltipPlacement>());
        placement.FindProperty("target").objectReferenceValue = target;
        placement.FindProperty("height").floatValue = height;
        placement.ApplyModifiedPropertiesWithoutUndo();
        panel = new GameObject("Panel", typeof(RectTransform), typeof(Image), typeof(Outline));
        panel.layer = 5;
        panel.transform.SetParent(root.transform, false);
        panel.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
        panel.GetComponent<Image>().color = new Color(0.05f, 0.07f, 0.11f, 0.9f);
        panel.GetComponent<Image>().raycastTarget = false;
        panel.GetComponent<Outline>().effectColor = Teal;
        panel.GetComponent<Outline>().effectDistance = new Vector2(3f, -3f);
        GameObject label = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.layer = 5;
        label.transform.SetParent(panel.transform, false);
        label.GetComponent<RectTransform>().sizeDelta = new Vector2(440f, 80f);
        TextMeshProUGUI tmp = label.GetComponent<TextMeshProUGUI>();
        tmp.font = font;
        tmp.text = text;
        tmp.fontSize = 26f;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;
        return root;
    }

    private static void Marker(Material material, Transform parent, Vector3 position, float size)
    {
        GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "Marker";
        Object.DestroyImmediate(ring.GetComponent<Collider>());
        ring.transform.SetParent(parent, true);
        ring.transform.position = position;
        ring.transform.rotation = Quaternion.identity;
        ring.transform.localScale = Vector3.one;
        Vector3 lossy = ring.transform.lossyScale;
        ring.transform.localScale = new Vector3(size / lossy.x, 0.002f / lossy.y, size / lossy.z);
        ring.GetComponent<Renderer>().sharedMaterial = material;
        Report("glowing marker under " + parent.name + " at " + position);
    }

    private static Bounds Bounds(GameObject target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    private static void Save(Scene scene)
    {
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static Transform Find(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == name)
                {
                    return child;
                }
            }
        }
        return null;
    }

    private static void Report(string line)
    {
        File.AppendAllText(ReportPath, System.DateTime.Now.ToString("HH:mm:ss") + " " + line + "\n");
    }
}
