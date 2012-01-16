using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Saga;

namespace Smr.Tc.Messages
{
	public class DoSomethingCommand : IMessage
	{
		public Guid SagaId { get; set; }
	}

	public class DoSomethingResponse : ISagaMessage
	{
		public Guid SagaId { get; set; }
	}

	public class DoSomethingElseCommand : IMessage
	{
		public Guid SagaId { get; set; }
	}

	public class DoSomethingElseResponse : ISagaMessage
	{
		public Guid SagaId { get; set; }
	}

	public class ForwardSagaMessage : ISagaMessage
	{
		public Guid SagaId { get; set; }
	}
}
