using Microsoft.AspNetCore.Mvc;
using _2FAplicattion.Transactions;
using _2FAplicattion.Integration;
using OtpNet;
using QRCoder;

namespace _2FAplicattion.Controllers
{
    public class TwoFactorAuthenticationController : Controller
    {
        [HttpPost("api/GenerateQRCode")]
        public IActionResult GenerateQRCode([FromBody] CreateTwoFactoAuthentication request)
        {
            string nomeUsuario = request.Login;
            string nomeAplicacao = "Fitbank Admin";
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string chaveConfiguracao = Base32Encoding.ToString(secretKey).Replace("=", "");

            string dadosConfiguracao = $"otpauth://totp/{nomeAplicacao}:{nomeUsuario}?secret={chaveConfiguracao}&issuer={nomeAplicacao}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(dadosConfiguracao, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            string nomeArquivo = "qrcode.png";
            System.IO.File.WriteAllBytes(nomeArquivo, qrCodeBytes);

            return Json(new { Status = "OK", Message = chaveConfiguracao + qrCodeBytes});
        }

        [HttpPost("api/verify2fa")]
        public IActionResult Verify2FA([FromBody] TwoFactorAuthenticator request)
        {
            string userlogin = request.Login;
            string codeAuthenticator = request.AuthenticationCode;

            bool validCode = TwoFactorAuthenticationTRA.VerifyAuthentication(userlogin, codeAuthenticator);

            if (validCode)
            {
                return Json(new { Status = "OK", Message = "Usuário validado" });
            }
            else
            {
                return Json(new { Status = "NOK", Message = "Usuário invalidado" });
            }
        }
    }
}
