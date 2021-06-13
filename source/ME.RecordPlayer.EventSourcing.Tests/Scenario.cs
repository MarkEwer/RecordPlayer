using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ME.RecordPlayer.EventSourcing.Tests
{
    public class Scenario : INotifyCompletion
    {
        private Task _task;

        private Scenario()
        {
        }

        public bool IsCompleted => _task?.IsCompleted ?? true;

        public static Scenario Given(Action axiom) => new Scenario().Then(axiom);

        public static Scenario Given(Func<ValueTask> axiomTask) => new Scenario().Then(() => axiomTask().AsTask());

        public Scenario And(Action additionalStepOrAssertion) => Then(additionalStepOrAssertion);

        public Scenario And(Func<ValueTask> additionalStepOrAssertionTask) => Then(() => additionalStepOrAssertionTask().AsTask());

        public Scenario GetAwaiter() => this;

        public Scenario GetResult() => this;

        public Scenario Then(Action assertion)
        {
            Queue(assertion);
            return this;
        }

        public Scenario Then(Func<ValueTask> assertionTask) => Then(() => assertionTask().AsTask());

        public Scenario When(Action step) => Then(step);

        public Scenario When(Func<ValueTask> step) => Then(() => step().AsTask());

        void INotifyCompletion.OnCompleted(Action continuation) => Queue(continuation);

        private void Queue(Action continuation)
        {
            if (_task is null)
            {
                continuation();
                return;
            }
            Queue(() => Task.Run(continuation));
        }

        private void Queue(Func<Task> taskFactory)
        {
            if (_task is null)
            {
                _task = taskFactory();
                return;
            }
            // Capture the previous task in the closure
            Task antecedent = _task;
            _task = Task.Run(async () =>
            {
                await antecedent.ConfigureAwait(false);
                await taskFactory().ConfigureAwait(false);
            });
        }

        private Scenario Then(Func<Task> assertionTask)
        {
            Queue(assertionTask);
            return this;
        }
    }
}
