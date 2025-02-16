using ElectroDepotClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Context;
using Server.Services;

namespace Server.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected readonly DatabaseContext _context;
        protected readonly ImageStorageService _imageStorageService;
        protected readonly ServerConfigService _serverConfigService;

        public CustomControllerBase(DatabaseContext context)
        {
            _context = context;
            _serverConfigService = ServerConfigService.Create();
            try
            {
                _serverConfigService.LoadConfigFile();
            }
            catch (Exception exception)
            {

            }
            _imageStorageService = ImageStorageService.CreateService();
            _imageStorageService.Initialize(_serverConfigService.StoragePath);
        }
    }
}
