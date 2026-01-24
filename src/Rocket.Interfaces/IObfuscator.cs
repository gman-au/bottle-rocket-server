namespace Rocket.Interfaces
{
    public interface IObfuscator
    {
        string Obfuscate(
            string input,
            int displayLastCharacters = 4,
            int dummyCharacters = 4,
            char obfuscationCharacter = '*'
        );
    }
}