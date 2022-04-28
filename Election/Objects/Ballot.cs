using System.Collections.Generic;

using Election.Interfaces;

namespace Election.Objects
{
    public class SimpleBallot : IBallot<SimpleVote>
    {
        public IEnumerable<SimpleVote> Votes => _votes;
        private List<SimpleVote> _votes;
        public SimpleBallot(SimpleVote vote) { _votes = new List<SimpleVote>() { vote }; }
    }

    public class RankedChoiceBallot : IBallot<RankedChoiceVote>
    {
        public IEnumerable<RankedChoiceVote> Votes { get; private set; }
        public RankedChoiceBallot(IEnumerable<RankedChoiceVote> rankedChoiceVotes) { this.Votes = rankedChoiceVotes; }
    }
}
