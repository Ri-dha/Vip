using System.Security.Claims;
using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Tickets.Dto;
using VipTest.Tickets.models;
using VipTest.Tickets.Payload;
using VipTest.Tickets.utli;
using VipTest.Warehouses.Services;

namespace VipTest.Tickets.Service;

public interface ISupportTicketsService
{
    Task<(SupportTicketsDto? ticketsDto, string? error)> CreateSupportTicketAsync(SupportTicketsCreateForm form);

    Task<(SupportTicketsDto? ticketsDto, string? error)> UpdateSupportTicketAsync(Guid ticketId,
        SupportTicketsUpdateForm form);

    Task<(SupportTicketsDto? ticketsDto, string? error)> GetSupportTicketAsync(Guid ticketId);

    Task<(List<SupportTicketsDto>? ticketsDto, int? totalCount, string? error)> GetAllTickets(
        SupportTicketsFilterForm form);

    Task<(bool success, string? error)> DeleteSupportTicketAsync(Guid ticketId);
    Task<(bool success, string? error)> AssignSupportTicketAsync(Guid ticketId, Guid adminId);
    Task<(bool success, string? error)> CloseSupportTicketAsync(Guid ticketId);

    Task<(SupportTicketsDto? dto, string? error)> ReplyToTicketAsync(Guid ticketId, string response);
}

public class SupportTicketsService : ISupportTicketsService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDtoTranslationService _dtoTranslationService;

    public SupportTicketsService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize,
        IHttpContextAccessor httpContextAccessor, IDtoTranslationService dtoTranslationService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
        _httpContextAccessor = httpContextAccessor;
        _dtoTranslationService = dtoTranslationService;
    }

    private async Task<string> GenerateSupportTicketCode()
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var latestTicket = await _repositoryWrapper.SupportTicketsReposiotry.GetLatestSupportTicketAsync(datePart);

        if (latestTicket == null)
        {
            return datePart + "001";
        }

        var ticketCode = latestTicket.TicketCode;
        var ticketNumber = int.Parse(ticketCode.Substring(ticketCode.Length - 3));
        ticketNumber++;
        return datePart + ticketNumber.ToString().PadLeft(3, '0');
    }

    public async Task<(SupportTicketsDto? ticketsDto, string? error)> CreateSupportTicketAsync(
        SupportTicketsCreateForm form)
    {
        var ticket = _mapper.Map<SupportTickets>(form);
        ticket.TicketCode = await GenerateSupportTicketCode();

        if (form.CustomerId != null)
        {
            var customer = await _repositoryWrapper.CustomerRepository.GetById(form.CustomerId.Value);
            if (customer == null)
            {
                return (null, _localize.GetLocalizedString("CustomerNotFound"));
            }

            ticket.Customer = customer;
            ticket.CustomerId = customer.Id;
        }
        else if (form.DriverId != null)
        {
            var driver = await _repositoryWrapper.DriverRepository.GetById(form.DriverId.Value);
            if (driver == null)
            {
                return (null, _localize.GetLocalizedString("DriverNotFound"));
            }

            ticket.Driver = driver;
            ticket.DriverId = driver.Id;
        }

        var result = await _repositoryWrapper.SupportTicketsReposiotry.Add(ticket);

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateSupportTicket"));
        }

        return (_mapper.Map<SupportTicketsDto>(result), null);
    }

    public async Task<(SupportTicketsDto? ticketsDto, string? error)> UpdateSupportTicketAsync(Guid ticketId,
        SupportTicketsUpdateForm form)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.GetById(ticketId);
        if (ticket == null)
        {
            return (null, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        _mapper.Map(form, ticket);
        var result = await _repositoryWrapper.SupportTicketsReposiotry.Update(ticket, ticketId);

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateSupportTicket"));
        }

        return (_mapper.Map<SupportTicketsDto>(result), null);
    }

    public async Task<(SupportTicketsDto? ticketsDto, string? error)> GetSupportTicketAsync(Guid ticketId)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.GetById(ticketId);
        if (ticket == null)
        {
            return (null, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        var dto = _mapper.Map<SupportTicketsDto>(ticket);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }


    public async Task<(List<SupportTicketsDto>? ticketsDto, int? totalCount, string? error)> GetAllTickets(
        SupportTicketsFilterForm form)
    {
        var (tickets, totalCount) = await _repositoryWrapper.SupportTicketsReposiotry.GetAll<SupportTicketsDto>(
            x =>
                (form.CustomerId == null || x.CustomerId == form.CustomerId) &&
                (form.DriverId == null || x.DriverId == form.DriverId) &&
                (form.TicketCode == null || x.TicketCode == form.TicketCode) &&
                (form.TicketType == null || x.TicketType == form.TicketType) &&
                (form.TicketStatus == null || x.TicketStatus == form.TicketStatus) &&
                (form.ClosedAt == null || x.ClosedAt == form.ClosedAt) &&
                (form.ClosedBy == null || x.ClosedBy == form.ClosedBy) &&
                !x.Deleted,
            form.PageNumber, form.PageSize);

        tickets.ForEach(x => _dtoTranslationService.TranslateEnums(x));
        return (tickets, totalCount, null);
    }


    public async Task<(bool success, string? error)> DeleteSupportTicketAsync(Guid ticketId)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.Get(x => x.Id == ticketId);
        if (ticket == null)
        {
            return (false, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        ticket.Deleted = true;
        await _repositoryWrapper.SupportTicketsReposiotry.Update(ticket, ticketId);
        return (true, null);
    }

    public async Task<(bool success, string? error)> AssignSupportTicketAsync(Guid ticketId, Guid adminId)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.GetById(ticketId);
        if (ticket == null)
        {
            return (false, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        var admin = await _repositoryWrapper.AdminRepository.GetById(adminId);
        if (admin == null)
        {
            return (false, _localize.GetLocalizedString("AdminNotFound"));
        }

        ticket.AssignedTo = adminId;
        ticket.AssignedToName = admin.Username;

        var result = await _repositoryWrapper.SupportTicketsReposiotry.Update(ticket, ticketId);

        if (result == null)
        {
            return (false, _localize.GetLocalizedString("FailedToAssignSupportTicket"));
        }

        return (true, null);
    }

    public async Task<(bool success, string? error)> CloseSupportTicketAsync(Guid ticketId)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.GetById(ticketId);
        if (ticket == null)
        {
            return (false, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (false, _localize.GetLocalizedString("UserNotFound"));
        }

        var user = await _repositoryWrapper.AdminRepository.GetById(Guid.Parse(userId));

        if (user == null)
        {
            return (false, _localize.GetLocalizedString("UserNotFound"));
        }

        ticket.ClosedAt = DateTime.UtcNow;
        ticket.ClosedBy = user.Id;
        ticket.ClosedByName = user.Username;
        ticket.TicketStatus = TicketStatus.Closed;

        var result = await _repositoryWrapper.SupportTicketsReposiotry.Update(ticket, ticketId);

        if (result == null)
        {
            return (false, _localize.GetLocalizedString("FailedToCloseSupportTicket"));
        }

        return (true, null);
    }

    public async Task<(SupportTicketsDto? dto, string? error)> ReplyToTicketAsync(Guid ticketId, string response)
    {
        var ticket = await _repositoryWrapper.SupportTicketsReposiotry.GetById(ticketId);
        if (ticket == null)
        {
            return (null, _localize.GetLocalizedString("SupportTicketNotFound"));
        }

        if (ticket.Responses == null)
        {
            ticket.Responses = new List<string>();
        }

        ticket.Responses.Add(response);
        var result = await _repositoryWrapper.SupportTicketsReposiotry.Update(ticket, ticketId);

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToReplyToTicket"));
        }

        return (_mapper.Map<SupportTicketsDto>(result), null);
    }
}