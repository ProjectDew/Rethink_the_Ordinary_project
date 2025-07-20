public struct Interaction
{
    public InteractableObject Object { get; private set; }

    public Task Task { get; private set; }

    public Interaction (InteractableObject interactableObject, Task task)
    {
        Object = interactableObject;
        Task = task;
    }
}
