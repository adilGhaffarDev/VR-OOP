
public interface IManager
{
    /// <summary>
    /// to add event listeners
    /// </summary>
    void Load();

    /// <summary>
    /// clean up listeners and all set up
    /// </summary>
    void Cleanup();
}
