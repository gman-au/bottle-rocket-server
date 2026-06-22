namespace Rocket.Domain.Enum
{
    // For future reference: these are two ASCII codes put together
    // e.g. 0x3146 => 0x31 = '1', 0x46 = 'F' => '1F'
    // https://www.rapidtables.com/code/text/ascii-table.html
    public enum RocketbookModernProductTypeEnum
    {
        FusionV16 = 0x3136,
        FusionV1F = 0x3146,
        CoreV17   = 0x3137, 
        CoreV1D = 0x3144,
        FlipV1J = 0x314A
    }
}
