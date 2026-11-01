using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneSetup
{
    private const string Task = "door-clear-floor";
    private const string ReportPath = "Logs/SceneSetup.txt";

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
        Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/VR Room.unity", OpenSceneMode.Single);
        Transform door = null;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "glass_panel_1_door")
                {
                    door = child;
                }
            }
        }
        Physics.SyncTransforms();
        BoxCollider leaf = door.GetComponent<BoxCollider>();
        float lowest = leaf.bounds.min.y;
        float floorTop = lowest;
        foreach (Collider hit in Physics.OverlapBox(leaf.bounds.center, leaf.bounds.extents, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore))
        {
            if (!hit.transform.IsChildOf(door) && !Physics.GetIgnoreLayerCollision(door.gameObject.layer, hit.gameObject.layer) && hit.bounds.max.y < leaf.bounds.center.y)
            {
                floorTop = Mathf.Max(floorTop, hit.bounds.max.y);
            }
        }
        float raise = floorTop + 0.02f - lowest;
        if (raise > 0f)
        {
            float localRaise = raise / Mathf.Abs(door.lossyScale.y);
            leaf.center += new Vector3(0f, localRaise * 0.5f, 0f);
            leaf.size -= new Vector3(0f, localRaise, 0f);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Physics.SyncTransforms();
        }
        Report("floor top under the door " + floorTop.ToString("0.000") + ", collider bottom raised by " + raise.ToString("0.000") + " to " + leaf.bounds.min.y.ToString("0.000"));
        HingeJoint hinge = door.GetComponent<HingeJoint>();
        Report("hinge anchor " + hinge.anchor + " axis " + hinge.axis + " limits " + hinge.limits.min + " to " + hinge.limits.max + " spring " + hinge.useSpring + " " + hinge.spring.spring + " target " + hinge.spring.targetPosition);
        foreach (Collider part in door.GetComponentsInChildren<Collider>())
        {
            Bounds b = part.bounds;
            Report("door part " + part.name + " layer " + part.gameObject.layer + " bounds min " + b.min.ToString("0.000") + " max " + b.max.ToString("0.000"));
            BoxCollider box = part as BoxCollider;
            if (box == null)
            {
                continue;
            }
            Vector3 centre = part.transform.TransformPoint(box.center);
            Vector3 half = Vector3.Scale(box.size, part.transform.lossyScale) * 0.5f;
            foreach (Collider hit in Physics.OverlapBox(centre, new Vector3(Mathf.Abs(half.x), Mathf.Abs(half.y), Mathf.Abs(half.z)), part.transform.rotation, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.IsChildOf(door))
                {
                    continue;
                }
                bool ignored = Physics.GetIgnoreLayerCollision(part.gameObject.layer, hit.gameObject.layer);
                Report("  overlaps " + hit.name + " (" + hit.GetType().Name + ", layer " + hit.gameObject.layer + ", parent " + (hit.transform.parent != null ? hit.transform.parent.name : "-") + ") ignored by layers: " + ignored);
            }
        }
        Report("done " + Task);
    }

    private static void Report(string line)
    {
        File.AppendAllText(ReportPath, System.DateTime.Now.ToString("HH:mm:ss") + " " + line + "\n");
    }
}
