using UnityEngine;

public class FoodTube : MonoBehaviour
{
    private bool tubeEnabled;
    [SerializeField] private FoodBall ball;
    [SerializeField] private Vector3 startVelocity;

    #region Testing
#if UNITY_EDITOR
    [SerializeField] private bool testSpawnBall;
    private void FixedUpdate()
    {
        if (testSpawnBall)
        {
            tubeEnabled = true;
            SpawnBall();
            testSpawnBall = false;
        }
    }
#endif
    #endregion

    public void SetTubeEnabled(bool enabled)
    {
        tubeEnabled = enabled;
        if(tubeEnabled)
        {
            //add logic for connecting to player controls
        }
    }

    public void SpawnBall()
    {
        if(tubeEnabled)
        {
            Instantiate(ball, this.transform).rb.linearVelocity = startVelocity;
        }
    }
}