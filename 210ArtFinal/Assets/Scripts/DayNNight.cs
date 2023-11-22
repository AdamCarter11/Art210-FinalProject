using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNNight : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f; // Speed at which the light rotates around the X-axis
    [SerializeField] GameObject fireFlies;

    [SerializeField] Volume postProcessingVolume;
    [SerializeField] float vignetteIntensityMax = 0.6f; // Maximum intensity
    [SerializeField] float vignetteIntensityMin = 0.1f; // Minimum intensity
    [SerializeField] float lerpSpeed = 0.5f; // Speed of the lerp
    void Start()
    {
        // Assuming you've set up your volume in the Inspector or found it through code
        if (postProcessingVolume == null)
        {
            postProcessingVolume = GetComponent<Volume>();
            if (postProcessingVolume == null)
            {
                Debug.LogError("Post-Processing Volume not found. Make sure you've assigned it in the Inspector or attach this script to an object with the Volume component.");
            }
        }
    }
    void Update()
    {
        // Rotate the directional light around the X-axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        if(transform.rotation.eulerAngles.x < 0 || transform.rotation.eulerAngles.x > 180)
        {
            fireFlies.SetActive(true);
            // Lerp the vignette intensity to the maximum value
            float targetIntensity = Mathf.Lerp(GetVignetteIntensity(), vignetteIntensityMax, lerpSpeed * Time.deltaTime);
            SetVignetteIntensity(targetIntensity);
        }
        else
        {
            fireFlies.SetActive(false);
            // Lerp the vignette intensity to the minimum value
            float targetIntensity = Mathf.Lerp(GetVignetteIntensity(), vignetteIntensityMin, lerpSpeed * Time.deltaTime);
            SetVignetteIntensity(targetIntensity);
        }
    }
    float GetVignetteIntensity()
    {
        // Get the Vignette component from the volume
        if (postProcessingVolume.profile.TryGet(out Vignette vignette))
        {
            return vignette.intensity.value;
        }

        return 0f;
    }

    void SetVignetteIntensity(float intensity)
    {
        // Set the Vignette intensity in the volume
        if (postProcessingVolume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = intensity;
        }
    }
}
