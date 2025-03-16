using ilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InHouseUICanvas : MonoBehaviour
{
    public Text player1HealthText;
    public Text player1EnemyText;
    public Text player2HealthText;
    public Text player2EnemyText;
    public Image player1HealthImage;
    public Image player1EnergyImage;
    public Image player2HealthImage;
    public Image player2EnergyImage;

    //参数
    private int player1HealthInt = 1;
    private int currentPlayer1HealthInt;
    private int player1EnemyInt = 1;
    private int currentPlayer1EnemyInt;
    private int player2HealthInt = 1;
    private int currentPlayer2HealthInt;
    private int player2EnemyInt = 1;
    private int currentPlayer2EnemyInt;
    private void OnEnable()
    {
        currentPlayer1HealthInt = player1HealthInt;
        currentPlayer1EnemyInt = player1EnemyInt;
        currentPlayer2HealthInt = player2HealthInt;
        currentPlayer2EnemyInt = player2EnemyInt;
        //获取血量和能量上限与当前能量血量，再赋值
        player1HealthText.text = currentPlayer1HealthInt + "/" + player1HealthInt;
        player1EnemyText.text = currentPlayer1EnemyInt + "/" + player1EnemyInt;
        player2HealthText.text = currentPlayer2HealthInt + "/" + player2HealthInt;
        player2EnemyText.text = currentPlayer2EnemyInt + "/" + player2EnemyInt;
        player1HealthImage.fillAmount = currentPlayer1HealthInt / player1HealthInt;
        player1EnergyImage.fillAmount = currentPlayer1EnemyInt / player1EnemyInt;
        player2HealthImage.fillAmount = currentPlayer2HealthInt / player2HealthInt;
        player2EnergyImage.fillAmount = currentPlayer2EnemyInt / player2EnemyInt;
    }
    private void OnDisable()
    {
        
    }
}
