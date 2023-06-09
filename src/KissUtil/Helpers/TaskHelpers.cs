namespace KissUtil.Helpers
{
    /// <summary>
    /// 任务助手.
    /// </summary>
    public static class TaskHelpers
    {
        private static readonly Task _defaultCompleted = Task.FromResult(default(AsyncVoid));
        private static readonly Task<object> _completedTaskReturningNull = Task.FromResult<object>(null);

        /// <summary>
        /// 取消此实例.
        /// </summary>
        /// <returns>Task.</returns>
        public static Task Canceled()
        {
            return CancelCache<AsyncVoid>.Canceled;
        }

        /// <summary>
        /// 取消此实例.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public static Task<TResult> Canceled<TResult>()
        {
            return CancelCache<TResult>.Canceled;
        }

        /// <summary>
        /// 完成此实例.
        /// </summary>
        /// <returns>Task.</returns>
        public static Task Completed()
        {
            return _defaultCompleted;
        }

        /// <summary>
        /// 形成错误.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Task.</returns>
        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        /// <summary>
        /// 形成错误.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        /// <summary>
        /// 返回空结果
        /// </summary>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public static Task<object> NullResult()
        {
            return _completedTaskReturningNull;
        }

        /// <summary>
        /// Struct AsyncVoid
        /// </summary>
        private struct AsyncVoid
        {
        }

        /// <summary>
        /// 取消缓存.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        private static class CancelCache<TResult>
        {
            /// <summary>
            /// The canceled
            /// </summary>
            public static readonly Task<TResult> Canceled = GetCancelledTask();

            private static Task<TResult> GetCancelledTask()
            {
                var tcs = new TaskCompletionSource<TResult>();
                tcs.SetCanceled();
                return tcs.Task;
            }
        }
    }
}
