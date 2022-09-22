namespace TranslationProjectManager.Data;

public enum UnitEnum
{
    Hour = 1,
    CertifiedPage = 2,
    Page = 3,
    ShortPage = 4,
    Sheet = 5,
    Verse = 6,
    Sign = 7,
    Word = 8,
}

public class Unit
{
    public UnitEnum Value { get; private set; }
    public string Title { get; private set; }
    public long CharactersCount { get; private set; }

    public Unit(UnitEnum value, string title, long charactersCount)
    {
        Value = value;
        Title = title;
        CharactersCount = charactersCount;
    }

    public string ToString(long amount)
    {
        if (Value == UnitEnum.Hour)
        {
            return $"{amount} {Title}(s)";
        }
        
        return $"{amount} {Title}(s) - {amount * CharactersCount:0.00} chars";
    }

    public static readonly Unit Hour = new Unit(UnitEnum.Hour, "Hour", 0);
    public static readonly Unit CertifiedPage = new Unit(UnitEnum.CertifiedPage, "Certified Page", 1125);
    public static readonly Unit Page = new Unit(UnitEnum.Page, "Page", 1800);
    public static readonly Unit ShortPage = new Unit(UnitEnum.ShortPage, "Short Page", 1500);
    public static readonly Unit Sheet = new Unit(UnitEnum.Sheet, "Sheet", 200000);
    public static readonly Unit Verse = new Unit(UnitEnum.Verse, "Verse", 60);
    public static readonly Unit Sign = new Unit(UnitEnum.Sign, "Sign", 1);
    public static readonly Unit Word = new Unit(UnitEnum.Word, "Word", 6);

    public static IEnumerable<Unit> All
    {
        get
        {
            return new[]
            {
                Hour,
                CertifiedPage,
                Page,
                ShortPage,
                Sheet,
                Verse,
                Sign,
                Word
            };
        }
    }

    public static Unit FromValue(UnitEnum value)
    {
        return All.FirstOrDefault(x => x.Value == value);
    }
}
