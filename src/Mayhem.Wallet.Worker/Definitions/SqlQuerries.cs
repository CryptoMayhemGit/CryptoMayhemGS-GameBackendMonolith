namespace Mayhem.Wallet.Worker.Definitions
{
    public static class SqlQuerries
    {
        public const string GetTopOneBlockWhereeBlobkTypeIdSql = "SELECT TOP 1 * FROM dbo.GameUserBlocks;";
        public const string UpdateBlockWhereLasBlockSql = "UPDATE dbo.GameUserBlocks set LastBlock = @LastBlock, LastModificationDate = GETUTCDATE()";
        public static string AddWalletSql = "INSERT INTO dbo.GameUser (Wallet, VoteCategoryId, UsdcAmount) VALUES (@Wallet, @VoteCategoryId, @UsdcAmount);";
        public const string GetVoteCategoryIdByInvestorCategory = "SELECT Id FROM VoteCategories WHERE Name = @InvestorCategory";
    }
}
