using RecordingBot.UiTests.Shared.Models;

namespace RecordingBot.UiTests.Shared.Users
{
    public class UserC : Person
    {
        public override string Username { get; set; } = Environment.GetEnvironmentVariable("UserC_Username") ?? throw new Exception("Please provide environment variable UserC_Username");
        public override string Password { get; set; } = Environment.GetEnvironmentVariable("UserC_UserPassword") ?? throw new Exception("Please provide environment variable UserC_UserPassword");
        public override string Seed { get; set; } = Environment.GetEnvironmentVariable("UserC_UserSeed") ?? throw new Exception("Please provide environment variable UserC_UserSeed");
    }
}
