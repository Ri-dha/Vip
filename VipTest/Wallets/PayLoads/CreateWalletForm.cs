using Swashbuckle.AspNetCore.Annotations;

namespace VipTest.Wallets.PayLoads;

public class CreateWalletForm
{
    [SwaggerSchema(Description = "Name of the wallet(Required)")]
    public string Name { get; set; }
    
    [SwaggerSchema(Description = "User Id of the wallet(Nullable)")]
    public Guid? UserId { get; set; }   
}