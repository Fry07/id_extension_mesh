﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public class SplitButton : Button //class library project, should be compiled separately and then added to constructor
    {
        public ContextMenuStrip Cms = new ContextMenuStrip();
        Bitmap BmpSplit;

        private String[] Cole = new String[10];

        public String[] ColeItems
        {
            get { return Cole; }
            set { Cole = value; }
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            Split();

            for (int cont = 0; cont < Cole.LongLength; cont++)
            {
                Cms.Items.Add(Cole[cont]);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (Position()) { Cms.Show(this, 0, this.Height); }
        }

        private bool Position()
        {
            bool pos = false;

            int X = PointToClient(MousePosition).X;
            int Y = PointToClient(MousePosition).Y;

            if (X > 0 && X < this.Width && Y > 0 && Y < this.Height)
            {
                pos = true;
            }
            return pos;
        }

        private void Split()
        {
            BmpSplit = new Bitmap(30, this.Height);

            Graphics gra = Graphics.FromImage(BmpSplit);

            gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            Point[] pt = { new Point(20, BmpSplit.Height / 2 - 5), new Point(10, BmpSplit.Height / 2 - 5), new Point(15, BmpSplit.Height / 2) };

            gra.FillPolygon(new SolidBrush(Color.Black), pt);
            gra.DrawLine(new Pen(Color.Silver), new Point(0, BmpSplit.Height / 4), new Point(0, BmpSplit.Height - BmpSplit.Height / 4));

            this.ImageAlign = ContentAlignment.MiddleRight;
            this.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Image = BmpSplit;
        }
    }
}