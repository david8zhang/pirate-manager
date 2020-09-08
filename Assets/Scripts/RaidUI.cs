using UnityEngine;
using UnityEngine.UI;

public class RaidUI : MonoBehaviour
{
    [SerializeField]
    Button stopRaidingButton;

    // Start is called before the first frame update
    void Start()
    {
        stopRaidingButton.onClick.AddListener(() => StopRaiding());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void StopRaiding()
    {
        GameManager.instance.player.CloseRaidTargetSelect();
    }
}
