namespace SendMe.Services
{
    public interface IPersistence<T> where T: IModel
    {
        public Task<T> Save(T entity);
        public Task Remove(string id);
        public Task<IEnumerable<T>> FindAll();
        public Task<T> FindByID(string id);
        public Task<IEnumerable<T>> FindByEqualsFilter<V>(string property, V value);
    }
}
