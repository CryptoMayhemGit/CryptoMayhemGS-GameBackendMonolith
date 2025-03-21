namespace Mayhem.Nft.Worker.Definitions
{
    public class SqlQuerries
    {
        public const string GetVoteCategoryIdByInvestorCategory = "SELECT Id FROM VoteCategories WHERE Name = @InvestorCategory";
        public const string AddGameUser = "INSERT INTO GameUser (Wallet, VoteCategoryId) VALUES (@Wallet, @VoteCategoryId)";
        public const string DeleteAllGameUsersByInvestorCategory = "DELETE FROM GameUser WHERE VoteCategoryId = (SELECT Id FROM VoteCategories WHERE Name = @InvestorCategory)";
    }
}