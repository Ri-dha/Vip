using System.ComponentModel.DataAnnotations;
using VipTest.RideBillings.Models;
using VipTest.Rides.Models;
using VipTest.Utlity.Basic;

namespace VipTest.AppSettings.models;

public class Settings : BaseEntity<Guid>
{
    [MaxLength(256)] public string? UrlWebsite { get; set; }
    [MaxLength(256)] public string? Address { get; set; }
    [MaxLength(256)] public string? Email { get; set; }
    [MaxLength(256)] public string? IosAppLink { get; set; }
    [MaxLength(256)] public string? AndroidAppLink { get; set; }
    [MaxLength(256)] public string? UrlFacebook { get; set; }
    [MaxLength(256)] public string? UrlLinkedin { get; set; }
    [MaxLength(256)] public string? UrlInstagram { get; set; }
    [MaxLength(16)] public string? PhoneWhatsapp { get; set; }
    public string? PrivacyPolicy { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? AboutUs { get; set; }
    public string? EmergncyPhone { get; set; }
    public decimal? IqdToUsd { get; set; }
    public decimal? DriverCommission { get; set; }
    public decimal VisaCommission { get; set; }
    public decimal MissingBaggageCommission { get; set; }

    public decimal VipLoungeCommission { get; set; }

    public decimal WelcomePackageCommission { get; set; }


    // Navigation property for RideBillingTypesConfig
    public List<RideBillingTypesConfig> RideBillingTypesConfigs { get; set; } = new List<RideBillingTypesConfig>();
}