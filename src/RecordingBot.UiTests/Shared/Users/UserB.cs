using RecordingBot.UiTests.Shared.Models;

namespace RecordingBot.UiTests.Shared.Users
{
    public class UserB : Person
    {
        public override string Username { get; set; } = Environment.GetEnvironmentVariable("UserB_Username") ?? throw new Exception("Please provide environment variable UserB_Username");
        public override string Password { get; set; } = Environment.GetEnvironmentVariable("UserB_UserPassword") ?? throw new Exception("Please provide environment variable UserB_UserPassword");
        public override string Seed { get; set; } = Environment.GetEnvironmentVariable("UserB_UserSeed") ?? throw new Exception("Please provide environment variable UserB_UserSeed");
    }
}
