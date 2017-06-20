using VerkiezingHelper.Helpers.DAL;

namespace VerkiezingHelper.Helpers.Objects
{
    public abstract class BaseObject
    {
        public BaseObject(int? id, string name)
        {
            Id = id;
            Name = name;
        }

        public int? Id { get; protected set; }
        public string Name { get; protected set; }
        public Repository Repository { get; } = new Repository();

        public abstract void Save();
        public abstract void Delete();

        public void EditId(int id)
        {
            Id = id;
        }
    }
}