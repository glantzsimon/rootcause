namespace K9.DataAccessLayer.Models
{
    public interface IScorableBase
    {
        int Score { get; set; }
        int RelativeScore { get; set; }
    }
}
