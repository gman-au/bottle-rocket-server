namespace Rocket.Web.Client
{
    public class ThemeService : IThemeService
    {
        public bool IsDarkMode { get; set; }
        
        public void ToggleDarkMode() => IsDarkMode = !IsDarkMode;

        public void SetDarkMode(bool darkMode) => IsDarkMode = darkMode;
    }
}