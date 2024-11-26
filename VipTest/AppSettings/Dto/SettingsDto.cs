using VipTest.RideBillings.Dto;
using VipTest.Utlity.Basic;

namespace VipTest.AppSettings.Dto;

public class SettingsDto:BaseDto<Guid>
{
    public string? UrlWebsite { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? IosAppLink { get; set; }
    public string? AndroidAppLink { get; set; }
    public string? UrlFacebook { get; set; }
    public string? UrlLinkedin { get; set; }
    public string? UrlInstagram { get; set; }
    public string? PhoneWhatsapp { get; set; }
    public string? PrivacyPolicy { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? AboutUs { get; set; }
    public string? EmergncyPhone { get; set; }
    public decimal? IqdToUsd { get; set; }
    public decimal? DriverCommission { get; set; }
    public decimal? VisaCommission { get; set; }
    public decimal MissingBaggageCommission { get; set; }
    public decimal VipLoungeCommission { get; set; }
    public decimal WelcomePackageCommission { get; set; }


    // public List<RideBillingSettingsDto> RideBillingTypesConfigs { get; set; }
    
    public decimal? VipPrice { get; set; }
    public decimal? VipDetourPrice { get; set; }
    public decimal? NormalPrice { get; set; }
    public decimal? NormalDetourPrice { get; set; }
    
    public decimal? VipToAlMuthanaPrice { get; set; }
    public decimal? VipToAlMuthanaDetourPrice { get; set; }
    public decimal? NormalToAlMuthanaPrice { get; set; }
    public decimal? NormalToAlMuthanaDetourPrice { get; set; }
    
    public decimal? VipToDhiQarPrice { get; set; }
    public decimal? VipToDhiQarDetourPrice { get; set; }
    public decimal? NormalToDhiQarPrice { get; set; }
    public decimal? NormalToDhiQarDetourPrice { get; set; }
    
    public decimal? VipToMaysanPrice { get; set; }
    public decimal? VipToMaysanDetourPrice { get; set; }
    public decimal? NormalToMaysanPrice { get; set; }
    public decimal? NormalToMaysanDetourPrice { get; set; }
    
    public decimal? VipToQadisiyahPrice { get; set; }
    public decimal? VipToQadisiyahDetourPrice { get; set; }
    public decimal? NormalToQadisiyahPrice { get; set; }
    public decimal? NormalToQadisiyahDetourPrice { get; set; }
    
    public decimal? VipToSamawaPrice { get; set; }
    public decimal? VipToSamawaDetourPrice { get; set; }
    public decimal? NormalToSamawaPrice { get; set; }
    public decimal? NormalToSamawaDetourPrice { get; set; }
    
    public decimal? VipToWasitPrice { get; set; }
    public decimal? VipToWasitDetourPrice { get; set; }
    public decimal? NormalToWasitPrice { get; set; }
    public decimal? NormalToWasitDetourPrice { get; set; }
    
    public decimal? VipToBabilPrice { get; set; }
    public decimal? VipToBabilDetourPrice { get; set; }
    public decimal? NormalToBabilPrice { get; set; }
    public decimal? NormalToBabilDetourPrice { get; set; }
    
    public decimal? VipToNajafPrice { get; set; }
    public decimal? VipToNajafDetourPrice { get; set; }
    public decimal? NormalToNajafPrice { get; set; }
    public decimal? NormalToNajafDetourPrice { get; set; }
    
    public decimal? VipToKarbalaPrice { get; set; }
    public decimal? VipToKarbalaDetourPrice { get; set; }
    public decimal? NormalToKarbalaPrice { get; set; }
    public decimal? NormalToKarbalaDetourPrice { get; set; }
    


}