using System;
using NServiceBus;
using Smr.Messages;

namespace sagamappingreply
{
	public class Program : IWantToRunAtStartup
	{
		public IBus Bus { get; set; }

		public void Run()
		{
			Console.WriteLine("q and enter to exit, anyting else to start a saga");

			while(Console.ReadLine() != "q")
			{
				var command = new StartOuterSagaCommand {AccountId = Guid.NewGuid()};
				Console.WriteLine("Sending command with AccountId={0}", command.AccountId);
				Bus.Send(command);
			}
		}

		public void Stop()
		{
		}
	}
}
