using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Utlity.Basic;

namespace VipTest.Users.OTP;

public interface IPendingCustomerRepository:IBaseRepository<PendingCustomer,Guid>
{
    Task<PendingCustomer?> GetLatestPendingCustomerByPhoneNumber(string phoneNumber);
    Task<List<PendingCustomer>?> GetAllPendingCustomerByPhoneNumber(string phoneNumber);
    Task<List<PendingCustomer>?> GetAllPendingCustomerWithOtpVerificationStatusIsFalse();
}


public class PendingCustomerRepository:BaseRepository<PendingCustomer,Guid>,IPendingCustomerRepository
{
    private readonly VipProjectContext _context;

    public PendingCustomerRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<PendingCustomer?> GetLatestPendingCustomerByPhoneNumber(string phoneNumber)
    {
        return await _context.OtpCustomers.OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task<List<PendingCustomer>?> GetAllPendingCustomerByPhoneNumber(string phoneNumber)
    {
        return await _context.OtpCustomers.Where(x => x.PhoneNumber == phoneNumber).ToListAsync();
    }

    public async Task<List<PendingCustomer>?> GetAllPendingCustomerWithOtpVerificationStatusIsFalse()
    {
        return await _context.OtpCustomers.Where(x => x.IsOtPVerified == false).ToListAsync();
        
    }
}