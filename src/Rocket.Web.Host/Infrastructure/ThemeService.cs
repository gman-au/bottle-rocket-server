namespace Rocket.Web.Host.Infrastructure
{
    public class ThemeService : IThemeService
    {
        public bool IsDarkMode { get; set; }
        
        public void ToggleDarkMode()
        {
            IsDarkMode = !IsDarkMode;
        }
    }
}