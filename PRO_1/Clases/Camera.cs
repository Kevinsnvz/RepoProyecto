using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    public class Camera
    {
        private int _cameraip;
        public int CameraIP
        {
            get { return _cameraip; }
            set { _cameraip = value; }
        }

        public string CameraName {  get; set; }

        public Camera(int cameraip, string cameraname)
        {
            this.CameraIP = cameraip;
            this.CameraName = cameraname;
        }
    }
}
