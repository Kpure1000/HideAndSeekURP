using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeCubemap : ScriptableWizard
{
    public Transform renderPosition;
    public Cubemap cubemap;

    void OnWizardUpdate()
    {
        helpString = "Select transform to render" +
                       " from and cubemap to render into";
        if (renderPosition != null && cubemap != null)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }

    void OnWizardCreate()
    {
        GameObject go = new GameObject("CubeCam");

        go.transform.position = renderPosition.position;
        go.transform.rotation = Quaternion.identity;

        var camera = go.AddComponent<Camera>();

        camera.RenderToCubemap(cubemap);

        DestroyImmediate(go);
    }

    [MenuItem("My Tools/Render Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard("Render CubeMap", typeof(MakeCubemap), "Render!");
    }
}
