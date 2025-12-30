namespace Mzstruct.Base.Entities
{
    public class MongoEntity : BaseEntity
    {
        public BaseEvent? Created { get; set; } // = new BaseEvent();
        public BaseEvent? Modified { get; set; }
    }
}
