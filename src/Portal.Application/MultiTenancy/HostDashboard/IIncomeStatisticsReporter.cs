using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.MultiTenancy.HostDashboard.Dto;

namespace Portal.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}