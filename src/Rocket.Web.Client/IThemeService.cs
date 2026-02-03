namespace Rocket.Web.Client
{
    public interface IThemeService
    {
        public bool IsDarkMode { get; set; }

        public void ToggleDarkMode();
        
        public void SetDarkMode(bool darkMode);
    }
}