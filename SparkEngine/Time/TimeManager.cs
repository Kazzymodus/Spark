namespace SparkEngine.Time
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Player;

    public static class TimeManager
    {
        #region Fields

        private static List<TemporaryTicker> temporaryTickers = new List<TemporaryTicker>();

        #endregion

        #region Methods

        internal static void Update(GameTime gameTime, Player player)
        {
            // Probably should do this differently.

            for (int i = 0; i < temporaryTickers.Count; i++)
            {
                TemporaryTicker ticker = temporaryTickers[i];
                if (ticker.IsActive)
                {
                    ticker.Update(gameTime, player); // This is bad.

                    if (ticker.TimeLeft <= 0)
                    {
                        temporaryTickers.Remove(ticker);
                        i--;
                    }
                }
            }
        }

        internal static void RegisterTemporaryTicker(TemporaryTicker ticker)
        {
            temporaryTickers.Add(ticker);
        }

        #endregion
    }
}
