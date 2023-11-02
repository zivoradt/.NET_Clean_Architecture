using MVC.Contracts;
using MVC.Services.Base;

namespace MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IClient _client;

        public LeaveAllocationService(IClient client, ILocalStorageService localStorageService) : base(localStorageService, client)
        {
            this._client = client;
            this._storageService = localStorageService;
        }

        public async Task<Response<int>> CreateLeaveAllocation(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();

                CreateLeaveAllocationDto createLeaveAllocationDto = new() { LeaveTypeId = leaveTypeId };

                AddBearerToken();

                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocationDto);

                Console.Write(apiResponse.Message);

                if (apiResponse.Success)
                {
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }
                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}