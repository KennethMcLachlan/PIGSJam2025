using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private FoodBall ball;
    [SerializeField] private Vector3 startVelocity;

    #region Testing
#if UNITY_EDITOR
    [SerializeField] private bool testSpawnBall;
    private void FixedUpdate()
    {
        if (testSpawnBall)
        {
            SpawnBall();
            testSpawnBall = false;
        }
    }
#endif
    #endregion

    public void SpawnBall()
    {
        Instantiate(ball, this.transform).rb.linearVelocity = startVelocity;
    }
}