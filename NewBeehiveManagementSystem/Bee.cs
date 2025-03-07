namespace NewBeehiveManagementSystem;

abstract class Bee : IWorker
{
    public string Job { get; private set; }
    public abstract float CostPerShift { get; }

    public Bee(string job)
    {
        Job = job;
    }

    public void WorkTheNextShift()
    {
        if (HoneyVault.ConsumeHoney(CostPerShift))
        {
            DoJob();
        }
    }
    protected abstract void DoJob();
}
class Queen : Bee
{
    const float EGGS_PER_SHIFT = 0.45f;
    const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;

    private IWorker[] workers = new IWorker[0];
    private float eggs = 0;
    private float unassignedWorkers = 3;

    public string StatusReport { get; private set; }
    public override float CostPerShift { get { return 2.15f; } }
    public Queen() : base ("Queen")
    {
        AssignBee("Nectar Collector");
        AssignBee("Honey Manufacturer");
        AssignBee("Egg Care");
    }
    private void AddWorker(Bee worker)
    {
        if (unassignedWorkers >= 1)
        {
            unassignedWorkers--;
            Array.Resize(ref workers, workers.Length + 1);
            workers[workers.Length - 1] = worker;
        }
    }
    private void UpdateStatusReport()
    {
        StatusReport = $"Vault report:\n{HoneyVault.StatusReport}\n" +
        $"\nEgg count: {eggs:0.0}\nUnassigned workers: {unassignedWorkers:0.0}\n" +
        $"{WorkerStatus("Nectar Collector")}\n{WorkerStatus("Honey Manufacturer")}" +
        $"\n{WorkerStatus("Egg Care")}\nTOTAL WORKERS: {workers.Length}";
    }
    private string WorkerStatus(string job)
    {
        int count = 0;
        foreach (IWorker worker in workers)
            if (worker.Job == job) count++;
        string s = "s";
        if (count == 1) s = "";
        return $"{count} {job} bee{s}";
    }
    public void AssignBee(string job) 
    {
        switch (job)
        {
            case "Egg Care":
                AddWorker(new EggCare(this));
                break;
            case "Nectar Collector":
                AddWorker(new NectarCollector());
                break;
            case "Honey Manufacturer":
                AddWorker(new HoneyManufacturer());
                break;
            default:
                break;
        }
        UpdateStatusReport();
    }
    protected override void DoJob()
    {
        eggs += EGGS_PER_SHIFT;
        foreach (IWorker worker in workers)
        {
            worker.WorkTheNextShift();
        }
        HoneyVault.ConsumeHoney(unassignedWorkers * HONEY_PER_UNASSIGNED_WORKER);
        UpdateStatusReport();
    }
    public void CareForEggs(float eggsToConvert)
    {
        if (eggs >= eggsToConvert)
        {
            eggs -= eggsToConvert;
            unassignedWorkers += eggsToConvert;
        }
    }
}
class HoneyManufacturer : Bee
{
    const float NECTAR_PROCESSED_PER_SHIFT = 33.15f;
    public HoneyManufacturer() : base("Honey Manufacturer") { }
    public override float CostPerShift { get { return 1.7f; } }
    protected override void DoJob()
    {
        HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
    }
}
class NectarCollector : Bee
{
    const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;
    public NectarCollector() : base("Nectar Collector") { }
    public override float CostPerShift { get { return 1.95f; } }

    protected override void DoJob() 
    {
        HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
    }
}
class EggCare : Bee
{
    const float CARE_PROGRESS_PER_SHIFT = 0.15f;
    public override float CostPerShift { get { return 1.35f; } }
    private Queen queen;
    public EggCare(Queen queen) : base("Egg Care")
    {
        this.queen = queen;
    }
    protected override void DoJob()
    {
        queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
    }
}
