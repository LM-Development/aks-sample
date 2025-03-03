using Microsoft.Playwright;
using RecordingBot.UiTests.PageObjects.Call.Page;
using RecordingBot.UiTests.PageObjects.Login.Steps;
using RecordingBot.UiTests.PageObjects.Teams.Steps;
using RecordingBot.UiTests.Shared.Models;
using RecordingBot.UiTests.Shared.Users;

namespace RecordingBot.UiTests.Tests.Call;

[TestFixture]
[Category("CallComplianceBotOnline")]
[Description("Automated E2E-Tests for a call with joined compliance bot")]
[Parallelizable(ParallelScope.None)]
public class CallComplianceBotOnlineTests : PageTest
{
    [Test]
    [Description("PersonA calls PersonB. Compliance bot starts recording call")]
    public async Task AudioCall_Should_DisplayRecordingComplianceToast_When_PersonACallsPersonB()
    {
        var userA = new UserA();
        var userB = new UserB();

        var contextUserA = await CreateBrowserContextAsync(["microphone", "camera"]);
        var contextUserB = await CreateBrowserContextAsync(["microphone", "camera"]);

        var pageUserA = await contextUserA.NewPageAsync();
        var pageUserB = await contextUserB.NewPageAsync();

        await Task.WhenAll(SetupPerson(pageUserA, userA, userB), SetupPerson(pageUserB, userB, userA));

        await MakeAudioCall(pageUserA, pageUserB);
        await VerifyRecordingToast(pageUserA);
        await VerifyRecordingToast(pageUserB);
        await HangUpCall(pageUserA);
    }

    [Test]
    [Description("PersonA calls PersonB. Bot starts recording call")]
    public async Task VideoCall_ShouldDisplayRecordingComplianceToast_When_PersonACallsPersonB()
    {
        var userA = new UserA();
        var userB = new UserB();

        var contextUserA = await CreateBrowserContextAsync(["microphone", "camera"]);
        var contextUserB = await CreateBrowserContextAsync(["microphone", "camera"]);

        var pageUserA = await contextUserA.NewPageAsync();
        var pageUserB = await contextUserB.NewPageAsync();

        await Task.WhenAll(SetupPerson(pageUserA, userA, userB), SetupPerson(pageUserB, userB, userA));

        await MakeVideoCall(pageUserA, pageUserB);
        await VerifyRecordingToast(pageUserA);
        await VerifyRecordingToast(pageUserB);
        await HangUpCall(pageUserA);
    }

    private async Task<IBrowserContext> CreateBrowserContextAsync(string[] permissions)
    {
        return await Browser.NewContextAsync(new BrowserNewContextOptions { Permissions = permissions });
    }

    private static async Task MakeAudioCall(IPage pageUserA, IPage pageUserB)
    {
        await pageUserA.WaitForSelectorAsync(CallPage.CallOptionsBtn);
        await pageUserA.Locator(CallPage.CallOptionsBtn).ClickAsync();

        await pageUserA.WaitForSelectorAsync(CallPage.AudioCallBtn);
        await pageUserA.Locator(CallPage.AudioCallBtn).ClickAsync();

        await pageUserB.WaitForSelectorAsync(CallPage.CallToastAcceptAudio);
        await pageUserB.Locator(CallPage.CallToastAcceptAudio).ClickAsync();
    }

    private static async Task MakeVideoCall(IPage pageUserA, IPage pageUserB)
    {
        await pageUserA.WaitForSelectorAsync(CallPage.CallOptionsBtn);
        await pageUserA.Locator(CallPage.CallOptionsBtn).ClickAsync();

        await pageUserA.WaitForSelectorAsync(CallPage.VideoCallBtn);
        await pageUserA.Locator(CallPage.VideoCallBtn).ClickAsync();

        await pageUserB.WaitForSelectorAsync(CallPage.CallToastAcceptVideo);
        await pageUserB.Locator(CallPage.CallToastAcceptVideo).ClickAsync();
    }

    private async Task VerifyRecordingToast(IPage pageUser)
    {
        await Expect(pageUser.FrameLocator("iframe").Locator(CallPage.CallComplianceToast)).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 30000 });
    }

    private static async Task HangUpCall(IPage pageUser)
    {
        await pageUser.FrameLocator("iframe").Locator(CallPage.HangUpBtn).ClickAsync();
    }

    private static async Task SetupPerson(IPage page, Person caller, Person responder)
    {
        await LoginSteps.LoginPerson(page, caller);
        await TeamsSteps.SearchPersonAndOpenChat(page, responder);
    }
}
