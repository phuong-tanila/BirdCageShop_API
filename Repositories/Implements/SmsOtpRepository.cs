using BusinessObjects.Models;
using DataAccessObjects;
using DataTransferObjects.SmsOtpDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using static System.Net.WebRequestMethods;

namespace Repositories.Implements
{
    public class SmsOtpRepository : ISmsOtpRepository
    {
        private readonly SmsDAO _smsDAO;
        private readonly IConfiguration _configuration;
        private readonly int _otpLifeTime;
        private readonly int _otpRequestTimeOut;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        private readonly string _bodyTemplate;

        public SmsOtpRepository(IConfiguration configuration, SmsDAO smsDAO)
        {
            _configuration = configuration;
            _smsDAO = smsDAO;
            _otpLifeTime = int.Parse(_configuration["Otp:LifeTime"]);
            _otpRequestTimeOut = int.Parse(_configuration["Otp:RequestTimeOut"]);
            _accountSid = _configuration["Otp:Twilio:AccountSid"];
            _authToken = _configuration["Otp:Twilio:AuthToken"];
            _fromNumber = _configuration["Otp:Twilio:FromNumber"];
            _bodyTemplate = _configuration["Otp:BodyTemplate"];
        }

        public async Task<SmsOtp> SendOtp(string phoneNumber)
        {
            var otp = await _smsDAO.FindOtp(phoneNumber);
            if (otp == null)
            {
                otp = createOtpObject(phoneNumber);
            }else
            {
                handleExistedOtp(otp);
            }
            otp.OtpValue = generateOtpValue();
            var otpEntity = await _smsDAO.CreateOtp(otp);
            //sendOtp(otp);
            otpEntity.OtpValue = null;
            return otpEntity;
        }

        private async void sendOtp(SmsOtp otp)
        {
            var message = await MessageResource.CreateAsync(
                from: new Twilio.Types.PhoneNumber(_fromNumber),
                to: new Twilio.Types.PhoneNumber(otp.OtpValue),
                body: _bodyTemplate + otp.OtpValue
                );
            
        }
        private SmsOtp createOtpObject(string phoneNumber)
        {
            SmsOtp otp = new SmsOtp();
            otp.PhoneNumber = phoneNumber;
            otp.CreateAt = DateTime.Now;
            otp.ExpiredAt = DateTime.Now.AddMinutes(_otpLifeTime);
            otp.RequestCount = 0;
            return otp;
        }
        private SmsOtp handleExistedOtp(SmsOtp otp)
        {
            //var otpRequestTimeOut = 
            if (otp.CreateAt.Value.AddMinutes(_otpRequestTimeOut) >= DateTime.Now)
            {
                otp.CreateAt = DateTime.Now;
                otp.RequestCount += 1;
                otp.ExpiredAt = DateTime.Now.AddMinutes(_otpLifeTime);
            }
            else
            {

            }
            return null;
        }
        private string generateOtpValue()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }

        public async Task<bool> VerifyOtp(VerifyOtpDTO request)
        {

            var otp = await _smsDAO.FindOtp(request.PhoneNumber);
            if(
                otp != null 
                && otp.OtpValue == request.OtpValue
                && otp.ExpiredAt < DateTime.Now
             )
            {
                return true;
            }
            return false;
        }

    }
}
