namespace TranslationProjectManager.Data;

public enum TranslatorAssignmentStatusEnum
{
    Awaiting = 1,
    OfferSent = 2,
    OfferRejected = 3,
    Accepted = 4,
    InviteRejected = 5,
    InviteCancelled = 6
}

public class TranslatorAssignmentStatus
{
    public TranslatorAssignmentStatusEnum Value { get; private set; }
    public string Title { get; private set; }

    private TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum value, string title)
    {
        Value = value;
        Title = title;
    }

    public static TranslatorAssignmentStatus Awaiting = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.Awaiting, "Awaiting");
    public static TranslatorAssignmentStatus OfferSent = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.OfferSent, "Offer Sent");
    public static TranslatorAssignmentStatus OfferRejected = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.OfferRejected, "Offer Rejected");
    public static TranslatorAssignmentStatus Accepted = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.Accepted, "Accepted");
    public static TranslatorAssignmentStatus InviteRejected = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.InviteRejected, "Invite Rejected");
    public static TranslatorAssignmentStatus InviteCancelled = new TranslatorAssignmentStatus(TranslatorAssignmentStatusEnum.InviteCancelled, "Invite Cancelled");

    public static IEnumerable<TranslatorAssignmentStatus> All
    {
        get
        {
            return new[]
            {
                Awaiting,
                OfferSent,
                OfferRejected,
                Accepted,
                InviteRejected,
                InviteCancelled
            };
        }
    }

    public static TranslatorAssignmentStatus FromValue(TranslatorAssignmentStatusEnum value)
    {
        return All.FirstOrDefault(x => x.Value == value);
    }
}
