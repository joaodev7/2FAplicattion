using OtpNet;

namespace _2FAplicattion.Transactions
{
    public class TwoFactorAuthenticationTRA
    {
        public static bool VerifyAuthentication(string userlogin, string codeAuthenticator)
        {
            var segredoAutenticacao = "SKZN2VVKF4ARXRTIB3FBU2OBSGUBDX4R";

            if (segredoAutenticacao == null)
            {
                return false;
            }

            var totp = new Totp(Base32Encoding.ToBytes(segredoAutenticacao.ToString()));

            bool codigoValido = totp.VerifyTotp(codeAuthenticator, out _);

            return codigoValido;
        }

    }
}
