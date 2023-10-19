using BusinessObjects.Models;
using DataTransferObjects.SmsOtpDTOs;

namespace Repositories
{
    public interface ISmsOtpRepository
    {
        Task<SmsOtp> SendOtp(string phoneNumber);

        Task<bool> VerifyOtp(VerifyOtpDTO request);
    }
}