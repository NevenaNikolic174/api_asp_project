using System.Collections.Concurrent;

namespace MyProject.Api.Core.Token
{
    public class InMemoryTokenStorage : ITokenStorage
    {
        private static ConcurrentDictionary<Guid, bool> tokens = new ConcurrentDictionary<Guid, bool>();

        public void Add(Guid tokenId)
        {
            var added = tokens.TryAdd(tokenId, true);
            if (!added)
            {
                throw new InvalidOperationException("Token not added to cache.");
            }
        }

        public bool Exists(Guid tokenId)
        {
            return tokens.ContainsKey(tokenId);
        }

        public void Remove(Guid tokenId)
        {
            if (Exists(tokenId))
            {
                var removed = false;
                tokens.TryRemove(tokenId, out removed);

                if (!removed)
                {
                    throw new InvalidOperationException("Token not removed.");
                }
            }
        }
    }
}
