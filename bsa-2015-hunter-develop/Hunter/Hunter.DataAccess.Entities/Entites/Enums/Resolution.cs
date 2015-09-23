using System.ComponentModel;
namespace Hunter.DataAccess.Entities
{
    public enum Resolution
    {
        [Description("None")]
        None,
        [Description("Hired")]
        Hired,
        [Description("Available")]
        Available,
        [Description("Not Now")]
        NotNow,
        [Description("Not Interested")]
        NotInterested,
        [Description("Unfit")]
        Unfit
    }
}