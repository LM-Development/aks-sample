namespace RecordingBot.UiTests.PageObjects.Call.Page
{
    public class CallPage
    {
        public const string CallOptionsBtn = "[data-tid='dropdown-calling-toggle-more-options-btn']";
        public const string VideoCallBtn = "[data-tid='chat-call-video-button']";
        public const string AudioCallBtn = "[data-tid='chat-call-audio-button']";
        public const string HangUpBtn = "#hangup-button";

        public const string CallToastCallingActions = "[data-testid='calling-actions']";
        public const string CallToastAcceptVideo = CallToastCallingActions + " button:nth-child(1)";
        public const string CallToastAcceptAudio = CallToastCallingActions + " button:nth-child(2)";
        public const string CallToastHangUp = CallToastCallingActions + "button:nth-child(3)";

        public const string CallComplianceToast = "#missing-selector";
    }
}
