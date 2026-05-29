using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText;
    [SerializeField]
    private TextMeshProUGUI _goldText;
    [SerializeField]
    private TextMeshProUGUI _maxPlayerText;

    public void Level(int level)
    {
        _levelText.text = $"Level:{level}";
    }

    public void Gold(int gold)
    {
        _goldText.text = $"Gold:{gold}";
    }

    public void MaxPlayer()
    {
        _maxPlayerText.text = $"max:{Player.MaxEnablePlayer}";
    }

    void Update()
    {
        MaxPlayer();
    }
}
