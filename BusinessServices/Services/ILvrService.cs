using Entity.DTOs;
using Entity.Models;

namespace BusinessServices.Services
{
    public interface ILvrService
    {
        Task<LVRDto> CalculateLVRAsync(InputLVR input);
    }
}