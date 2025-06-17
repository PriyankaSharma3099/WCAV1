using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Services
{
    public interface IImage
    {
        string Prefix { get; set; }
        ImageSource Implementation { get; set; }
        bool Extension { get; set; }
    }
}
