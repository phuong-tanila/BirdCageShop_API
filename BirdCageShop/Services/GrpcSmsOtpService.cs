using DataTransferObjects.SmsOtpDTOs;
using Grpc;
using Grpc.Core;
using Repositories;

namespace BirdCageShop.Grpcs
{
    public class GrpcSmsOtpService : SmsOtp.SmsOtpBase
    {
        private readonly ISmsOtpRepository _smsOtpRepository;

        public GrpcSmsOtpService(ISmsOtpRepository smsOtpRepository)
        {
            _smsOtpRepository = smsOtpRepository;
            Console.WriteLine("Constructor");
        }

        public override async Task<GrpcSmsOtpResponse> SendOtp(GrpcSmsOtpRequest request, ServerCallContext context)
        {
            Console.WriteLine("Send!~!!!");
            var otp = await _smsOtpRepository.SendOtp(request.PhoneNumber);
            var response = new GrpcSmsOtpResponse();
            response.PhoneNumber = otp.PhoneNumber;
            response.OtpId = otp.Id.ToString();
            response.ExpiredAt = otp.ExpiredAt.ToString();
            response.CreateAt = otp.CreateAt.ToString();
            return response;
        }
        public override async Task<GrpcVerifyOtpResponse> VerifyOtp(GrpcVerifyOtpRequest request, ServerCallContext context)
        {
            Console.WriteLine("Verify!!");
            var dto = new VerifyOtpDTO();
            dto.OtpValue = request.OtpValue;
            dto.PhoneNumber = request.PhoneNumber;
            var verifyResult = await _smsOtpRepository.VerifyOtp(dto);
            return new GrpcVerifyOtpResponse { Valid = verifyResult };
        }


    }
}
