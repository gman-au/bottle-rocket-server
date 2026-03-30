namespace Rocket.Replicate.Infrastructure
{
    public interface IMarkdownStripper
    {
        string StripFooter(string markdown);
    }
}