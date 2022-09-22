namespace TranslationProjectManager.Data;

public enum AccuracyEnum
{
    NotSpecified = 0,
    Estimation = 1,
    Accurate = 2
}

public class Accuracy
{
    public AccuracyEnum Value { get; private set; }
    public string Title { get; private set; }

    private Accuracy(AccuracyEnum value, string title)
    {
        Value = value;
        Title = title;
    }

    public static readonly Accuracy NotSpecified = new Accuracy(AccuracyEnum.NotSpecified, "Not Specified");
    public static readonly Accuracy Estimation = new Accuracy(AccuracyEnum.Estimation, "Estimation");
    public static readonly Accuracy Accurate = new Accuracy(AccuracyEnum.Accurate, "Accurate");

    public static IEnumerable<Accuracy> All
    {
        get
        {
            return new[] { NotSpecified, Estimation, Accurate };
        }
    }

    public static Accuracy FromValue(AccuracyEnum value)
    {
        return All.SingleOrDefault(x => x.Value == value);
    }
}
