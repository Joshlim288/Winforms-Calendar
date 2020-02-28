using System.Collections.Generic;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// CalendarItemCollection
    /// </summary>
    public class CalendarItemCollection : List<CalendarItem>
    {
        #region Ctor

        /// <summary>
        /// Creates a new collection that will belong to the specified calendar.
        /// </summary>
        /// <param name="c">Calendar this collection will belong to.</param>
        public CalendarItemCollection(Calendar c)
        {
            this.m_Calendar = c;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the calendar this collection belongs to
        /// </summary>
        private Calendar m_Calendar;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the end of the list
        /// </summary>
        /// <param name="item">The object to be added to the end of the collection. The value can be null for reference types.</param>
        public new void Add(CalendarItem item)
        {
            base.Add(item);
            CollectionChanged();
        }

        /// <summary>
        /// Adds the items of the specified collection to the end of the list.
        /// </summary>
        /// <param name="items">The items whose elements should be added to the end of the collection. The collection itself cannont be null, but it can contain elements that are null.</param>
        public new void AddRange(IEnumerable<CalendarItem> items)
        {
            base.AddRange(items);
            CollectionChanged();
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public new void Clear()
        {
            base.Clear(); 
            CollectionChanged();
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        public new void Insert(int index, CalendarItem item) 
        {
            base.Insert(index, item); 
            CollectionChanged();
        }

        /// <summary>
        /// Inserts the items of a collection into this collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="items"></param>
        public new void InsertRange(int index, IEnumerable<CalendarItem> items) 
        {
            base.InsertRange(index, items); 
            CollectionChanged();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The item to remove from the collection. The value can be null for reference types.</param>
        /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns false if item was not found in the collection.</returns>
        public new bool Remove(CalendarItem item) 
        {
            bool result = base.Remove(item); 
            
            CollectionChanged();

            return result;
        }

        /// <summary>
        /// Removes the item at the specified index of the collection
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <returns></returns>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index); 
            CollectionChanged();
        }

        /// <summary>
        /// Removes the all the items that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the conditions of the items to remove.</param>
        /// <returns>The number of items removed from the collection.</returns>
        public new int RemoveAll(Predicate<CalendarItem> match) 
        {
            int result = base.RemoveAll(match); 
            
            CollectionChanged();

            return result;
        }

        /// <summary>
        /// Removes a range of elements from the collection
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of items to remove.</param>
        /// <param name="count">The number of items to remove</param>
        public new void RemoveRange(int index, int count) 
        {
            base.RemoveRange(index, count); 
            CollectionChanged();
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1"/> using the ascending start date.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default"/> cannot find an implementation of the <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface for type <paramref name="T"/>.
        /// </exception>
        public new void Sort()
        {
            // Lamda style.
            base.Sort((item1, item2) =>
            {
                if (this.m_Calendar.DaysMode == DaysModes.Expanded)
                    return DateTime.Compare(item1.StartDate, item2.StartDate);

                return DateTime.Compare(item2.StartDate, item1.StartDate);
            });

            // Anonymous style.
            /*
            base.Sort(delegate(CalendarItem item1, CalendarItem item2)
            {
                if (this._calendar.DaysMode == DaysModes.Expanded)
                    return DateTime.Compare(item1.StartDate, item2.StartDate);
                
                return DateTime.Compare(item2.StartDate, item1.StartDate);
            });
            */
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles what to do when this collection changes
        /// </summary>
        private void CollectionChanged()
        {
            this.Sort();
            if (m_Calendar.m_Updating)
                return;

            m_Calendar.Renderer.PerformItemsLayout();
            m_Calendar.Invalidate();
        }

        #endregion
    }
}
