namespace Freetime.Base.Data.Entities
{
    public interface IActivatable
    {
        void ActivateRecord();
        void DeActivateRecord();

        bool IsActive { get; }
    }
}
