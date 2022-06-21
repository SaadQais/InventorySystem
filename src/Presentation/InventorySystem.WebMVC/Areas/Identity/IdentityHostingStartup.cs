[assembly: HostingStartup(typeof(InventorySystem.WebMVC.Areas.Identity.IdentityHostingStartup))]
namespace InventorySystem.WebMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}