using AutoMapper;
using MVC.Contracts;
using MVC.Models;
using MVC.Services.Base;

namespace MVC.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        private readonly IMapper _mapper;

        public LeaveRequestService(IClient client, ILocalStorageService localStorageService
            , IMapper mapper)
            : base(localStorageService, client)
        {
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM createLeaveRequestVM)
        {
            try
            {
                var response = new Response<int>();

                CreateLeaveRequestDto createLeaveRequest = _mapper.Map<CreateLeaveRequestDto>(createLeaveRequestVM);
                AddBearerToken();

                var apiResponse = await _client.LeaveRequestsPOSTAsync(createLeaveRequest);

                if (apiResponse.Success)
                {
                    response.Data = apiResponse.Id;
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

        public Task DeleteLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }
    }
}