using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using WCA.Services;

namespace WCA.Droid.Classes
{
    public class ImageImp : IImage
    {
        public string Prefix { get; set; }
        public ImageSource Implementation { get; set; }
        public bool Extension { get; set; }

        public ImageImp()
        {
            Implementation = new AndroidImageSource();
            Prefix = string.Empty;
            Extension = false;
        }
    }

}
