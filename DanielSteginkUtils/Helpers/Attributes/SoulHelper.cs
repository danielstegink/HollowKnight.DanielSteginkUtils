using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Libraries
{
    /// <summary>
    /// Code library for handling the player getting SOUL
    /// </summary>
    public static class SoulHelper
    {
        /// <summary>
        /// Gives the player SOUL while triggering the UI. Use this when you don't want to trigger an infinite loop of HeroController hooks
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="performLogging"></param>
        public static void GainSoul(int amount, bool performLogging = false)
        {
            int reserve = PlayerData.instance.MPReserve;
            PlayerData.instance.AddMPCharge(amount);

            // Sends FSM events so UI updates
            GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN");
            if (PlayerData.instance.MPReserve != reserve)
            {
                GameManager.instance.soulVessel_fsm.SendEvent("MP RESERVE UP");
            }

            if (performLogging)
            {
                Logging.Log("SoulHelper", $"{amount} SOUL sent to player.");
            }
        }
    }
}
