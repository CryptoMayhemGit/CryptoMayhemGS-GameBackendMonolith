using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Dto.Dtos
{
    public class LandOperations<T>
    {
        public LandOperationsType OperationType { get; set; }
        public T Operation { get; set; }
    }
}
