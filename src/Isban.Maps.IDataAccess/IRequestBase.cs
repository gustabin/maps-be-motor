﻿using Isban.Common.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.IDataAccess
{
    public interface IRequestBase
    {  /// <summary>
       /// Gets or sets the codigo error.
       /// </summary>
       /// <value>
       /// The codigo error.
       /// </value>

        long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>

        string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>

        string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>        
        string Usuario { get; set; }

        /// <summary>
        /// Método para verificar si existen errores en los contenidos
        /// </summary>
        void CheckError();
    }
}
