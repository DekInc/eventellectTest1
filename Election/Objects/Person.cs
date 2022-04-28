using Election.Interfaces;

namespace Election.Objects
{
    public abstract class Person : IPerson
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Person(int id, string name) { this.Id = id; this.Name = name; }
    }

    public class SimpleVoter : Person, IVoter
    {
        public SimpleVoter(int id, string name) : base(id, name) { }
    }

    public class SimpleCandidate : Person, ICandidate, IVoter
    {
        public SimpleCandidate(int id, string name) : base(id, name) { }
    }
}
