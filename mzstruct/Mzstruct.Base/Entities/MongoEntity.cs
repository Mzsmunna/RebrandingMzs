namespace Mzstruct.Base.Entities
{
    public class MongoEntity : BaseEntity
    {
        public AppEvent? Created { get; set; } // = new AppEvent();
        public AppEvent? Modified { get; set; }
    }
}
