using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class testnoisescript : MonoBehaviour
{
    [SerializeField] public float noise;
    [SerializeField] private Material noiseMaterial;
    private Material materialInstance;

    [SerializeField] string fullScreenPassFeatureName;

    private UniversalRenderPipelineAsset pipeline;

    void Start()
    {
        /*materialInstance = new Material(noiseMaterial);

        pipeline = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;

        var _rendererFeatures = pipeline.scriptableRenderer.GetType().GetProperty("rendererFeatures", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(pipeline.scriptableRenderer, null) as List<ScriptableRendererFeature>;


        // Iterate through the renderer features to find the full screen pass feature
        foreach (var feature in _rendererFeatures)
        {
            if (feature is FullScreenPassRendererFeature fullScreenPassFeature &&
                feature.name == fullScreenPassFeatureName)
            {
                // Assuming the feature has a public material property
                fullScreenPassFeature.passMaterial = materialInstance;
            }
        }*/
    }
    
    void Update()
    {
        noiseMaterial.SetFloat("_intensity", noise); 
    }
}
