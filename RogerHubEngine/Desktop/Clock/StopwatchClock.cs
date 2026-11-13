using System.Diagnostics;

namespace RogerHubEngine.Desktop.Clock
{
	/// <summary>
	/// Measures elapsed time using a stopwatch.
	/// </summary>
	public class Clock : IClock
	{
		private readonly Stopwatch clock = new();

		/// <inheritdoc/>
		public long CurrentTime
		{
			get
			{
				return clock.ElapsedMilliseconds;
			}
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
