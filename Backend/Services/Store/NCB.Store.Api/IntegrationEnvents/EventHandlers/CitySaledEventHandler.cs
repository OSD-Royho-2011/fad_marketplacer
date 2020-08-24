using NCB.Core.DataAccess.BaseRepository;
using NCB.EventBus.Abstractions;
using NCB.Store.Api.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.Entities;
using NCB.Store.Api.IntegrationEnvents.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.IntegrationEnvents.EventHandlers
{
    public class CitySaledEventHandler : IEventHandler
    {
        private readonly IBaseRepository<City> _cityRepository;
        public CitySaledEventHandler(IBaseRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task Handle(dynamic eventData)
        {
            try
            {
                var ev = (CitySaledEvent)JsonConvert.DeserializeObject(eventData.ToString(), typeof(CitySaledEvent));

                var city = new City
                {
                    Name = ev.Name,
                    Zipcode = ev.Zipcode
                };

                 _cityRepository.InsertAsync(city);
                await _cityRepository.SaveAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            await Task.CompletedTask;
        }
    }
}
