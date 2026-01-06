using Microsoft.Playwright;
using RecordingBot.UiTests.PageObjects.Call.Page;
using RecordingBot.UiTests.PageObjects.Login.Steps;
using RecordingBot.UiTests.PageObjects.Teams.Steps;
using RecordingBot.UiTests.Shared.Users;

namespace RecordingBot.UiTests.Tests.Call;

[TestFixture]
[Category("CallComplianceBotOnline")]
[Description("Automated E2E-Tests for a call with joined compliance bot")]
public class CallComplianceBotOnlineTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            Permissions = ["microphone", "camera"]
        };
    }

    [Test]
    [Description("PersonA creates a meeting. Compliance bot starts recording call")]
    public async Task AudioCall_Should_DisplayRecordingComplianceToast_When_PersonACreatesMeeting()
    {
        var user = new UserA();
        var page = Page;

        await LoginSteps.LoginPerson(page, user);
        await CalendarSteps.CreateAudioCall(page);
        
        await VerifyRecordingToast(page);
        await HangUpCall(page);
    }

    private async Task VerifyRecordingToast(IPage page)
    {
        await Expect(page.Locator(CallPage.CallComplianceToast)).ToBeVisibleAsync();
    }

    private static async Task HangUpCall(IPage page)
    {
        await page.Locator(CallPage.HangUpBtn).ClickAsync();
    }
}
