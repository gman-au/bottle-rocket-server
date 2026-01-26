namespace Rocket.Web.Host.Infrastructure
{
    public interface IThemeService
    {
        public bool IsDarkMode { get; set; }

        public void ToggleDarkMode();
    }
}