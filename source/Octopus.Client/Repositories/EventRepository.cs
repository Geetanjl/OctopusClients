using System;
using Octopus.Client.Model;
using Octopus.Client.Util;

namespace Octopus.Client.Repositories
{
    public interface IEventRepository : IGet<EventResource>, ICanLimitToSpaces<IEventRepository>
    {
        [Obsolete("This method was deprecated in Octopus 3.4.  Please use the other List method by providing named arguments.")]
        ResourceCollection<EventResource> List(int skip = 0, 
            string filterByUserId = null,
            string regardingDocumentId = null,
            bool includeInternalEvents = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="take">Number of records to take (First supported in Server 3.14.159)</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="regarding"></param>
        /// <param name="regardingAny"></param>
        /// <param name="includeInternalEvents"></param>
        /// <param name="user"></param>
        /// <param name="users"></param>
        /// <param name="projects"></param>
        /// <param name="environments"></param>
        /// <param name="eventGroups"></param>
        /// <param name="eventCategories"></param>
        /// <param name="tenants"></param>
        /// <param name="tags"></param>
        /// <param name="fromAutoId"></param>
        /// <param name="toAutoId"></param>
        /// <param name="documentTypes"></param>
        /// <returns></returns>
        ResourceCollection<EventResource> List(int skip = 0, 
            int? take = null,
            string from = null,
            string to = null,
            string regarding = null,
            string regardingAny = null,
            bool includeInternalEvents = true,
            string user = null,
            string users = null,
            string projects = null,
            string environments = null,
            string eventGroups = null,
            string eventCategories = null,
            string tenants = null,
            string tags = null,
            long? fromAutoId = null,
            long? toAutoId = null,
            string documentTypes = null);
    }
    
    class EventRepository : MixedScopeBaseRepository<EventResource>, IEventRepository
    {
        public EventRepository(IOctopusClient client)
            : base(client, "Events", null)
        {
        }

        EventRepository(IOctopusClient client, SpaceQueryParameters spaceQueryParameters): base(client, "Events", spaceQueryParameters)
        {
        }

        [Obsolete("This method was deprecated in Octopus 3.4.  Please use the other List method by providing named arguments.")]
        public ResourceCollection<EventResource> List(int skip = 0, 
            string filterByUserId = null,
            string regardingDocumentId = null,
            bool includeInternalEvents = false)
        {
            return Client.List<EventResource>(Client.Link("Events"), new
            {
                skip,
                user = filterByUserId,
                regarding = regardingDocumentId,
                @internal = includeInternalEvents.ToString()
            });
        }

        public ResourceCollection<EventResource> List(int skip = 0, 
            int? take = null,
            string from = null,
            string to = null,
            string regarding = null,
            string regardingAny = null,
            bool includeInternalEvents = true,
            string user = null,
            string users = null,
            string projects = null,
            string environments = null,
            string eventGroups = null,
            string eventCategories = null,
            string tenants = null,
            string tags = null,
            long? fromAutoId = null,
            long? toAutoId = null,
            string documentTypes = null)
        {
            var parameters = ParameterHelper.CombineParameters(AdditionalQueryParameters, new
            {
                skip,
                take,
                from,
                to,
                regarding,
                regardingAny,
                @internal = includeInternalEvents,
                user,
                users,
                projects,
                environments,
                eventGroups,
                eventCategories,
                tenants,
                tags,
                fromAutoId,
                toAutoId,
                documentTypes
            });
            return Client.List<EventResource>(Client.Link("Events"), parameters);
        }

        public IEventRepository LimitTo(bool includeSystem, params string[] spaceIds)
        {
            var newParameters = this.CreateParameters(includeSystem, spaceIds);
            return new EventRepository(Client, newParameters);
        }
    }
}