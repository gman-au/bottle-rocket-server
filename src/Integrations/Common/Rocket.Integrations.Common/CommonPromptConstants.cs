namespace Rocket.Integrations.Common
{
    public static class CommonPromptConstants
    {
        public const string SystemPromptRetitleFile =
            "You are an expert editor. Read the provided document and recommend a short, accurate title summarizing the contents.";

        public const string UserPromptRetitleFileTemplate =
            @"Here is the document:\n\n<document>\n{0}\n</document>\n\nRecommend a short file name title based on the text above.";
    }
}