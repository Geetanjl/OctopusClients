using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;

namespace Octopus.Client.Repositories.Async
{
    public interface IProjectTriggerRepository : ICreate<ProjectTriggerResource>, IModify<ProjectTriggerResource>, IGet<ProjectTriggerResource>, IDelete<ProjectTriggerResource>
    {
        Task<ProjectTriggerResource> FindByName(ProjectResource project, string name);

        Task<ProjectTriggerEditor> CreateOrModify(ProjectResource project, string name, TriggerFilterResource filter, TriggerActionResource action);
        Task<ResourceCollection<ProjectTriggerResource>> FindByRunbook(params string[] runbookIds);
    }

    class ProjectTriggerRepository : BasicRepository<ProjectTriggerResource>, IProjectTriggerRepository
    {
        public ProjectTriggerRepository(IOctopusAsyncRepository repository)
            : base(repository, "ProjectTriggers")
        {
            MinimumCompatibleVersion("2019.10.8");  // TODO: this needs to be bumped to 2019.11.0 just as we are ready to go live
        }

        public Task<ProjectTriggerResource> FindByName(ProjectResource project, string name)
        {
            return FindByName(name, path: project.Link("Triggers"));
        }

        public Task<ProjectTriggerEditor> CreateOrModify(ProjectResource project, string name, TriggerFilterResource filter, TriggerActionResource action)
        {
            ThrowIfServerVersionIsNotCompatible().ConfigureAwait(false);
            
            return new ProjectTriggerEditor(this).CreateOrModify(project, name, filter, action);
        }

        public async Task<ResourceCollection<ProjectTriggerResource>> FindByRunbook(params string[] runbookIds)
        {
            await ThrowIfServerVersionIsNotCompatible();
            
            return await Client.List<ProjectTriggerResource>(await Repository.Link("Triggers"), new { runbooks = runbookIds });
        }
    }
}
