using System;
using System.Collections.Generic;

using Election.Interfaces;

namespace Election.Objects
{
    public abstract class Election<TBallot, TVote> : IElection<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        public IEnumerable<TBallot> Ballots { get; protected set; }
        public IEnumerable<ICandidate> Candidates { get; protected set; }
        public ICandidate Winner { get; protected set; }

        public Election(IEnumerable<TBallot> ballots, IEnumerable<ICandidate> candidates)
        {
            this.Ballots = ballots;
            this.Candidates = candidates;
        }

        public abstract void CountVotes();
    }

    class SimpleElection : Election<SimpleBallot, SimpleVote>
    {
        public SimpleElection(IEnumerable<SimpleBallot> ballots, IEnumerable<ICandidate> candidates) : base(ballots, candidates) { }

        public override void CountVotes()
        {
            throw new NotImplementedException();
        }
    }

    class RankedChoiceElection : Election<RankedChoiceBallot, RankedChoiceVote>
    {
        public RankedChoiceElection(IEnumerable<RankedChoiceBallot> ballots, IEnumerable<ICandidate> candidates) : base(ballots, candidates) { }

        public override void CountVotes()
        {
            throw new NotImplementedException();
        }
    }
}
