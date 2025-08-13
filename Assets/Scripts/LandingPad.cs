using UnityEngine;

public class LandingPad : MonoBehaviour
{
    //this script only handles the logic
    [SerializeField] private int scoreMultiplier;


    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
    
    //public int GetScoreMultiplier => scoreMultiplier;
}