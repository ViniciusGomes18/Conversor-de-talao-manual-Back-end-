namespace DSIN.Business.Models
{
    public sealed class Agent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        private Agent() { }
        public Agent(Guid id, string name, string email, string passwordHash)
        {
            Id = id; Name = name; Email = email; PasswordHash = passwordHash;
        }
    }
}