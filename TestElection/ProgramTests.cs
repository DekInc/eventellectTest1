using Microsoft.VisualStudio.TestTools.UnitTesting;
using Election;
using System;
using System.Collections.Generic;
using System.Text;
using Election.Interfaces;
using Election.Objects;

namespace Election.Tests {
	[TestClass()]
	public class ProgramTests {
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
		public void GenerateVotersTest10000() {
            int numVoters = 10000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
			Assert.AreEqual(totalVoters, voters.Count);
		}
        [TestMethod()]
        public void GenerateVotersTest50000() {
            int numVoters = 50000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Assert.AreEqual(totalVoters, voters.Count);
        }
        [TestMethod()]
        public void GenerateVotersTest100000() {
            int numVoters = 100000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Assert.AreEqual(totalVoters, voters.Count);
        }
        [TestMethod()]
        public void GenerateVotersTest0() {
            int numVoters = 0;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Assert.AreEqual(totalVoters, voters.Count);
        }
        [TestMethod()]
        public void RunSimpleElection100000() {
            int numVoters = 100000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
        }
        [TestMethod()]
        public void RunSimpleElection1000000() {
            int numVoters = 1000000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
        }
        [TestMethod()]
        public void RunSimpleElection10000000() {
            int numVoters = 10000000;
            int totalVoters = numVoters + _candidates.Count;
            List<IVoter> voters = Election.Program.GenerateVoters(numVoters, totalVoters);
            Election.Program.RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
        }
    }
}