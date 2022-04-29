using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Election.Interfaces;

namespace Election.Objects
{
    public abstract class BallotGenerator<TBallot, TVote> : IBallotGenerator<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        protected Random _random;
        public BallotGenerator(Random random = null)
        {
            _random = random ?? new Random();
        }

        public abstract List<TBallot> GenerateBallots(IEnumerable<IVoter> voters, IList<ICandidate> candidates);
    }


    public class SimpleBallotGenerator : BallotGenerator<SimpleBallot, SimpleVote>
    {
        public SimpleBallotGenerator(Random random = null) : base(random) { }

        public override List<SimpleBallot> GenerateBallots(IEnumerable<IVoter> voters, IList<ICandidate> candidates)
        {
            List<SimpleBallot> ballots = new List<SimpleBallot>();
            int numCandidates = candidates.Count();
            SimpleVote vote;
            foreach (IVoter voter in voters)
            {   
                if (voter is ICandidate candidate)
                    vote = new SimpleVote(voter, candidate);
                else
                    vote = new SimpleVote(voter, candidates[_random.Next(0, numCandidates - 1)]);
                ballots.Add(new SimpleBallot(vote));
            }
            return ballots;
        }
    }

    public class RankedChoiceBallotGenerator : BallotGenerator<RankedChoiceBallot, RankedChoiceVote>
    {
        public RankedChoiceBallotGenerator(Random random = null) : base(random) { }

        public override List<RankedChoiceBallot> GenerateBallots(IEnumerable<IVoter> voters, IList<ICandidate> candidates) => GenerateBallots(voters, candidates, null);
        public List<RankedChoiceBallot> GenerateBallots(IEnumerable<IVoter> voters, IList<ICandidate> candidates, IList<SimpleBallot> simpleBallots)
        {
            int rank;
            Dictionary<int, SimpleVote> simpleBallotByVoterId = simpleBallots?.SelectMany(b => b.Votes).ToDictionary(v => v.Voter.Id) ?? new Dictionary<int, SimpleVote>();
            List<RankedChoiceBallot> ballots = new List<RankedChoiceBallot>();
            SortedList<int, ICandidate> remainingCandidatesSL = new SortedList<int, ICandidate>(candidates.ToDictionary(c => c.Id));
            foreach (IVoter voter in voters) {
				List<RankedChoiceVote> votes = new List<RankedChoiceVote>();
				rank = 1;
                SortedList<int, ICandidate> remainingCandidates = new SortedList<int, ICandidate>(remainingCandidatesSL);
                // use SimpleVote as Rank 1
                if (simpleBallotByVoterId.TryGetValue(voter.Id, out SimpleVote simpleVote)) {
					votes.Add(new RankedChoiceVote(voter, simpleVote.Candidate, 1));
					remainingCandidates.Remove(simpleVote.Candidate.Id);
					rank = 2;
				}

				while (remainingCandidates.Count > 0) {
					ICandidate nextCandidate = remainingCandidates.ElementAt(_random.Next(remainingCandidates.Count - 1)).Value;
					votes.Add(new RankedChoiceVote(voter, nextCandidate, rank++));
					remainingCandidates.Remove(nextCandidate.Id);
				}

				ballots.Add(new RankedChoiceBallot(votes));
			}
			return ballots;
        }
    }
}
