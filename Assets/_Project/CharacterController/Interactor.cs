public class Interactor : Collector<Interactable>
{
    public override void OnCollect(Interactable item)
    {
        item.EnterRange();
    }

    public override void OnDrop(Interactable item)
    {
        item.ExitRange();
    }

    public bool TryInteract()
    {
        if (Count <= 0) return false;
        GetClosest().Interact();
        return true;
    }
}