using System.Threading.Tasks.Sources;

namespace KissUtil.TaskSources
{
    /// <summary>
    /// Class ManualResetValueTaskSource.
    /// </summary>
    public class ManualResetValueTaskSource<T> : IStrongBox<ManualResetValueTaskSourceLogic<T>>, IValueTaskSource<T>,
        IValueTaskSource
    {
        private readonly Action _cancellationCallback;
        private ManualResetValueTaskSourceLogic<T> _logic;

        public ManualResetValueTaskSource(ContinuationOptions options = ContinuationOptions.None)
        {
            _logic = new ManualResetValueTaskSourceLogic<T>(this, options);
            _cancellationCallback = SetCanceled;
        }

        public short Version => _logic.Version;

        public bool RunContinuationsAsynchronously { get; set; } = true;

        ref ManualResetValueTaskSourceLogic<T> IStrongBox<ManualResetValueTaskSourceLogic<T>>.Value => ref _logic;

        void IValueTaskSource.GetResult(short token)
        {
            _logic.GetResult(token);
        }

        public T GetResult(short token)
        {
            return _logic.GetResult(token);
        }

        public ValueTaskSourceStatus GetStatus(short token)
        {
            return _logic.GetStatus(token);
        }

        public void OnCompleted(Action<object> continuation, object state, short token,
            ValueTaskSourceOnCompletedFlags flags)
        {
            _logic.OnCompleted(continuation, state, token, flags);
        }

        public bool SetResult(T result)
        {
            lock (_cancellationCallback)
            {
                if (_logic.Completed)
                {
                    return false;
                }

                _logic.SetResult(result);
                return true;
            }
        }

        public void SetException(Exception error)
        {
            if (Monitor.TryEnter(_cancellationCallback))
            {
                if (_logic.Completed)
                {
                    Monitor.Exit(_cancellationCallback);
                    return;
                }

                _logic.SetException(error);
                Monitor.Exit(_cancellationCallback);
            }
        }

        public void SetCanceled()
        {
            SetException(new TaskCanceledException());
        }


        public ValueTask<T> AwaitValue(CancellationToken cancellation)
        {
            var registration = cancellation == CancellationToken.None
                ? (CancellationTokenRegistration?) null
                : cancellation.Register(_cancellationCallback);
            return _logic.AwaitValue(this, registration);
        }

        public ValueTask AwaitVoid(CancellationToken cancellation)
        {
            var registration = cancellation == CancellationToken.None
                ? (CancellationTokenRegistration?) null
                : cancellation.Register(_cancellationCallback);
            return _logic.AwaitVoid(this, registration);
        }

        public void Reset()
        {
            _logic.Reset();
        }
    }
}
