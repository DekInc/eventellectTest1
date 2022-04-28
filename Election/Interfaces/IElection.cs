using System.Collections.Generic;

namespace Election.Interfaces
{
    public interface IElection<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        public IEnumerable<TBallot> Ballots { get; }
        public IEnumerable<ICandidate> Candidates { get; }
        public ICandidate Winner { get; }
        void CountVotes();
    }
}
