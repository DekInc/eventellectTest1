using System;
using System.Collections.Generic;
using System.Linq;

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
        { //80% perf improvement 
            List<RankedChoiceBallot> ballots = new List<RankedChoiceBallot>();
            int[] candidateIds = candidates.Select(C => C.Id).ToArray();
            int totalCandidates = candidateIds.Length;
            //Parallel.ForEach(voters, voter => { // No gain
            foreach (IVoter voter in voters) {
                List<RankedChoiceVote> votes = new List<RankedChoiceVote>();
                int[] candidateIdsShuffled;
                if (voter is SimpleCandidate)
                    candidateIdsShuffled = candidateIds.SkipLast(totalCandidates - 1).Union(candidateIds.Skip(1).OrderBy(X1 => _random.Next())).ToArray();
                else
                    candidateIdsShuffled = candidateIds.OrderBy(X1 => _random.Next()).ToArray();
                votes.Add(new RankedChoiceVote(voter, null, candidateIdsShuffled));
                ballots.Add(new RankedChoiceBallot(votes));
            }
            return ballots;
        }
    }
}
