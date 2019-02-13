namespace SparkEngine.Time
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Player;

    public class OnTickEventArgs : EventArgs
    {
        public Player Player { get; set; }
    }
}
