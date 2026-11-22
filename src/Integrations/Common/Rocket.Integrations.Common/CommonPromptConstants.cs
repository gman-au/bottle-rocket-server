namespace Rocket.Integrations.Common
{
    public static class CommonPromptConstants
    {
        public const string SystemPromptRetitleFile =
            "You are an expert editor. Read the provided document and recommend a short, accurate title summarizing the contents.";

        public const string UserPromptRetitleFileTemplate =
            @"Here is the document:\n\n<document>\n{0}\n</document>\n\nRecommend a short file name title based on the text above.";

        public const string SystemPromptExtractTextFromImage =
            "You transcribe handwritten documents. Output only the transcription, " +
            "preserving line breaks and paragraph structure. Write [illegible] for " +
            "words you cannot read. Never guess or add text that isn't on the page.";

        public const string UserPromptExtractTextFromImage =
            "Perform OCR on this image and return only the text.";
    }
}