using Microsoft.Playwright;
using RecordingBot.UiTests.Shared.Models;
using RecordingBot.UiTests.PageObjects.Teams.Page;

namespace RecordingBot.UiTests.PageObjects.Teams.Steps
{
    public class TeamsSteps
    {
        public static async Task SearchPersonAndOpenChat(IPage page, Person person)
        {
            var searchInput = page.Locator(TeamsPage.Search);

            if (!string.IsNullOrWhiteSpace(person.Username))
            {
                await searchInput.ClickAsync();
                await searchInput.FillAsync(person.Username);
                await searchInput.PressAsync("Enter");
            }

            await page.ClickAsync(SearchPage.TabBarPeople);
            await page.ClickAsync(SearchPage.ContentAreaPerson);
        }
    }
}
