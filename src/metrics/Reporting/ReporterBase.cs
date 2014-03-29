using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace metrics.Reporting
{
	/// <summary>
	///  A reporter that periodically prints out formatted application metrics to a specified <see cref="TextWriter" />
	/// </summary>
	public abstract class ReporterBase : IReporter
	{
		protected CancellationTokenSource Token;
		private int Runs { get; set; } 

		/// <summary>
		/// Starts the reporting task for periodic output
		/// </summary>
		/// <param name="period">The period between successive displays</param>
		/// <param name="unit">The period time unit</param>
		public virtual void Start(long period, TimeUnit unit)
		{
			var seconds = unit.Convert(period, TimeUnit.Seconds);
			var interval = TimeSpan.FromSeconds(seconds);

			Token = new CancellationTokenSource();
			Task.Factory.StartNew(async () =>
			{
				OnStarted();
				while (!Token.IsCancellationRequested)
				{
					await Task.Delay(interval, Token.Token);
					if (!Token.IsCancellationRequested)
					{
						Runs++;
						Run();
					}
					
				}
			}, Token.Token);
		}

		public void Stop()
		{
			Token.Cancel();
			OnStopped();
		}

		public event EventHandler<EventArgs> Started;
		public void OnStarted()
		{
			var handler = Started;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> Stopped;
		public void OnStopped()
		{
			var handler = Stopped;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public abstract void Run();

		public virtual void Dispose()
		{
			if (Token != null)
			{
				Token.Cancel();
			}
		}
	}
}
