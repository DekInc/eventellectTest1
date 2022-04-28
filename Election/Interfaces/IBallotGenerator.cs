using System.Collections.Generic;

namespace Election.Interfaces
{
    public interface IBallotGenerator<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        List<TBallot> GenerateBallots(IEnumerable<IVoter> voters, IList<ICandidate> candidates);
    }
}
