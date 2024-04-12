public class Wallet
{

    public int Coins { get; private set; }

    public Wallet(int coins)
    {
        Coins = coins;
    }

    public void AddCoins(int value)
    {
        Coins += value;
    }

    public void TakeCoins(int value)
    {
        Coins -= value;
    }

}
