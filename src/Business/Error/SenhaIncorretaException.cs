namespace Business.Error
{
    public class SenhaIncorretaException : RegraDeNegocioException
    {
        public SenhaIncorretaException() : base("Não confere a senha") { }
    }
}
