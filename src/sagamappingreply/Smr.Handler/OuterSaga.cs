using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Saga;
using Smr.Messages;
using Smr.Tc.Messages;
using log4net;

namespace Smr.Handler
{
	public class OuterSaga : Saga<OuterSaga.SagaEntity>
		, IAmStartedByMessages<StartOuterSagaCommand>
		, IHandleMessages<DoSomethingResponse>
		, IHandleMessages<DoSomethingElseResponse>
		, IHandleMessages<ForwardMessage>
	{
		public class SagaEntity : ISagaEntity
		{
			public virtual Guid Id { get; set; }
			public virtual string Originator { get; set; }
			public virtual string OriginalMessageId { get; set; }
			public virtual Guid AccountId { get; set; }
		}

		private readonly ILog _log;

		public OuterSaga()
		{
			_log = LogManager.GetLogger(GetType());
		}

		public override void ConfigureHowToFindSaga()
		{
			base.ConfigureHowToFindSaga();
			ConfigureMapping<ForwardMessage>(e => e.AccountId, m => m.AccountId);
		}

		public void Handle(StartOuterSagaCommand message)
		{
			Data.AccountId = message.AccountId;
			_log.WarnFormat("Handled StartOuterSagaCommand, AccountId={0}, SagaId={1}", message.AccountId, Data.Id);

			var forwardMessage = new ForwardMessage {AccountId = message.AccountId};
			_log.WarnFormat("Sending ForwardMessage, AccountId={0}", forwardMessage.AccountId);
			Bus.SendLocal(forwardMessage);
		}

		public void Handle(DoSomethingResponse message)
		{
			_log.WarnFormat("Handled DoSomethingResponse, message.SagaId={0}, SagaId={1}", message.SagaId, Data.Id);
		}

		public void Handle(DoSomethingElseResponse message)
		{
			_log.WarnFormat("Handled DoSomethingElseResponse, message.SagaId={0}, SagaId={1}", message.SagaId, Data.Id);
		}

		public void Handle(ForwardMessage message)
		{
			_log.WarnFormat("Handled ForwardMessage, AccountId={0}, SagaId={1}", message.AccountId, Data.Id);

			var doSomethingCommand = new DoSomethingCommand {SagaId = Data.Id};
			_log.WarnFormat("Sending DoSomethingCommand, SagaId={0}", doSomethingCommand.SagaId);
			Bus.Send(doSomethingCommand);

			var doSomethingElseCommand = new DoSomethingElseCommand {SagaId = Data.Id};
			_log.WarnFormat("Sending DoSomethingElseCommand, SagaId={0}", doSomethingElseCommand.SagaId);
			Bus.Send(doSomethingElseCommand);
		}
	}
}
