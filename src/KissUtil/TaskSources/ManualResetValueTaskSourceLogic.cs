using System.Runtime.ExceptionServices;
using System.Threading.Tasks.Sources;

namespace KissUtil.TaskSources
{
    /// <summary>
    /// Struct ManualResetValueTaskSourceLogic
    /// </summary>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    internal struct ManualResetValueTaskSourceLogic<TResult>
    {
        private static readonly Action<object> s_sentinel = s => throw new InvalidOperationException();

        private readonly IStrongBox<ManualResetValueTaskSourceLogic<TResult>> _parent;
        private readonly ContinuationOptions _options;
        private Action<object> _continuation;
        private object _continuationState;
        private object _capturedContext;
        private ExecutionContext _executionContext;
        private TResult _result;
        private ExceptionDispatchInfo _error;
        private CancellationTokenRegistration? _registration;

        public ManualResetValueTaskSourceLogic(
            IStrongBox<ManualResetValueTaskSourceLogic<TResult>> parent,
            ContinuationOptions options)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _options = options;
            _continuation = null;
            _continuationState = null;
            _capturedContext = null;
            _executionContext = null;
            Completed = false;
            _result = default;
            _error = null;
            Version = 0;
            _registration = null;
        }

        public short Version { get; private set; }

        public bool Completed { get; private set; }

        /// <summary>
        /// Validates the token.
        /// </summary>
        /// <param name="token">The token.</param>
        private void ValidateToken(short token)
        {
            if (token != Version)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>System.Threading.Tasks.Sources.ValueTaskSourceStatus.</returns>
        public ValueTaskSourceStatus GetStatus(short token)
        {
            ValidateToken(token);

            return
                !Completed ? ValueTaskSourceStatus.Pending :
                _error == null ? ValueTaskSourceStatus.Succeeded :
                _error.SourceException is OperationCanceledException ? ValueTaskSourceStatus.Canceled :
                ValueTaskSourceStatus.Faulted;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TResult.</returns>
        public TResult GetResult(short token)
        {
            ValidateToken(token);

            if (!Completed)
            {
                throw new InvalidOperationException();
            }

            var result = _result;
            var error = _error;
            Reset();

            error?.Throw();
            return result;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Version++;

            _registration?.Dispose();

            Completed = false;
            _continuation = null;
            _continuationState = null;
            _result = default;
            _error = null;
            _executionContext = null;
            _capturedContext = null;
            _registration = null;
        }

        /// <summary>
        /// Called when [completed].
        /// </summary>
        /// <param name="continuation">The continuation.</param>
        /// <param name="state">The state.</param>
        /// <param name="token">The token.</param>
        /// <param name="flags">The flags.</param>
        public void OnCompleted(Action<object> continuation, object state, short token,
            ValueTaskSourceOnCompletedFlags flags)
        {
            if (continuation == null)
            {
                throw new ArgumentNullException(nameof(continuation));
            }

            ValidateToken(token);


            if ((flags & ValueTaskSourceOnCompletedFlags.FlowExecutionContext) != 0)
            {
                _executionContext = ExecutionContext.Capture();
            }

            if ((flags & ValueTaskSourceOnCompletedFlags.UseSchedulingContext) != 0)
            {
                var sc = SynchronizationContext.Current;
                if (sc != null && sc.GetType() != typeof(SynchronizationContext))
                {
                    _capturedContext = sc;
                }
                else
                {
                    var ts = TaskScheduler.Current;
                    if (ts != TaskScheduler.Default)
                    {
                        _capturedContext = ts;
                    }
                }
            }

            _continuationState = state;
            if (Interlocked.CompareExchange(ref _continuation, continuation, null) != null)
            {
                _executionContext = null;

                var cc = _capturedContext;
                _capturedContext = null;

                switch (cc)
                {
                    case null:
                        Task.Factory.StartNew(continuation, state, CancellationToken.None,
                            TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                        break;

                    case SynchronizationContext sc:
                        sc.Post(
                            s =>
                            {
                                var tuple = (Tuple<Action<object>, object>)s;
                                tuple.Item1(tuple.Item2);
                            }, Tuple.Create(continuation, state));
                        break;

                    case TaskScheduler ts:
                        Task.Factory.StartNew(continuation, state, CancellationToken.None,
                            TaskCreationOptions.DenyChildAttach, ts);
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void SetResult(TResult result)
        {
            _result = result;
            SignalCompletion();
        }

        /// <summary>
        /// Sets the exception.
        /// </summary>
        /// <param name="error">The error.</param>
        public void SetException(Exception error)
        {
            _error = ExceptionDispatchInfo.Capture(error);
            SignalCompletion();
        }

        /// <summary>
        /// Signals the completion.
        /// </summary>
        private void SignalCompletion()
        {
            if (Completed)
            {
                throw new InvalidOperationException("Double completion of completion source is prohibited");
            }

            Completed = true;

            if (Interlocked.CompareExchange(ref _continuation, s_sentinel, null) != null)
            {
                if (_executionContext != null)
                {
                    ExecutionContext.Run(
                        _executionContext,
                        s => ((IStrongBox<ManualResetValueTaskSourceLogic<TResult>>)s)?.Value.InvokeContinuation(),
                        _parent ?? throw new InvalidOperationException());
                }
                else
                {
                    InvokeContinuation();
                }
            }
        }

        /// <summary>
        /// Invokes the continuation.
        /// </summary>
        private void InvokeContinuation()
        {
            var cc = _capturedContext;
            _capturedContext = null;

            if (_options == ContinuationOptions.ForceDefaultTaskScheduler)
            {
                cc = TaskScheduler.Default;
            }

            switch (cc)
            {
                case null:
                    if (_parent.RunContinuationsAsynchronously)
                    {
                        var c = _continuation;
                        if (_executionContext != null)
                        {
                            ThreadPool.QueueUserWorkItem(s => c(s), _continuationState);
                        }
                        else
                        {
                            ThreadPool.UnsafeQueueUserWorkItem(s => c(s), _continuationState);
                        }
                    }
                    else
                    {
                        _continuation(_continuationState);
                    }

                    break;

                case SynchronizationContext sc:
                    sc.Post(
                        s =>
                        {
                            ref var logicRef = ref ((IStrongBox<ManualResetValueTaskSourceLogic<TResult>>)s).Value;
                            logicRef._continuation(logicRef._continuationState);
                        }, _parent ?? throw new InvalidOperationException());
                    break;

                case TaskScheduler ts:
                    Task.Factory.StartNew(_continuation, _continuationState, CancellationToken.None,
                        TaskCreationOptions.DenyChildAttach, ts);
                    break;
            }
        }

        /// <summary>
        /// Awaits the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>System.Threading.Tasks.ValueTask&lt;T&gt;.</returns>
        public ValueTask<T> AwaitValue<T>(IValueTaskSource<T> source, CancellationTokenRegistration? registration)
        {
            _registration = registration;
            return new ValueTask<T>(source, Version);
        }

        /// <summary>
        /// Awaits the void.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>System.Threading.Tasks.ValueTask.</returns>
        public ValueTask AwaitVoid(IValueTaskSource source, CancellationTokenRegistration? registration)
        {
            _registration = registration;
            return new ValueTask(source, Version);
        }
    }
}
