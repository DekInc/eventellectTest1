using Election.Interfaces;

namespace Election.Objects {
    public class VoteResult {
        public ICandidate Candidate { get; private set; }
		public long NumVotes { get; set; }
        public double NumVotesPercentage { get; set; }
		public bool Eliminated { get; set; }
        public VoteResult(ICandidate candidate) {
            this.Candidate = candidate;
        }
    }
}
