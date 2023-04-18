namespace TimeTracker.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IActivityRepository ActivityRepository { get; }

        IActivityTypeRepository ActivityTypeRepository { get; }

        IEmployeeRepository EmployeeRepository { get; }

        IProjectRepository ProjectRepository { get; }

        IRoleRepository RoleRepository { get; }

        Task SaveAsync();
    }
}
