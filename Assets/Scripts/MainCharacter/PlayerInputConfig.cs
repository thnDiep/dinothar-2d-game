using UnityEngine;

[System.Serializable]
public class PlayerInputConfig
{

    // Sử dụng để player phím bấm điều khiển
    private static KeyCode Player1MoveLeft = KeyCode.A;
    private static KeyCode Player1MoveRight = KeyCode.D;
    private static KeyCode Player1MoveUp = KeyCode.W;
    private static KeyCode Player1MoveDown = KeyCode.S;
    private static KeyCode Player1Shoot = KeyCode.Space;
    private static KeyCode Player1UseSkill = KeyCode.F;
    private static KeyCode Player1UseCombineSkill = KeyCode.C;

    private static KeyCode Player2MoveLeft = KeyCode.LeftArrow;
    private static KeyCode Player2MoveRight = KeyCode.RightArrow;
    private static KeyCode Player2MoveUp = KeyCode.UpArrow;
    private static KeyCode Player2MoveDown = KeyCode.DownArrow;
    private static KeyCode Player2Shoot = KeyCode.Return;
    private static KeyCode Player2UseSkill = KeyCode.O;
    private static KeyCode Player2UseCombineSkill = KeyCode.P;

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode shoot;
    public KeyCode useSkill;
    public KeyCode useCombineSkill;

    public PlayerInputConfig(PlayerManager.Player player)
	{
        if(player == PlayerManager.Player.Player1)
        {
            moveLeft = Player1MoveLeft;
            moveRight = Player1MoveRight;
            moveUp = Player1MoveUp;
            moveDown = Player1MoveDown;
            shoot = Player1Shoot;
            useSkill = Player1UseSkill;
            useCombineSkill = Player1UseCombineSkill;
        } else // Player 2
        {
            moveLeft = Player2MoveLeft;
            moveRight = Player2MoveRight;
            moveUp = Player2MoveUp;
            moveDown = Player2MoveDown;
            shoot = Player2Shoot;
            useSkill = Player2UseSkill;
            useCombineSkill = Player2UseCombineSkill;
        }
    }
}