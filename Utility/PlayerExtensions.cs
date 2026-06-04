namespace EfficientQuesting;

public static class PlayerExtensions
{
    public static void ShowTooltip(this EntityPlayerLocal? player, string text)
    {
        if (player is null)
        {
            return;
        }

        GameManager.ShowTooltip(player, text, true);
    }
}