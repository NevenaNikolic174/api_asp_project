namespace MyProject.Api.Core.Token
{
    public interface ITokenStorage
    {
        bool Exists(Guid tokenId);
        void Add(Guid tokenId);
        void Remove(Guid tokenId);
    }
}
