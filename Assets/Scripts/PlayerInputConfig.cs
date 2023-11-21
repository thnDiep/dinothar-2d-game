using UnityEngine;

[System.Serializable]
public class PlayerInputConfig
{
    public enum Player
    {
        Player1,
        Player2,
    }

    // Sử dụng để player phím bấm điều khiển
    private static KeyCode Player1MoveLeft = KeyCode.A;
    private static KeyCode Player1MoveRight = KeyCode.D;
    private static KeyCode Player1MoveUp = KeyCode.W;
    private static KeyCode Player1MoveDown = KeyCode.S;

    private static KeyCode Player2MoveLeft = KeyCode.LeftArrow;
    private static KeyCode Player2MoveRight = KeyCode.RightArrow;
    private static KeyCode Player2MoveUp = KeyCode.UpArrow;
    private static KeyCode Player2MoveDown = KeyCode.DownArrow;

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;

	public PlayerInputConfig(Player player)
	{
        if(player == Player.Player1)
        {
            moveLeft = Player1MoveLeft;
            moveRight = Player1MoveRight;
            moveUp = Player1MoveUp;
            moveDown = Player1MoveDown;
        } else // Player 2
        {
            moveLeft = Player2MoveLeft;
            moveRight = Player2MoveRight;
            moveUp = Player2MoveUp;
            moveDown = Player2MoveDown;
        }
	}
}