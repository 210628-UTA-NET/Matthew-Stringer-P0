using System.Collections.Generic;

namespace p0class
{
    public interface Datastore<T> where T : class
    {
        /// <summary>
        /// Retrieves all of the customers in the datastore.
        /// </summary>
        /// <returns>A list of customers.</returns>
        public List<T> LoadAllRecords();
        
        /// <summary>
        /// Loads a given customer based on search criteria given via an instantiated customer object.
        /// </summary>
        /// <returns>A matching customer object, or null.</returns>
        public T LoadRecord(T p_rec);

        /// <summary>
        /// Adds a customer object to the datastore.
        /// </summary>
        /// <returns>True for success, false for failure.</returns>
        public bool AddRecord(T p_cust);
    }

}