namespace Doods.Framework.Repository.Std.Interfaces
{
    public interface ISqLiteFactory
    {
        string GetDatabasePath(string fileName);
    }
}