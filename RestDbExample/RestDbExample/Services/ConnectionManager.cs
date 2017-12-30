using RestDbExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.Services
{
    public class ConnectionManager
    {
        ServiceBroker serviceBroker;

        public ConnectionManager(ServiceBroker service)
        {
            serviceBroker = service;
        }

        public Task<List<ProductHuntPost>> GetPostsAsync()
        {
            return serviceBroker.RefreshDataAsync();
        }

        public Task<List<ProductHuntPost>> GetPostsByDateAsync(DateTime filterByDate)
        {
            return serviceBroker.RefreshDataByDateAsync(filterByDate);
        }

        public Task<ProductHuntPost> GetPostDetailsAsync(int iD)
        {
            return serviceBroker.RetrievePostDetailsAsync(iD);
        }

        public Task<Votes> GetPostVotesAsync(int iD)
        {
            return serviceBroker.RetrievePostVotesAsync(iD);
        }
    }
}
