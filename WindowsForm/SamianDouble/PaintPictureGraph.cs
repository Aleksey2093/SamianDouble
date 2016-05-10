using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamianDouble
{
    public class PaintPictureGraph
    { 
        public PictureBox пЕрерисовка(PictureBox pictureBox,List<Node_struct> listnode)
        {
            pictureBox.Image = null;
            List<Node_struct> proНарисован = new List<Node_struct>();
            int Size = 100;
            Random rand = new Random();
            foreach(var nod in listnode)
            {
            ццц:
                nod.cordx = rand.Next(101, pictureBox.Size.Width - 100);
                nod.cordy = rand.Next(101, pictureBox.Size.Height - 100);
                foreach (var waw in proНарисован)
                {
                    if (Math.Sqrt(Math.Pow(nod.cordx-waw.cordx,2)-Math.Pow(nod.cordy-waw.cordy,2)) < Size*2+1)
                    {
                        goto ццц;
                    }
                }
                proНарисован.Add(nod);
                pictureBox.CreateGraphics().DrawEllipse(new Pen(Color.Black), nod.cordx - Size/2, nod.cordy - Size/2, Size, Size);
            }
            return pictureBox;
        }
    }
}
