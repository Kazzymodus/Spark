namespace SparkEngine.Time
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SparkEngine.Player;

    /// <summary>
    /// A ticker that will continuously fire in set intervals.
    /// </summary>
    public class Ticker
    {
        #region Fields

        protected readonly float Interval;

        #endregion

        #region Constructors

        protected Ticker(float interval)
        {
            Interval = interval;
        }

        #endregion

        #region Events

        public event EventHandler<OnTickEventArgs> OnTickEvent;

        #endregion

        #region Properties

        public bool IsActive { get; private set; }

        protected float Timer { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a ticker that continuously fires in set intervals.
        /// </summary>
        /// <param name="interval">The length of the tick interval.</param>
        /// <returns></returns>
        public static Ticker Create(float interval)
        {
            return new Ticker(interval);
        }

        public void Start()
        {
            IsActive = true;
        }

        public void Stop()
        {
            IsActive = true;
        }

        internal virtual void Update(GameTime gameTime, Player player)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Timer > Interval)
            {
                OnTickEventArgs args = new OnTickEventArgs
                {
                    Player = player
                };
                OnTickEvent?.Invoke(this, args);
                Timer = 0;
            }
        }

        #endregion
    }
}
