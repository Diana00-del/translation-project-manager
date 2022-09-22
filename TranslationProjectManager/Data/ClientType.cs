namespace TranslationProjectManager.Data;

public enum ClientTypeEnum
{
    Individual = 1,
    Corporate = 2,
    Institutional = 3,
    Other = 4
}

public class ClientType
{
    public ClientTypeEnum Value { get; private set; }
    public string Title { get; set; }

    private ClientType(ClientTypeEnum value, string title)
    {
        Value = value;
        Title = title;
    }

    public static readonly ClientType Individual = new ClientType(ClientTypeEnum.Individual, "Individual");
    public static readonly ClientType Corporate = new ClientType(ClientTypeEnum.Corporate, "Corporate");
    public static readonly ClientType Institutional = new ClientType(ClientTypeEnum.Institutional, "Institutional");
    public static readonly ClientType Other = new ClientType(ClientTypeEnum.Other, "Other");

    public static IEnumerable<ClientType> All
    {
        get
        {
            return new[] { Individual, Corporate, Institutional, Other };
        }
    }

    public static ClientType FromValue(ClientTypeEnum value)
    {
        return All.FirstOrDefault(x => x.Value == value);
    }
}
