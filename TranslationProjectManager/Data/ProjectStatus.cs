namespace TranslationProjectManager.Data;

public enum ProjectStatusEnum
{
    Draft = 1,
    ReadyToStart = 2,
    InProgress = 3,
    ReadyToFinalize = 4,
    Finalized = 5,
    Cancelled = 6
}

public class ProjectStatus
{
    public ProjectStatusEnum Value { get; private set; }
    public string Title { get; private set; }

    private ProjectStatus(ProjectStatusEnum status, string title)
    {
        Value = status;
        Title = title;
    }

    public static readonly ProjectStatus Draft = new ProjectStatus(ProjectStatusEnum.Draft, "Draft");
    public static readonly ProjectStatus ReadyToStart = new ProjectStatus(ProjectStatusEnum.ReadyToStart, "Ready to Start");
    public static readonly ProjectStatus InProgress = new ProjectStatus(ProjectStatusEnum.InProgress, "In Progress");
    public static readonly ProjectStatus ReadyToFinalize = new ProjectStatus(ProjectStatusEnum.ReadyToFinalize, "Ready to Finalize");
    public static readonly ProjectStatus Finalized = new ProjectStatus(ProjectStatusEnum.Finalized, "Finalized");
    public static readonly ProjectStatus Canceled = new ProjectStatus(ProjectStatusEnum.Cancelled, "Cancelled");

    public static IEnumerable<ProjectStatus> All
    {
        get
        {
            return new[] { Draft, ReadyToStart, InProgress, ReadyToFinalize, Finalized, Canceled };
        }
    }

    public static ProjectStatus FromValue(ProjectStatusEnum status)
    {
        return All.SingleOrDefault(s => s.Value == status);
    }
}
