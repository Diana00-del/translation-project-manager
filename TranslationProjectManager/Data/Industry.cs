namespace TranslationProjectManager.Data;

public enum IndustryEnum
{
    NotSpecified = 0,
    Finance = 1,
    Healthcare = 2,
    Manufacturing = 3,
    Retail = 4,
    Technology = 5,
    Law = 6,
    Patents = 7,
    Media = 8,
    History = 9,
    Education = 10,
    Science = 11,
    Art = 12,
    Other = 13
}

public class Industry
{
    public IndustryEnum Value { get; private set; }
    public string Title { get; private set; }

    private Industry(IndustryEnum value, string title)
    {
        Value = value;
        Title = title;
    }

    public static readonly Industry NotSpecified = new Industry(IndustryEnum.NotSpecified, "Not Specified");
    public static readonly Industry Finance = new Industry(IndustryEnum.Finance, "Finance");
    public static readonly Industry Healthcare = new Industry(IndustryEnum.Healthcare, "Healthcare");
    public static readonly Industry Manufacturing = new Industry(IndustryEnum.Manufacturing, "Manufacturing");
    public static readonly Industry Retail = new Industry(IndustryEnum.Retail, "Retail");
    public static readonly Industry Technology = new Industry(IndustryEnum.Technology, "Technology");
    public static readonly Industry Law = new Industry(IndustryEnum.Law, "Law");
    public static readonly Industry Patents = new Industry(IndustryEnum.Patents, "Patents");
    public static readonly Industry Media = new Industry(IndustryEnum.Media, "Media");
    public static readonly Industry History = new Industry(IndustryEnum.History, "History");
    public static readonly Industry Education = new Industry(IndustryEnum.Education, "Education");
    public static readonly Industry Science = new Industry(IndustryEnum.Science, "Science");
    public static readonly Industry Art = new Industry(IndustryEnum.Art, "Art");
    public static readonly Industry Other = new Industry(IndustryEnum.Other, "Other");

    public static IEnumerable<Industry> All
    {
        get
        {
            return new[]
            {
                    NotSpecified,
                    Finance,
                    Healthcare,
                    Manufacturing,
                    Retail,
                    Technology,
                    Law,
                    Patents,
                    Media,
                    History,
                    Education,
                    Science,
                    Art,
                    Other
                 };
        }
    }

    public static Industry FromValue(IndustryEnum value)
    {
        return All.FirstOrDefault(x => x.Value == value);
    }
}
