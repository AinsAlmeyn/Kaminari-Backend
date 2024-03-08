namespace WMS.Core.Entities.ConnectionAndSettings.MongoDb
{
    /// <summary>
    /// Represents a sort option for MongoDB queries.
    /// </summary>
    public class SortOption
    {
        /// <summary>
        /// Gets or sets the name of the field to sort by.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sort order is ascending or descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the sort order is ascending; otherwise, <c>false</c>.
        /// </value>
        public bool Ascending { get; set; } = true;
    }
}