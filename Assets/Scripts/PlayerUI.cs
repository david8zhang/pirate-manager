using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Text goldAmountLabel;

    [SerializeField]
    Button showShipsButton;

    [SerializeField]
    Button cancelRaidButton;

    public void Start()
    {
        cancelRaidButton.onClick.AddListener(() => CancelRaid());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetTotalGoldAmount(int goldAmount)
    {
        goldAmountLabel.text = "Gold: " + goldAmount.ToString();
    }

    public void ShowCancelRaid()
    {
        showShipsButton.gameObject.SetActive(false);
        goldAmountLabel.gameObject.SetActive(false);
        cancelRaidButton.gameObject.SetActive(true);
    }

    public void HideCancelRaid()
    {
        showShipsButton.gameObject.SetActive(true);
        goldAmountLabel.gameObject.SetActive(true);
        cancelRaidButton.gameObject.SetActive(false);
    }

    public void CancelRaid()
    {
        GameManager.instance.player.CloseRaidTargetSelect();
    }

    public void OnShowShipsClick()
    {
    }
}
