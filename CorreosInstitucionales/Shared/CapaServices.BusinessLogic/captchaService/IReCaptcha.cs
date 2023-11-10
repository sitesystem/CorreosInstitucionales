using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.captchaService
{
    public interface IReCaptcha
    {
        Task<bool> ValidateResponse(string recaptchaResponse, string secretKey);
    }
}
