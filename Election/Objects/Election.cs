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
        public void InitVotesPerCandidate(Dictionary<int, VoteResult> votesPerCandidate) {
            foreach (ICandidate candidate in Candidates) {
                votesPerCandidate.Add(candidate.Id, new VoteResult(candidate));
            }
        }
		public ICandidate GetWinner(Dictionary<int, VoteResult> votesPerCandidate) {
			return votesPerCandidate.OrderByDescending(V => V.Value.NumVotes).FirstOrDefault().Value.Candidate;
		}
		public void ShowListOfVotes(Dictionary<int, VoteResult> votesPerCandidate, long totalVotes) {
			for (int i = 0; i < votesPerCandidate.Count; i++) {
				votesPerCandidate.ElementAt(i).Value.NumVotesPercentage = 1.0f * votesPerCandidate.ElementAt(i).Value.NumVotes / totalVotes;
				Console.WriteLine($"Candidate {votesPerCandidate.ElementAt(i).Key} - {votesPerCandidate.ElementAt(i).Value.Candidate.Name} got {votesPerCandidate.ElementAt(i).Value.NumVotes} votes or {votesPerCandidate.ElementAt(i).Value.NumVotesPercentage * 100:F4} % {(votesPerCandidate.ElementAt(i).Value.Eliminated ? "- Eliminated -" : "")}");
			}
		}
		public abstract void CountVotes();
    }

    public class SimpleElection : Election<SimpleBallot, SimpleVote>
    {
        public SimpleElection(IEnumerable<SimpleBallot> ballots, IEnumerable<ICandidate> candidates) : base(ballots, candidates) { }

        public override void CountVotes() {
			//Winner = Candidates.First();
			Dictionary<int, VoteResult> VotesPerCandidate = new Dictionary<int, VoteResult>();
			InitVotesPerCandidate(VotesPerCandidate);
			long totalVotes = Ballots.Count();
			foreach (SimpleBallot ballot in Ballots) {
				if (ballot.Votes != null) {
					SimpleVote vote = ballot.Votes.FirstOrDefault();
					if (vote != null) {
						VotesPerCandidate[vote.Candidate.Id].NumVotes = VotesPerCandidate[vote.Candidate.Id].NumVotes + 1;
					}
				}
			}
			for (int i = 0; i < VotesPerCandidate.Count; i++) {
				VotesPerCandidate.ElementAt(i).Value.NumVotesPercentage = 1.0f * VotesPerCandidate.ElementAt(i).Value.NumVotes / totalVotes;
				Console.WriteLine($"Candidate {VotesPerCandidate.ElementAt(i).Key} - {VotesPerCandidate.ElementAt(i).Value.Candidate.Name} got {VotesPerCandidate.ElementAt(i).Value.NumVotes} votes or {VotesPerCandidate.ElementAt(i).Value.NumVotesPercentage * 100:F4} %");
			}
			Winner = GetWinner(VotesPerCandidate);
		}
	}

	public class RankedChoiceElection : Election<RankedChoiceBallot, RankedChoiceVote>
    {
        public RankedChoiceElection(IEnumerable<RankedChoiceBallot> ballots, IEnumerable<ICandidate> candidates) : base(ballots, candidates) { }

        public override void CountVotes() {
			Dictionary<int, VoteResult> VotesPerCandidate = new Dictionary<int, VoteResult>();
			InitVotesPerCandidate(VotesPerCandidate);
			long totalVotes = Ballots.Count();
			long totalVotesToWin = (totalVotes / 2) + 1;
			int totalCandidates = Candidates.Count();
			while (VotesPerCandidate.Where(Cl => !Cl.Value.Eliminated).Count() >= 2) {
				foreach (RankedChoiceBallot ballot in Ballots) {
					if (ballot.Votes != null) {
						RankedChoiceVote vote = ballot.Votes.FirstOrDefault();
						if (vote != null) {
							if (!VotesPerCandidate[vote.CandidatePreferences.FirstOrDefault()].Eliminated)
								VotesPerCandidate[vote.CandidatePreferences.FirstOrDefault()].NumVotes = VotesPerCandidate[vote.CandidatePreferences.FirstOrDefault()].NumVotes + 1;
						}
					}
				}
				Console.WriteLine($"Rank # {totalCandidates - VotesPerCandidate.Where(Vpc => !Vpc.Value.Eliminated).Count() + 1}");
				totalVotes = VotesPerCandidate.Sum(Vs => Vs.Value.NumVotes);
				totalVotesToWin = (totalVotes / 2) + 1;
				ShowListOfVotes(VotesPerCandidate, totalVotes);
				ICandidate eliminated = VotesPerCandidate.Where(Vd => !Vd.Value.Eliminated).OrderByDescending(V => V.Value.NumVotes).LastOrDefault().Value.Candidate;
				VotesPerCandidate[eliminated.Id].Eliminated = true;
				foreach (RankedChoiceBallot ballot in Ballots) {
					if (ballot.Votes != null) {
						RankedChoiceVote vote = ballot.Votes.FirstOrDefault();
						if (vote != null) {
							if (vote.CandidatePreferences.FirstOrDefault() == eliminated.Id)
								vote.EliminateOne();
						}
					}
				}
				if (VotesPerCandidate.Where(Cv => Cv.Value.NumVotes >= totalVotesToWin).Count() > 0) break;
				for (int i = 0; i < VotesPerCandidate.Count; i++) {
					VotesPerCandidate.ElementAt(i).Value.NumVotes = 0;
					VotesPerCandidate.ElementAt(i).Value.NumVotesPercentage = 0;
				}
			}
			Winner = GetWinner(VotesPerCandidate);
		}
	}
}
