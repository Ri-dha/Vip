using Microsoft.AspNetCore.Mvc;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.Wallets.Dtos;
using VipTest.Wallets.PayLoads;
using VipTest.Wallets.Services;

namespace VipTest.Wallets.Controller;


[Route("wallets")]
public class WalletController:BaseController
{
    
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    
    [HttpGet("get-wallet/{id}")]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        var (walletDto, error) = await _walletService.GetWalletAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(walletDto);
    }
    
    [HttpGet("get-all-wallets")]
    public async Task<IActionResult> GetAllWallets([FromQuery] WalletFilterForm filterForm)
    {
        var (walletDtos,totalCount, error) = await _walletService.GetWalletsAsync(filterForm);
        if (error != null) return BadRequest(new { error });
        
        var result = new Page<WalletDto>()
        {
            Data = walletDtos,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filterForm.PageSize),
            CurrentPage = filterForm.PageNumber,
        };
        
        return Ok(result);
    }
    
    [HttpPost("create-wallet")]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletForm walletCreateForm)
    {
        var (walletDto, error) = await _walletService.CreateWalletAsync(walletCreateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(walletDto);
    }
    
    
}