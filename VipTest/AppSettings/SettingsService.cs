using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.AppSettings.Dto;
using VipTest.AppSettings.Payloads;
using VipTest.Localization;
using VipTest.Rides.Utli;

namespace VipTest.AppSettings;

public interface ISettingsService
{
    Task<(SettingsDto? settingsDto, string? error)> GetByIdAsync();
    Task<(SettingsDto? settingsDto, string? error)> UpdateAsync(SettingsUpdateForm settingsDto);
}

public class SettingsService : ISettingsService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repo;
    private readonly ILocalizationService _localize;

    public SettingsService(IMapper mapper, IRepositoryWrapper repo, ILocalizationService localize)
    {
        _mapper = mapper;
        _repo = repo;
        _localize = localize;
    }

    public async Task<(SettingsDto? settingsDto, string? error)> GetByIdAsync()
    {
        var settings = await _repo.SettingsRepository.Get(x => true
            , include: source => source.Include(x => x.RideBillingTypesConfigs));

        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var dto = _mapper.Map<SettingsDto>(settings);
        dto.VipPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip)?.BaseFarePrice;
        dto.VipDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip)?.DetourFarePrice;
        dto.NormalPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal)?.BaseFarePrice;
        dto.NormalDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal)?.DetourFarePrice;
        dto.VipToAlMuthanaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_AlMuthana)?.BaseFarePrice;
        dto.VipToAlMuthanaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_AlMuthana)?.DetourFarePrice;
        dto.NormalToAlMuthanaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_AlMuthana)?.BaseFarePrice;
        dto.NormalToAlMuthanaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_AlMuthana)?.DetourFarePrice;
        dto.VipToDhiQarPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_DhiQar)?.BaseFarePrice;
        dto.VipToDhiQarDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_DhiQar)?.DetourFarePrice;
        dto.NormalToDhiQarPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_DhiQar)?.BaseFarePrice;
        dto.NormalToDhiQarDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_DhiQar)?.DetourFarePrice;
        dto.VipToBabilPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Babil)?.BaseFarePrice;
        dto.VipToBabilDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Babil)?.DetourFarePrice;
        dto.NormalToBabilPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Babil)?.BaseFarePrice;
        dto.NormalToBabilDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Babil)?.DetourFarePrice;
        dto.VipToKarbalaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Karbala)?.BaseFarePrice;
        dto.VipToKarbalaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Karbala)?.DetourFarePrice;
        dto.NormalToKarbalaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Karbala)?.BaseFarePrice;
        dto.NormalToKarbalaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Karbala)?.DetourFarePrice;
        dto.VipToNajafPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Najaf)?.BaseFarePrice;
        dto.VipToNajafDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Najaf)?.DetourFarePrice;
        dto.NormalToNajafPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Najaf)?.BaseFarePrice;
        dto.NormalToNajafDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Najaf)?.DetourFarePrice;
        dto.VipToMaysanPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Maysan)?.BaseFarePrice;
        dto.VipToMaysanDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Maysan)?.DetourFarePrice;
        dto.NormalToMaysanPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Maysan)?.BaseFarePrice;
        dto.NormalToMaysanDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Maysan)?.DetourFarePrice;
        dto.VipToQadisiyahPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Qadisiyah)?.BaseFarePrice;
        dto.VipToQadisiyahDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Qadisiyah)?.DetourFarePrice;
        dto.NormalToQadisiyahPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Qadisiyah)?.BaseFarePrice;
        dto.NormalToQadisiyahDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Qadisiyah)?.DetourFarePrice;
        dto.VipToSamawaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Samawa)?.BaseFarePrice;
        dto.VipToSamawaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Samawa)?.DetourFarePrice;
        dto.NormalToSamawaPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Samawa)?.BaseFarePrice;
        dto.NormalToSamawaDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Samawa)?.DetourFarePrice;
        dto.VipToWasitPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Wasit)?.BaseFarePrice;
        dto.VipToWasitDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Vip_To_Wasit)?.DetourFarePrice;
        dto.NormalToWasitPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Wasit)?.BaseFarePrice;
        dto.NormalToWasitDetourPrice = settings.RideBillingTypesConfigs
            .FirstOrDefault(x => x.RideType == RideType.Normal_To_Wasit)?.DetourFarePrice;

        return (dto, null);
    }

    public async Task<(SettingsDto? settingsDto, string? error)> UpdateAsync(SettingsUpdateForm settingsUpdateForm)
    {
        // Fetch the settings with RideBillingTypesConfigs
        var settings = await _repo.SettingsRepository.Get(
            x => true,
            include: source => source.Include(x => x.RideBillingTypesConfigs)
        );

        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        // Update the basic settings fields
        _mapper.Map(settingsUpdateForm, settings);

        if (settingsUpdateForm.VipPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipPrice.Value;
            }
        }

        if (settingsUpdateForm.VipDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToAlMuthanaPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_AlMuthana);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToAlMuthanaPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToAlMuthanaDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_AlMuthana);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToAlMuthanaDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToAlMuthanaPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_AlMuthana);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToAlMuthanaPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToAlMuthanaDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_AlMuthana);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToAlMuthanaDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToDhiQarPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_DhiQar);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToDhiQarPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToDhiQarDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_DhiQar);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToDhiQarDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToDhiQarPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_DhiQar);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToDhiQarPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToDhiQarDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_DhiQar);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToDhiQarDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToBabilPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Babil);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToBabilPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToBabilDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Babil);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToBabilDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToBabilPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Babil);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToBabilPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToBabilDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Babil);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToBabilDetourPrice.Value;
            }
        }


        if (settingsUpdateForm.VipToNajafPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Najaf);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToNajafPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToNajafDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Najaf);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToNajafDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToNajafPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Najaf);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToNajafPrice.Value;
            }
        }


        if (settingsUpdateForm.VipToMaysanPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Maysan);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToMaysanPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToMaysanDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Maysan);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToMaysanDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToMaysanPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Maysan);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToMaysanPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToMaysanDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Maysan);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToMaysanDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToQadisiyahPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Qadisiyah);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToQadisiyahPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToQadisiyahDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Qadisiyah);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToQadisiyahDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToQadisiyahPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Qadisiyah);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToQadisiyahPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToQadisiyahDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Qadisiyah);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToQadisiyahDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToSamawaPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Samawa);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToSamawaPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToSamawaDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Samawa);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToSamawaDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToSamawaPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Samawa);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToSamawaPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToSamawaDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Samawa);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToSamawaDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToWasitPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Wasit);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToWasitPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToWasitDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Wasit);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToWasitDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToWasitPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Wasit);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToWasitPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToWasitDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Wasit);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToWasitDetourPrice.Value;
            }
        }


        if (settingsUpdateForm.NormalToNajafDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Najaf);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToNajafDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToKarbalaPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Karbala);
            if (vipConfig != null)
            {
                vipConfig.BaseFarePrice = settingsUpdateForm.VipToKarbalaPrice.Value;
            }
        }

        if (settingsUpdateForm.VipToKarbalaDetourPrice != null)
        {
            var vipConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Vip_To_Karbala);
            if (vipConfig != null)
            {
                vipConfig.DetourFarePrice = settingsUpdateForm.VipToKarbalaDetourPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToKarbalaPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Karbala);
            if (normalConfig != null)
            {
                normalConfig.BaseFarePrice = settingsUpdateForm.NormalToKarbalaPrice.Value;
            }
        }

        if (settingsUpdateForm.NormalToKarbalaDetourPrice != null)
        {
            var normalConfig = settings.RideBillingTypesConfigs
                .FirstOrDefault(x => x.RideType == RideType.Normal_To_Karbala);
            if (normalConfig != null)
            {
                normalConfig.DetourFarePrice = settingsUpdateForm.NormalToKarbalaDetourPrice.Value;
            }
        }

        // Update the RideBillingTypesConfig based on the incoming form data
        // foreach (var incomingConfig in settingsUpdateForm.RideBillingTypesConfigs)
        // {
        //     // Find the matching RideBillingTypesConfig in the existing settings
        //     var existingConfig = settings.RideBillingTypesConfigs
        //         .FirstOrDefault(config => config.RideType == incomingConfig.RideType);
        //
        //     if (existingConfig != null)
        //     {
        //         // Update the prices based on the incoming form
        //         existingConfig.BaseFarePrice = incomingConfig.BaseFarePrice;
        //         existingConfig.DetourFarePrice = incomingConfig.DetourFarePrice;
        //     }
        // }

        // Save changes to the database
        var result = await _repo.SettingsRepository.Update(settings, settings.Id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateSettings"));
        }

        return (_mapper.Map<SettingsDto>(result), null);
    }
}