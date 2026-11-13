namespace RogerHubEngine.Desktop.Clock
{
    /// <summary>
    /// Provides an interface for measuring elapsed time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current elapsed time
        /// </summary>
        public long CurrentTime { get; }

        /// <summary>
        /// Starts or resumes the clock
        /// </summary>
        public void Start();
        /// <summary>
        /// Stops the clock and resets elapsed time
        /// </summary>
        public void Stop();
        /// <summary>
        /// Pauses the clock
        /// </summary>
        public void Pause();
        /// <summary>
        /// Resets elapsed time to its initial state
        /// </summary>
        public void Reset();
    }
}
