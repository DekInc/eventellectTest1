using Election.Interfaces;
using Election.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TestElection {
	[TestClass]
	public class RedundancyTests {
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
		[TestMethod]
		public void SimpleSimpleElectionRedundancy() {
			int numVoters = 3200;
			int totalVoters = numVoters + _candidates.Count;
			List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
			Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
			Random _random = new Random();
			SimpleBallotGenerator simpleVoteGenerator = new SimpleBallotGenerator(_random);
			simpleBallots = simpleVoteGenerator.GenerateBallots(voters, _candidates);
			SimpleElection simpleElection = new SimpleElection(simpleBallots, _candidates);
			try {
				string Json = JsonConvert.SerializeObject(simpleElection);
			} catch (Exception E) {
				Assert.Fail(E.Message);
			}
		}
		[TestMethod]
		public void RCVElectionRedundancy() {
			int numVoters = 3200;
			int totalVoters = numVoters + _candidates.Count;
			List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
			Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
			Random _random = new Random();
			RankedChoiceBallotGenerator rankedChoiceVoteGenerator = new RankedChoiceBallotGenerator(_random);
			List<RankedChoiceBallot> rankedChoiceBallots = rankedChoiceVoteGenerator.GenerateBallots(voters, _candidates, simpleBallots);
			RankedChoiceElection rankedChoiceElection = new RankedChoiceElection(rankedChoiceBallots, _candidates);

			try {
				string Json = JsonConvert.SerializeObject(rankedChoiceElection);
			} catch (Exception E) {
				Assert.Fail(E.Message);
			}
		}
	}
}
