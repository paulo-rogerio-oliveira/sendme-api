using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendMe.Models;
using SendMe.Services;

namespace SendMe.Controllers
{

    [Route("api/[controller]")]
    [ApiController]    
    [Authorize]
    public class SmsController : ControllerBase
    {

        private IPersistence<Sms> _persistenceService;

        public SmsController(IPersistence<Sms> persistenceService) { 
            this._persistenceService = persistenceService;
            
        }


        [HttpGet]
        public async Task<IEnumerable<Sms>> Get()
        {
            return await _persistenceService.FindAll();
        }

        [HttpDelete]
        public async Task Remove([FromBody] string id)
        {

            await _persistenceService.Remove(id);
        }

        [HttpPost]
        public async Task<JsonResult> Update([FromBody] Sms sms)
        {

            var result = await _persistenceService.Save(sms);
            return new JsonResult(result);
        }


        [HttpGet(template:"id", Name ="id")]
        public async Task<JsonResult> FindByID([FromQuery] string id)
        {
            var result = await _persistenceService.FindByID(id);
            return new JsonResult(result);
        }

        [HttpGet(template: "providerName", Name ="providerName")]
        public async Task<IEnumerable<Sms>> FindByProviderName([FromQuery] string providerName)
        {

            return await _persistenceService.FindByEqualsFilter<string>("ProviderName",providerName);
        }
    }
}
