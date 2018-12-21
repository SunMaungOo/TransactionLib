using System;

namespace TransactionLib
{
    /// <summary>
    /// When we want an transaction pattern for single method.
    /// If the exception is thrown, the rollback method is automatically called
    /// Author = Sun Maung Oo
    /// </summary>
    public sealed class TransactionWrapper : IDisposable
    {
        private Action rollbackMethod;

        private bool isCommited;

        private bool isDisposed;

        /// <summary>
        /// Should only be created in using(...) statement
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="rollbackMethod"> Method which will execute to rollback the change provided </param>
        public TransactionWrapper(Action method, Action rollbackMethod)
        {
            this.rollbackMethod = rollbackMethod;

            isCommited = false;

            isDisposed = false;

            try
            {
                method();
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
            //if it haven't been dispose yet

            if (!isDisposed)
            {
                isDisposed = true;

                if (!isCommited && rollbackMethod != null)
                {
                    rollbackMethod();
                }
            }

        }

    }
}
