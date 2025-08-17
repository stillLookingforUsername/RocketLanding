using System;
using TMPro;
using UnityEngine;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    /*
    private void Awake()
    {
        Hide();
    }
    */
    //never disable gameObject(here using Hide()) at Awake() as it will lead to not running Start()
    //which will again lead to not subscribing to the Onlanded Event (i.e., Lander.Instance.OnLanded += Lander_Onlanded)
    

    //listening to the event OnLanded
    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL";
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
        }
        //the value is too small so we multiply e.landingSpeed & e.dotVecot with offset 2f and 100f respectively
        statsTextMesh.text =
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            Mathf.Round(e.dotVector * 100f) + "\n" +
            "x" + e.scoreMultiplier + "\n" +
            e.score;
        Show();
    }

    //visibility of this window
    //we don't want it to be visible all the time - we want to show only when we land

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}