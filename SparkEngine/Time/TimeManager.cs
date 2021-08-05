using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SparkEngine.Time
{
    public static class TimeManager
    {
        #region Fields

        private static readonly List<TemporaryTicker> temporaryTickers = new List<TemporaryTicker>();

        #endregion

        #region Methods

        internal static void Update(GameTime gameTime)
        {
            // Probably should do this differently.

            for (var i = 0; i < temporaryTickers.Count; i++)
            {
                var ticker = temporaryTickers[i];
                if (ticker.IsActive)
                {
                    ticker.Update(gameTime); // This is bad.

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