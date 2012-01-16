using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using Smr.Tc.Messages;
using log4net;

namespace Smr.Tc
{
	public class DoSomethingElseHandler : IHandleMessages<DoSomethingElseCommand>
	{
		public IBus Bus { get; set; }
		private readonly ILog _log;

		public DoSomethingElseHandler()
		{
			_log = LogManager.GetLogger(GetType());
		}

		public void Handle(DoSomethingElseCommand message)
		{
			_log.WarnFormat("Handled DoSomethingCommand, message.SagaId={0}", message.SagaId);

			var response = new DoSomethingElseResponse {SagaId = message.SagaId};
			_log.WarnFormat("Replying SagaId={0}", response.SagaId);
			Bus.Reply(response);
		}
	}
}
