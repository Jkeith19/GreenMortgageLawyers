using AutoMapper;
using BusinessServices.Services.Mappers;
using Entity.DTOs;
using Entity.Entity;
using Entity.Entity.Models;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessServices.Services
{
    public class LvrService : ILvrService
    {
        private readonly LvrDbContext _dbContext;
        private readonly ILvrMapper _mapperConfig;
        private readonly IMapper _mapper;

        public LvrService(LvrDbContext dbContext, ILvrMapper mapperConfig)
        {
            _dbContext = dbContext;
            _mapperConfig = mapperConfig;
            _mapper = _mapperConfig.LvrConfiguration();
        }

        private async Task<bool> CheckLoanRecordAsync(decimal lvr, InputLVR input)
        {
            return await _dbContext.LVR.AnyAsync(record => record.LoanValuationRatio == lvr
                                                 && record.PropertyValue == input.PropertyValue
                                                 && record.LoanAmount == input.LoanAmount);
        }

        private async Task<LVR> SaveLVRAsync(decimal lvr, InputLVR input)
        {
            var loanRecord = new LVR
            {
                LoanAmount = input.LoanAmount,
                PropertyValue = input.PropertyValue,
                LoanValuationRatio = lvr
            };

            _dbContext.LVR.Add(loanRecord);
            await _dbContext.SaveChangesAsync();

            return loanRecord;
        }

        public async Task<LVRDto> CalculateLVRAsync(InputLVR input)
        {
            if (input == null || input.PropertyValue <= 0 || input.LoanAmount <= 0)
            {
                return new LVRDto
                {
                    Message = "Invalid input data.",
                    StatusCode = 400
                };
            }

            var lvr = (input.LoanAmount / input.PropertyValue) * 100;

            var isLvrExists = await CheckLoanRecordAsync(lvr, input);

            if (isLvrExists)
            {
                return new LVRDto
                {
                    Message = "This LVR already exists in the database.",
                    StatusCode = 400
                };
            }

            var response = await SaveLVRAsync(lvr, input);

            var result = _mapper.Map<LVRDto>(response);
            result.Message = "Record saved successfully.";
            result.StatusCode = 201;

            return result;
        }
    }
}
