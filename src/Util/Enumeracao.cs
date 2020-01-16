using System.ComponentModel;

namespace Util
{
    public sealed class Enumeracao
    {
        public enum ESituacao
        {
            [Description("Ativo")]
            Ativo = 1,
            [Description("Cancelado")]
            Cancelado = 2,
        }
    }
}
