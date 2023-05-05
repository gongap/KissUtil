namespace KissUtil.TaskSources
{
    /// <summary>
    /// Interface IStrongBox
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IStrongBox<T>
    {
        ref T Value { get; }

        bool RunContinuationsAsynchronously { get; set; }
    }
}
