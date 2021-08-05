using Microsoft.Xna.Framework;

namespace SparkEngine.Time
{
    /// <summary>
    ///     A ticker that will fire a limited amount of times in set intervals.
    /// </summary>
    public class TemporaryTicker : Ticker
    {
        #region Constructors

        private TemporaryTicker(float interval, float duration)
            : base(interval)
        {
            TimeLeft = duration;
        }

        #endregion

        #region Properties

        public float TimeLeft { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a ticker that will fire only once, after a set delay.
        /// </summary>
        /// <param name="delay">The delay until the ticker fires.</param>
        /// <returns></returns>
        public new static TemporaryTicker Create(float delay)
        {
            return Create(delay, delay);
        }

        /// <summary>
        ///     Creates a ticker that will tick for a set amount of ticks in set intervals.
        /// </summary>
        /// <param name="ticks">The amount of ticks.</param>
        /// <param name="interval">The length of the tick interval.</param>
        /// <returns></returns>
        public static TemporaryTicker Create(int ticks, float interval)
        {
            return Create(interval, ticks * interval);
        }

        /// <summary>
        ///     Creates a ticker that will fire in set intervals, until it expires.
        /// </summary>
        /// <param name="interval">The length of the tick interval.</param>
        /// <param name="duration">The total duration of the ticker.</param>
        /// <returns></returns>
        public static TemporaryTicker Create(float interval, float duration)
        {
            var ticker = new TemporaryTicker(interval, duration);
            TimeManager.RegisterTemporaryTicker(ticker);
            return ticker;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TimeLeft -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        #endregion
    }
}