using Election.Interfaces;

namespace Election.Objects
{
    public abstract class Vote : IVote
    {
        public IVoter Voter { get; private set; }
        public ICandidate Candidate { get; private set; }
        public Vote(IVoter voter, ICandidate candidate)
        {
            this.Voter = voter;
            this.Candidate = candidate;
        }
    }

    public class SimpleVote : Vote
    {
        public SimpleVote(IVoter voter, ICandidate candidate) : base(voter, candidate) { }
    }

    public class RankedChoiceVote : Vote
    {
        public int Rank { get; set; }
        public RankedChoiceVote(IVoter voter, ICandidate candidate, int rank) : base(voter, candidate)
        {
            this.Rank = rank;
        }
    }
}
