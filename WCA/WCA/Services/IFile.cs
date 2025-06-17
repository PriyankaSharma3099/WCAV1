using System;
using System.Collections.Generic;
using System.Text;

namespace WCA.Services
{
    public interface IFile
    {
        string GetLocalPath(string archivo);
    }
}
