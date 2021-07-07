using System.Collections.Generic;

namespace p0class
{
    public interface Datastore<T> where T : class
    {
        /// <summary>
        /// Retrieves all of the records in the datastore.
        /// </summary>
        /// <returns>A list of records.</returns>
        public List<T> LoadAllRecords();
        
        /// <summary>
        /// Loads a given record based on search criteria given via an instantiated record object.
        /// </summary>
        /// <returns>A matching record object, or null.</returns>
        public T LoadRecord(T p_rec);

        /// <summary>
        /// Adds a record object to the datastore.
        /// </summary>
        /// <returns>True for success, false for failure.</returns>
        public bool AddRecord(T p_cust);
    }

}