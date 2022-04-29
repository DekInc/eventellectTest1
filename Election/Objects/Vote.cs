using System.Linq;
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
        public int[] CandidatePreferences { get; private set; }
        public RankedChoiceVote(IVoter voter, ICandidate candidate, int[] candidatePreferences) : base(voter, candidate)
        {
            this.CandidatePreferences = candidatePreferences;
        }
        public void EliminateOne(){
            CandidatePreferences = CandidatePreferences.Skip(1).ToArray();
        }
    }
}
