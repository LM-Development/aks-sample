using Microsoft.Playwright;
using RecordingBot.UiTests.Shared.Models;
using RecordingBot.UiTests.PageObjects.Login.Page;
using OtpNet;

namespace RecordingBot.UiTests.PageObjects.Login.Steps
{
    public class LoginSteps
    {
        public static async Task LoginPerson(IPage page, Person person)
        {
            await page.GotoAsync("https://teams.microsoft.com/");

            if (!string.IsNullOrWhiteSpace(person.Username))
            {
                await page.WaitForSelectorAsync(LoginPage.UsernameInput);
                var usernameInput = page.Locator(LoginPage.UsernameInput);
                await usernameInput.ClickAsync();
                await usernameInput.FillAsync(person.Username);
                await page.Locator(LoginPage.SubmitBtn).ClickAsync();
            }

            if (!string.IsNullOrWhiteSpace(person.Password))
            {
                await page.WaitForSelectorAsync(LoginPage.PasswordInput);
                var passwordInput = page.Locator(LoginPage.PasswordInput);
                await passwordInput.ClickAsync();
                await passwordInput.FillAsync(person.Password);
                await page.Locator(LoginPage.SubmitBtn).ClickAsync();
            }

            if (!string.IsNullOrWhiteSpace(person.Seed))
            {
                await page.WaitForSelectorAsync(LoginPage.TokenInput);
                var tokenInput = page.Locator(LoginPage.TokenInput);

                var totp = new Totp(Base32Encoding.ToBytes(person.Seed), totpSize: 6 );
                string otpCode = totp.ComputeTotp();

                if (await tokenInput.IsVisibleAsync() && !string.IsNullOrWhiteSpace(otpCode))
                {
                    await tokenInput.ClickAsync();
                    await tokenInput.FillAsync(otpCode);
                    await page.Locator(LoginPage.TokenSubmitBtn).ClickAsync();
                }
            }
        }
    }
}
