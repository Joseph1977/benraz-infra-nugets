using System;
using System.Collections.Generic;
using System.Text;

namespace Benraz.Infrastructure.Domain.Common
{
    /// <summary>
    /// Export file status code.
    /// </summary>
    public enum ExportFileStatusCode
    {
        /// <summary>
        /// Pending.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// In progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Ready.
        /// </summary>
        Ready = 3,

        /// <summary>
        /// Removed.
        /// </summary>
        Removed = 4,

        /// <summary>
        /// Error.
        /// </summary>
        Error = 5
    }
}



