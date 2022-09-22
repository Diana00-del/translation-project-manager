namespace TranslationProjectManager.Data;

public enum TaskStatusEnum
{
    NotStarted = 1,
    ReadyToStart = 2,
    InProgress = 3,
    ReadyToFinalize = 4,
    Finalized = 5,
    Cancelled = 6
}

public class TaskStatus
{
    public TaskStatusEnum Value { get; private set; }
    public string Title { get; private set; }

    private TaskStatus(TaskStatusEnum value, string title)
    {
        Value = value;
        Title = title;
    }

    public static readonly TaskStatus NotStarted = new TaskStatus(TaskStatusEnum.NotStarted, "Not started");
    public static readonly TaskStatus ReadyToStart = new TaskStatus(TaskStatusEnum.ReadyToStart, "Ready to start");
    public static readonly TaskStatus InProgress = new TaskStatus(TaskStatusEnum.InProgress, "In progress");
    public static readonly TaskStatus ReadyToFinalize = new TaskStatus(TaskStatusEnum.ReadyToFinalize, "Ready to finalize");
    public static readonly TaskStatus Finalized = new TaskStatus(TaskStatusEnum.Finalized, "Finalized");
    public static readonly TaskStatus Canceled = new TaskStatus(TaskStatusEnum.Cancelled, "Canceled");

    public static IEnumerable<TaskStatus> All
    {
        get
        {
            return new[]
            {
                NotStarted,
                ReadyToStart,
                InProgress,
                ReadyToFinalize,
                Finalized,
                Canceled
            };
        }
    }

    public static TaskStatus FromValue(TaskStatusEnum value)
    {
        return All.FirstOrDefault(x => x.Value == value);
    }
}
