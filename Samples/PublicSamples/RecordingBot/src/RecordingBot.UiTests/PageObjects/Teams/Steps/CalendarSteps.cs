using Microsoft.Playwright;
using RecordingBot.UiTests.PageObjects.Teams.Page;

namespace RecordingBot.UiTests.PageObjects.Teams.Steps
{
    public class CalendarSteps
    {
        public static async Task CreateAudioCall(IPage page)
        {
            await page.Locator(TeamsPage.Calendar).ClickAsync();
            await page.Locator(CalendarPage.StartMeetingBtn).ClickAsync();
            await page.Locator(CalendarPage.MeetNowFlyoutBtn).ClickAsync();
            await page.Locator(CalendarPage.JoinBtn).ClickAsync();
            await page.Locator(CalendarPage.InviteDismissBtn).ClickAsync();
        }
    }
}
