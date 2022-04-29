using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using Election.Interfaces;
using Election.Objects;

namespace Election
{
    class Program
    {
        static Random _random = new Random();
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

        static void Main(string[] _)
        {
            int numVoters = 100000;
            int totalVoters = numVoters + _candidates.Count;
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<IVoter> voters = GenerateVoters(numVoters, totalVoters);
            stopwatch.Stop();
            Console.WriteLine($"Voters: {numVoters}");
            Console.WriteLine($"GenerateVoters exec time: {stopwatch.ElapsedMilliseconds} ms");
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            RunSimpleElection(voters, out List<SimpleBallot> simpleBallots);
            stopwatch2.Stop();
            Console.WriteLine($"RunSimpleElection: {stopwatch2.ElapsedMilliseconds} ms");
            Stopwatch stopwatch3 = Stopwatch.StartNew();
            RunRankedChoiceElection(voters, simpleBallots);
            stopwatch3.Stop();
            Console.WriteLine($"RunRankedChoiceElection: {stopwatch3.ElapsedMilliseconds} ms");
            Console.WriteLine("End. Press ENTER");
            Console.ReadLine();
        }

        private static List<IVoter> GenerateVoters(int numVoters, int totalVoters)
        {
            string voterFormat = $"{{0:{Regex.Replace(totalVoters.ToString(), "[1-9]", "0")}}}";
            List<IVoter> voters = new List<IVoter>(totalVoters);
            voters.AddRange(_candidates.Cast<IVoter>());
            int maxCandidateId = _candidates.Max(c => c.Id);
            for (int i = 1; i <= numVoters; i++)
            {
                int voterId = maxCandidateId + i;
                IVoter voter = new SimpleVoter(voterId, $"Voter {string.Format(voterFormat, voterId)}");
                voters.Add(voter);
            }

            return voters;
        }

        private static void RunSimpleElection(List<IVoter> voters, out List<SimpleBallot> simpleBallots)
        {
            SimpleBallotGenerator simpleVoteGenerator = new SimpleBallotGenerator(_random);
            simpleBallots = simpleVoteGenerator.GenerateBallots(voters, _candidates);
            SimpleElection simpleElection = new SimpleElection(simpleBallots, _candidates);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ICandidate simpleWinner = new SimpleElectionRunner().RunElection(simpleElection);
            stopwatch.Stop();
            Console.WriteLine($"The simple majority winner is {simpleWinner.Name}. RunElection Exec time: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static void RunRankedChoiceElection(List<IVoter> voters, List<SimpleBallot> simpleBallots = null)
        {
            RankedChoiceBallotGenerator rankedChoiceVoteGenerator = new RankedChoiceBallotGenerator(_random);
            List<RankedChoiceBallot> rankedChoiceBallots = rankedChoiceVoteGenerator.GenerateBallots(voters, _candidates, simpleBallots);
            RankedChoiceElection rankedChoiceElection = new RankedChoiceElection(rankedChoiceBallots, _candidates);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ICandidate rankedChoiceWinner = new RankedChoiceElectionRunner().RunElection(rankedChoiceElection);
            stopwatch.Stop();
            Console.WriteLine($"The ranked choice winner is {rankedChoiceWinner.Name}. RunElection Exec time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
