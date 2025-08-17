using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private UnityEngine.UI.Image fuelImage;

    private void Update()
    {
        UpdateStatsTextMesh();
    }
    private void UpdateStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        speedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        speedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0);
        speedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0);

        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalize();

        statsTextMesh.text = GameManager.Instance.GetScore() + "\n" +
            Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f));
    }
}
