
namespace Hunter.DataAccess.Entities.Enums
{
    public enum Status
    {
        Draft, // - is used when the vacancy is not yet opened.
        Open, // - we can add new candidates, publish\post active landings in future
        OnHold, // - vacancy is not relevant AT THE MOMENT. But will be IN SOME TIME. When vacancy is on hold we still can add candidates, feedbacks etc.
        Filled, // - candidate has been hired and the vacancy is officially closed VACANCY MOVES TO ARCHIVE
        Cancelled // - vacancy is no longer valid and not needed any more, no one is hired. VACANCY MOVES TO ARCHIVE
    }
}
