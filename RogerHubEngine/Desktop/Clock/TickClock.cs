using System.Diagnostics;

namespace RogerHubEngine.Desktop.Clock
{
    /// <summary>
    /// Measures elapsed time using ticks.
    /// </summary>
    public class TickClock : IClock
    {
        private readonly Stopwatch clock = new();

        /// <inheritdoc/>
        public long CurrentTime
        {
            get
            {
                return clock.ElapsedMilliseconds / (TickIntervalSec * 1000L);
            }
        }

        /// <summary>
        /// Gets the number of fully elapsed ticks.
        /// </summary>
        private int TickIntervalSec { get; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="TickClock"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the interval is less than or equal to zero.
        /// </exception>
        public TickClock(int tickIntervalSec = 1)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tickIntervalSec);

            TickIntervalSec = tickIntervalSec;
        }

        /// <inheritdoc/>
        public void Start()
        {
            clock.Start();
        }

        /// <inheritdoc/>
        public void Pause()
        {
            clock.Stop();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            clock.Stop();
            Reset();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            clock.Reset();
        }
    }
}
