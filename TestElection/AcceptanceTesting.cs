using Microsoft.VisualStudio.TestTools.UnitTesting;
using Election;
using System;
using System.Collections.Generic;
using System.Text;
using Election.Interfaces;
using Election.Objects;
using System.Linq;
namespace Election.Tests {
	[TestClass()]
	public class AcceptanceTesting {
		static List<ICandidate> _candidates = new List<ICandidate>
			{
			new SimpleCandidate(10001, "Eric Adams"),
			new SimpleCandidate(10002, "Shaun Donovan"),
			new SimpleCandidate(10003, "Kathryn Garcia"),
			new SimpleCandidate(10004, "Raymond McGuire"),
			new SimpleCandidate(10005, "Dianne Morales"),
			new SimpleCandidate(10006, "Scott Stringer"),
			new SimpleCandidate(10007, "Maya Wiley"),
			new SimpleCandidate(10008, "Andrew Yang")
		};
		[TestMethod()]
		public void AcceptRCVMethoTest() {
			//Test if code can manage candidate final 2 results of 50% - 50% 
			int numVoters = 2;
			int totalVoters = numVoters + _candidates.Count;
			List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
			Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);

			List<SimpleVote> all10Votes = new List<SimpleVote>(simpleBallots.FirstOrDefault().Votes);
			ICandidate candidate1 = simpleBallots.FirstOrDefault().Votes.FirstOrDefault().Candidate;
			ICandidate candidate2 = simpleBallots.FirstOrDefault().Votes.LastOrDefault().Candidate;
			List<SimpleVote> newAll10Votes = new List<SimpleVote>();
			List<SimpleBallot> newAll10Ballots = new List<SimpleBallot>();
			newAll10Votes.Add(new SimpleVote(simpleBallots[0].Votes.FirstOrDefault().Voter, candidate1));
			newAll10Votes.Add(new SimpleVote(simpleBallots[1].Votes.FirstOrDefault().Voter, candidate1));
			newAll10Votes.Add(new SimpleVote(simpleBallots[2].Votes.FirstOrDefault().Voter, candidate1));
			newAll10Votes.Add(new SimpleVote(simpleBallots[3].Votes.FirstOrDefault().Voter, candidate1));
			newAll10Votes.Add(new SimpleVote(simpleBallots[4].Votes.FirstOrDefault().Voter, candidate1));

			newAll10Votes.Add(new SimpleVote(simpleBallots[5].Votes.FirstOrDefault().Voter, candidate2));
			newAll10Votes.Add(new SimpleVote(simpleBallots[6].Votes.FirstOrDefault().Voter, candidate2));
			newAll10Votes.Add(new SimpleVote(simpleBallots[7].Votes.FirstOrDefault().Voter, candidate2));
			newAll10Votes.Add(new SimpleVote(simpleBallots[8].Votes.FirstOrDefault().Voter, candidate2));
			newAll10Votes.Add(new SimpleVote(simpleBallots[9].Votes.FirstOrDefault().Voter, candidate2));
			foreach (SimpleVote vote in newAll10Votes) {
				newAll10Ballots.Add(new SimpleBallot(vote));
			}
			ICandidate thisWinner = Election.Program.RunRankedChoiceElection(voters, newAll10Ballots);
			Assert.AreEqual(thisWinner.Id, candidate1.Id);
		}
	}
}