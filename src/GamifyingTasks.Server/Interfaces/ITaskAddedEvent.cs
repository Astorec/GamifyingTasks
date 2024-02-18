namespace GamifyingTasks.Interfaces{
    public interface ITaskAddedEvent{
        protected EventHandler OnTaskAddedHandler {get ; set;}

        event EventHandler OnTaskAdded{
            add => OnTaskAddedHandler += value;
            remove => OnTaskAddedHandler -= value;
        }
    }
}