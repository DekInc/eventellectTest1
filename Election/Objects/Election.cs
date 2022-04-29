using System;
using System.Collections.Generic;
using System.Linq;
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
            //Winner = Candidates.First();
            Dictionary<int, VoteResult> numberVotes = new Dictionary<int, VoteResult>();
			foreach (ICandidate candidate in Candidates) {
                numberVotes.Add(candidate.Id, new VoteResult(candidate));
			}
			foreach (SimpleBallot ballot in Ballots) {
                if (ballot.Votes != null) {
                    IVote vote = ballot.Votes.FirstOrDefault();
                    if (vote != null) {
                        numberVotes[vote.Candidate.Id].NumVotes = numberVotes[vote.Candidate.Id].NumVotes + 1;
                    }
                }
			}
            long totalVotes = Ballots.Count();            
            for (int i = 0; i < numberVotes.Count; i++) {
                numberVotes.ElementAt(i).Value.NumVotesPercentage = 1.0f * numberVotes.ElementAt(i).Value.NumVotes / totalVotes;
                Console.WriteLine($"Candidate {numberVotes.ElementAt(i).Key} - {numberVotes.ElementAt(i).Value.Candidate.Name} got {numberVotes.ElementAt(i).Value.NumVotes} votes or {numberVotes.ElementAt(i).Value.NumVotesPercentage * 100:F4} %");
			}
            Winner = numberVotes.OrderByDescending(V => V.Value.NumVotes).FirstOrDefault().Value.Candidate;
        }
    }

    class RankedChoiceElection : Election<RankedChoiceBallot, RankedChoiceVote>
    {
        public RankedChoiceElection(IEnumerable<RankedChoiceBallot> ballots, IEnumerable<ICandidate> candidates) : base(ballots, candidates) { }

        public override void CountVotes()
        {
            //throw new NotImplementedException();
            //Ballots.First().Votes.First().Voter
            Winner = Candidates.First();
            //Dictionary<int, VoteResult> numberVotes = new Dictionary<int, VoteResult>();
            //foreach (ICandidate candidate in Candidates) {
            //    numberVotes.Add(candidate.Id, new VoteResult(candidate));
            //}
            //foreach (RankedChoiceBallot ballot in Ballots) {
            //    if (ballot.Votes != null) {
            //        IVote vote = ballot.Votes.FirstOrDefault();
            //        if (vote != null) {
            //            numberVotes[vote.Candidate.Id].NumVotes = numberVotes[vote.Candidate.Id].NumVotes + 1;
            //        }
            //    }
            //}
            //long totalVotes = Ballots.Count();
            //for (int i = 0; i < numberVotes.Count; i++) {
            //    numberVotes.ElementAt(i).Value.NumVotesPercentage = 1.0f * numberVotes.ElementAt(i).Value.NumVotes / totalVotes;
            //    Console.WriteLine($"Candidate {numberVotes.ElementAt(i).Key} - {numberVotes.ElementAt(i).Value.Candidate.Name} got {numberVotes.ElementAt(i).Value.NumVotes} votes or {numberVotes.ElementAt(i).Value.NumVotesPercentage * 100:F4} %");
            //}
            //Winner = numberVotes.OrderByDescending(V => V.Value.NumVotes).FirstOrDefault().Value.Candidate;
        }
    }
}
