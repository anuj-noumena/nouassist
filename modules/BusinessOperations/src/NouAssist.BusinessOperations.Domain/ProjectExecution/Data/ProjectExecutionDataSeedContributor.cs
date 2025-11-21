using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.ProjectExecution.Data
{
    /// <summary>
    /// Data seeder for Project Execution module
    /// </summary>
    public class ProjectExecutionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IGuidGenerator _guidGenerator;
        
        public ProjectExecutionDataSeedContributor(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IGuidGenerator guidGenerator)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _guidGenerator = guidGenerator;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _projectRepository.GetCountAsync() > 0)
            {
                return;
            }
            
            await SeedSampleProjectAsync();
        }
        
        private async Task SeedSampleProjectAsync()
        {
            // Create a sample project
            var projectId = _guidGenerator.Create();
            var project = Project.Create(
                projectId,
                "Sample Project",
                "This is a sample project created for demonstration purposes.",
                Clock.Now.Date,
                Clock.Now.Date.AddMonths(3)
            );
            
            // Add phases
            var planningPhase = project.AddPhase(
                "Planning", 
                project.Details.StartDate, 
                project.Details.StartDate.AddDays(14), 
                1);
                
            var implementationPhase = project.AddPhase(
                "Implementation", 
                project.Details.StartDate.AddDays(15), 
                project.Details.StartDate.AddDays(75), 
                2);
                
            var testingPhase = project.AddPhase(
                "Testing", 
                project.Details.StartDate.AddDays(76), 
                project.Details.PlannedEndDate, 
                3);
            
            // Add milestones
            project.AddMilestone(
                "Project Start", 
                project.Details.StartDate, 
                "Official start of the project");
                
            project.AddMilestone(
                "Requirements Complete", 
                project.Details.StartDate.AddDays(14), 
                "All requirements are gathered and approved");
                
            project.AddMilestone(
                "MVP Ready", 
                project.Details.StartDate.AddDays(45), 
                "Minimum Viable Product is ready for review");
                
            project.AddMilestone(
                "Project Complete", 
                project.Details.PlannedEndDate, 
                "Project is completed and delivered");
            
            // Save the project
            await _projectRepository.InsertAsync(project);
            
            // Create tasks for the planning phase
            var task1 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "Requirements Gathering",
                "Gather and document all project requirements",
                16.0,
                TaskPriority.High,
                project.Details.StartDate,
                project.Details.StartDate.AddDays(7)
            );
            
            var task2 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "Project Plan Creation",
                "Create a detailed project plan with timelines and resources",
                8.0,
                TaskPriority.High,
                project.Details.StartDate.AddDays(8),
                project.Details.StartDate.AddDays(14)
            );
            
            // Create tasks for the implementation phase
            var task3 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "Core Architecture Development",
                "Develop the core architecture of the system",
                40.0,
                TaskPriority.Critical,
                project.Details.StartDate.AddDays(15),
                project.Details.StartDate.AddDays(30)
            );
            
            var task4 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "UI/UX Implementation",
                "Implement the user interface and experience design",
                32.0,
                TaskPriority.Medium,
                project.Details.StartDate.AddDays(31),
                project.Details.StartDate.AddDays(45)
            );
            
            // Create tasks for the testing phase
            var task5 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "System Testing",
                "Perform comprehensive system testing",
                24.0,
                TaskPriority.High,
                project.Details.StartDate.AddDays(76),
                project.Details.StartDate.AddDays(85)
            );
            
            var task6 = Task.Create(
                _guidGenerator.Create(),
                projectId,
                "UAT",
                "Conduct User Acceptance Testing",
                16.0,
                TaskPriority.High,
                project.Details.StartDate.AddDays(86),
                project.Details.PlannedEndDate.AddDays(-1)
            );
            
            // Add dependencies
            task2.AddDependency(task1.Id, DependencyType.FinishToStart);
            task3.AddDependency(task2.Id, DependencyType.FinishToStart);
            task4.AddDependency(task3.Id, DependencyType.StartToStart);
            task5.AddDependency(task4.Id, DependencyType.FinishToStart);
            task6.AddDependency(task5.Id, DependencyType.FinishToStart);
            
            // Save the tasks
            await _taskRepository.InsertAsync(task1);
            await _taskRepository.InsertAsync(task2);
            await _taskRepository.InsertAsync(task3);
            await _taskRepository.InsertAsync(task4);
            await _taskRepository.InsertAsync(task5);
            await _taskRepository.InsertAsync(task6);
        }
    }
}
