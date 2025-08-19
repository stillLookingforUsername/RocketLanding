using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private Transform _landerStartPositionTransform;

    //function to expose the above data

    public int GetLevelNumber()
    {
        return _levelNumber;
    }
    public Vector3 GetLevelStartPosition()
    {
        return _landerStartPositionTransform.position;
    }
  
}
