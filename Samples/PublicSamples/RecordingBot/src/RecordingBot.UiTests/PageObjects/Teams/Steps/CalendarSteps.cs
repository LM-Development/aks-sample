using Microsoft.Playwright;
using RecordingBot.UiTests.PageObjects.Teams.Page;

namespace RecordingBot.UiTests.PageObjects.Teams.Steps
{
    public class CalendarSteps
    {
        public static async Task CreateAudioCall(IPage page)
        {

            await page.Locator(TeamsPage.Calendar).ClickAsync();
            var calendarIframe = page.FrameLocator(CalendarPage.CalendarIFrame).First;
            await calendarIframe.Locator(CalendarPage.StartMeetingBtn).First.ClickAsync();
            await calendarIframe.Locator(CalendarPage.MeetNowFlyoutBtn).ClickAsync();
            await page.Locator(CalendarPage.JoinBtn).ClickAsync();
            await page.Locator(CalendarPage.InviteDismissBtn).ClickAsync();
        }
    }
}
