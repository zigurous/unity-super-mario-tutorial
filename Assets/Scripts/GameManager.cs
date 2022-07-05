using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives;
    private int coins;

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        coins = 0;
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }

    public void NextLevel()
    {
        // TODO
    }

    public void ResetLevel()
    {
        // TODO
    }

}
