 using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollectedCollectables : MonoBehaviour
{
    public TMP_Text CollectedDiceText;

    int _amountCollectedDice;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnLevelLoad;
    }

    private void OnLevelLoad(Scene current, Scene next)
    {
        CollectedDiceText = GameObject.Find("DiceCollectedCounter").GetComponent<TMP_Text>();
    }


    public void AddCollectableDice()
    {
        _amountCollectedDice++;
        CollectedDiceText.text = _amountCollectedDice.ToString();
    }
}
