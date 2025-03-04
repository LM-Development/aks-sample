using RecordingBot.UiTests.Shared.Models;
using System.Reflection.Metadata.Ecma335;

namespace RecordingBot.UiTests.Shared.Users
{
    public class UserA : Person
    {
        public override string Username { get; set; } = Environment.GetEnvironmentVariable("UserA_Username") ?? throw new Exception("Please provide environment variable UserA_Username");
        public override string Password { get; set; } = Environment.GetEnvironmentVariable("UserA_UserPassword") ?? throw new Exception("Please provide environment variable UserA_UserPassword");
        public override string Seed { get; set; } = Environment.GetEnvironmentVariable("UserA_UserSeed") ?? throw new Exception("Please provide environment variable UserA_UserSeed");
    }
}
