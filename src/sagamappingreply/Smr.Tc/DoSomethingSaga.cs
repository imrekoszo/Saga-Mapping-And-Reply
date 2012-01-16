using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Saga;
using Smr.Tc.Messages;
using log4net;

namespace Smr.Tc
{
	public class DoSomethingSaga : Saga<DoSomethingSaga.SagaEntity>
		, IAmStartedByMessages<DoSomethingCommand>
		, IHandleMessages<ForwardSagaMessage>
	{
		public class SagaEntity : ISagaEntity
		{
			public virtual Guid Id { get; set; }
			public virtual string Originator { get; set; }
			public virtual string OriginalMessageId { get; set; }
			public virtual Guid SagaId { get; set; }
		}

		private readonly ILog _log;

		public DoSomethingSaga()
		{
			_log = LogManager.GetLogger(GetType());
		}

		public void Handle(DoSomethingCommand message)
		{
			_log.WarnFormat("Handled DoSomethingCommand, message.SagaId={0}, SagaId={1}", message.SagaId, Data.Id);
			Data.SagaId = message.SagaId;

			var forwardSagaMessage = new ForwardSagaMessage {SagaId = Data.Id};
			_log.WarnFormat("Sending ForwardSagaMessage, SagaId={0}", forwardSagaMessage.SagaId);
			Bus.SendLocal(forwardSagaMessage);
		}

		public void Handle(ForwardSagaMessage message)
		{
			_log.WarnFormat("Handled ForwardSagaMessage, message.SagaId={0}, SagaId={1}", message.SagaId, Data.Id);

			var doSomethingResponse = new DoSomethingResponse {SagaId = Data.SagaId};
			_log.WarnFormat("ReplyToOriginator SagaId={0}", doSomethingResponse.SagaId);
			ReplyToOriginator(doSomethingResponse);
		}
	}
}
