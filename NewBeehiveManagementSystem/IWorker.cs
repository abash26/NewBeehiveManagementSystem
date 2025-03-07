namespace NewBeehiveManagementSystem;

internal interface IWorker
{
    string Job { get; }
    void WorkTheNextShift();
}
