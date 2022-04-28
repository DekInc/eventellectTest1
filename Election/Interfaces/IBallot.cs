using System.Collections.Generic;

namespace Election.Interfaces
{
    public interface IBallot<T> where T : IVote
    {
        IEnumerable<T> Votes { get; }
    }
}
