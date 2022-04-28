namespace Election.Interfaces
{
    public interface IElectionRunner<TBallot, TVote> where TBallot : IBallot<TVote> where TVote : IVote
    {
        ICandidate RunElection(IElection<TBallot, TVote> election);
    }
}
