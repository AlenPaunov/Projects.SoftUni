namespace ProjectsSoftuni.Common
{
    public static class EmailSendConstants
    {
        public const string InvitationEmailHtmlPath = "Views/Emails/invitationTemplate.html";

        public const string ProjectDetailsLink = "http://localhost:50198/Projects/Details/{0}";

        public const string InviteUserImagePlaceholder = "@InviteUserImage";

        public const string InviteUserUsernamePlaceholder = "@InviteUserUsername";

        public const string MemberImagePlaceholder = "@memberImage";

        public const string MemberUsernamePlaceholder = "@MemberUsername";

        public const string ProjectDetailsLinkPlaceholder = "@ProjectDetailsLink";

        public const string ProjectNamePlaceholder = "@ProjectName";

        public const string InvitationLinkPlaceholder = "@InvitationLink";

        public const string InvitationEmailSubject = "{0} invited you to {1} project";

    }
}
