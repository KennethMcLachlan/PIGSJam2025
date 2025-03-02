using UnityEngine;

public class FoodTube : MonoBehaviour
{
    #region Variables & References
    [Header("Variables")]
    [SerializeField] private float spawnVelocity;
    [SerializeField] private float cooldownTime;
    private bool tubeEnabled;
    private bool cooldown;
    [Space(20)]
    [Header("References")]
    [SerializeField] private BallCreature ball;
    [SerializeField] private Transform spawnPosition;
    [Space(10)]
    [SerializeField] private Animator tubeAnim;
    [Space(10)]
    [SerializeField] private RandomizedSound launchSound;
    [SerializeField] private RandomizedSound enabledSound;
    #endregion

    #region Testing
#if UNITY_EDITOR
    [Space(20)]
    [Header("Testing")]
    [SerializeField] private bool testSpawnBall;
    private void FixedUpdate()
    {
        if (testSpawnBall)
        {
            if (!tubeEnabled)
            {
                EnableTube();
            }
            LaunchBall();
            testSpawnBall = false;
        }
    }
#endif
    #endregion

    #region Tube Controls
    public void EnableTube()
    {
        tubeEnabled = true;
        //enabledSound.Play();
        tubeAnim.SetBool("Enabled", true);
        FindFirstObjectByType<LanternDevice>().SetActiveFoodTube(this);
    }

    public void LaunchBall()
    {
        if(tubeEnabled && !cooldown)
        {
            launchSound.PlaySound();
            tubeAnim.SetBool("Cooldown", true);
            Instantiate(ball, spawnPosition).rb.linearVelocity = -spawnPosition.up * spawnVelocity;
            Invoke(nameof(CooldownComplete), cooldownTime);
            cooldown = true;
        }
    }
    private void CooldownComplete()
    {
        cooldown = false;
        tubeAnim.SetBool("Cooldown", false);
    }

    public void DisableTube()
    {
        tubeEnabled = false;
        tubeAnim.SetBool("Enabled", false);
    }
    #endregion
}