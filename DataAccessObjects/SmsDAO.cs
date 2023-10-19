using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SmsDAO
    {
        private readonly BirdCageShopContext _context;

        public SmsDAO(BirdCageShopContext context)
        {
            _context = context;
        }

        public async Task<SmsOtp> CreateOtp(SmsOtp sendSms)
        {
            var entity = new SmsOtp();
            if (sendSms == null)
            {
                entity = _context.SmsOtps.AddAsync(sendSms).Result.Entity;
            }
            else
            {
                var entryEntity = _context.Entry(sendSms);
                entryEntity.State = EntityState.Modified;
                entity = entryEntity.Entity;
            }
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<SmsOtp> FindOtp(string phoneNumber)
        {

            return await _context.SmsOtps.FirstOrDefaultAsync(
                o => o.PhoneNumber == phoneNumber
                );
        }

    }

}
