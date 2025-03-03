using Microsoft.Playwright;
using RecordingBot.UiTests.Shared.Models;
using RecordingBot.UiTests.PageObjects.Login.Page;
using RecordingBot.UiTests.PageObjects.Teams.Page;

namespace RecordingBot.UiTests.PageObjects.Teams.Steps
{
    public class TeamsSteps
    {
        public static async Task SearchPersonAndOpenChat(IPage page, Person person)
        {
            await page.WaitForSelectorAsync(TeamsPage.Search);
            var personInput = page.Locator(TeamsPage.Search);

            if (await personInput.IsVisibleAsync() && !string.IsNullOrWhiteSpace(person.Username))
            {
                await personInput.ClickAsync();
                await personInput.FillAsync(person.Username);
                await personInput.PressAsync("Enter");
            }

            await page.WaitForSelectorAsync(SearchPage.TabBarPeople);
            await page.ClickAsync(SearchPage.TabBarPeople);
            await page.WaitForSelectorAsync(SearchPage.ContentAreaPerson);
            await page.ClickAsync(SearchPage.ContentAreaPerson);
        }
    }
}
