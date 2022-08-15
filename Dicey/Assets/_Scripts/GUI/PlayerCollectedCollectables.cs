using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollectedCollectables : MonoBehaviour
{
    private TMP_Text _collectedDiceText;

    int _amountCollectedDice;
    static int _amountOfCollectedDots;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnLevelLoad;
    }

    private void OnLevelLoad(Scene current, Scene next)
    {
        foreach (var item in ReferenceManager.INGAME_UI.GetComponentsInChildren<TMP_Text>())
        {
            if (item.name == "DiceCollectedCounter")
            {
                _collectedDiceText = item;
            }
        }
    }

    public void AddCollectableDice()
    {
        _amountCollectedDice++;
        _collectedDiceText.text = _amountCollectedDice.ToString();
    }

    public static void AddCollectableDot()
    {
        _amountOfCollectedDots++;
        if (_amountOfCollectedDots == 10)
        {
            FindObjectOfType<LevelWon>().ShowWinCamera();
            FindObjectOfType<DoorOpen>().OpenDoor();
        }
    }
}
