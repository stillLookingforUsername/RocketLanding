using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    //this script handles the visual (textMeshPro)
    [SerializeField] private TextMeshPro scoreMultiplierTextMesh;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierTextMesh.text = "x" + landingPad.GetScoreMultiplier();
    }
}