using UnityEngine;

public class DiceTowerChallengeController : MonoBehaviour
{
    public DiceRollDetector DiceRollDetector;
    public DiceTowerPassCheck DiceTowerPassCheck;
    public DiceTowerRangeCheck DiceTowerRangeCheck;

    public int DesiredNumber = 6;

    public GameObject DotSpawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        DiceRollDetector.AddDiceRollListener(processRollDetected);
    }

    void processRollDetected() 
    {
        if(DiceRollDetector.GetCurrentlyRolledNumber() == DesiredNumber &&
            DiceTowerPassCheck.HasPlayerPassed() &&
            DiceTowerRangeCheck.IsPlayerInRange())
        {
            SpawnReward();
            DiceRollDetector.RemoveDiceRollListeners();
            enabled = false;
        }
    }

    void SpawnReward()
    {
        if(DotSpawnPosition == null)
        {
            Debug.Log($"{typeof(DiceTowerChallengeController)}: No Dot Spawn Position set in inspector.");
            return;
        }

        DotAnimation.SpawnDot(DotSpawnPosition.transform.position);
    }
}
