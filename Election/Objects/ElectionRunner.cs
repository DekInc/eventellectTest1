using Election.Interfaces;

namespace Election.Objects
{
    public abstract class ElectionRunner<TBallot, TVote> : IElectionRunner<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        public ICandidate RunElection(IElection<TBallot, TVote> election)
        {
            election.CountVotes();
            return election.Winner;
        }
    }

    public class SimpleElectionRunner : ElectionRunner<SimpleBallot, SimpleVote> { }
    public class RankedChoiceElectionRunner : ElectionRunner<RankedChoiceBallot, RankedChoiceVote> { }
}
