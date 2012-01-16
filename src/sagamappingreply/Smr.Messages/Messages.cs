using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Saga;

namespace Smr.Messages
{
	public class StartOuterSagaCommand : IMessage
	{
		public Guid AccountId { get; set; }
	}

	public class ForwardMessage : IMessage
	{
		public Guid AccountId { get; set; }
	}
}
