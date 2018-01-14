using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MACAWeb
{
    public class MatrixComputationsException : Exception
    {
        public MatrixComputationsException(string message) : base(message) { }
    }

    public static class MatrixComputations
    {
        /// <summary>
        /// Computes the trace of a given matrix
        /// </summary>
        public static double Trace(double[,] matrix)
        {
            double trace = 0;

            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new MatrixComputationsException("MatrixComputations.Trace :: The matrix is not square!");
            }

            for (int i=0; i<matrix.GetLength(0); i++)
            {
                trace += matrix[i, i];
            }
            return trace;
        }
    }

}