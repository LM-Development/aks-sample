namespace RecordingBot.UiTests.Shared.Models
{
    public abstract class Person
    {
        /// <summary>
        /// Username that is used to login
        /// </summary>
        /// <remarks>Example: Max.Mustermann@musterpage.com</remarks>
        public abstract string Username { get; set; }
        /// <summary>
        /// Password that is used to login
        /// </summary>
        /// <remarks>Example: YourPassword</remarks>
        public abstract string Password { get; set; }

        /// <summary>
        /// Seed that is used to calculate token for login when 2fa is enabled
        /// </summary>
        /// <remarks>Example: YourSeed</remarks>
        public abstract string Seed { get; set; }
    }
}
