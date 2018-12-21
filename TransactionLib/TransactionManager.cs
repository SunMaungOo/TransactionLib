using System;
using System.Collections.Generic;

namespace TransactionLib
{
    /// <summary>
    /// When we wanted to link multiple method in a transaction pattern
    /// Author = Sun Maung Oo
    /// </summary>
    public sealed class TransactionManager : IDisposable
    {
        private bool isCommited;

        private bool isDisposed;

        private IList<TransactionWrapper> wrapperList;

        /// <summary>
        /// Should only be created in using(...) statement
        /// </summary>
        public TransactionManager()
        {

            isCommited = false;

            isDisposed = false;

            wrapperList = new List<TransactionWrapper>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="rollbackMethod"></param>
        public void Create(Action method, Action rollbackMethod)
        {
            try
            {
                wrapperList.Add(new TransactionWrapper(method, rollbackMethod));
            }
            catch
            {

                Dispose();

                //rethrow whatever the exception is caught

                throw;
            }
        }

        /// <summary>
        /// Commit an method.
        /// Provide guarantee that the method success if you can call this method.
        /// In other word the program will not execute this method line number if the 
        /// exception occur.
        /// </summary>
        public void Commit()
        {
            isCommited = true;

            foreach (TransactionWrapper wrapper in wrapperList)
            {
                wrapper.Commit();
            }
        }

        /// <summary>
        /// Warning : Dispose() method may only call in using(...) statement in order
        /// to provide a transaction.
        /// 
        /// Calling Dispose() method other than in using(...) statement break the 
        /// transaction pattern.
        /// 
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;

                if (!isCommited)
                {
                    foreach (TransactionWrapper wrapper in wrapperList)
                    {
                        wrapper.Dispose();
                    }
                }

                wrapperList.Clear();
            }
        }


    }
}
