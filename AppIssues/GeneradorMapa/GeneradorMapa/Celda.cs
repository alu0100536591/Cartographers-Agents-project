﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorMapa
{
    public class Celda : System.Windows.Forms.Label
    {
       // private System.Windows.Forms.PictureBox Img_;
        private int indx_;
        private Form1 parent_;
        
       public Celda(int i, int j, int ind, Form1 p) { 
            indx_ = ind;
            parent_ = p;
            this.Location = new System.Drawing.Point(50+ 32*i, 50 + 32*j);
          //  this.Name = "c";
            this.Size = new System.Drawing.Size(32, 32);
          //  this.TabIndex = i + j;
            this.TabStop = false;
            //this.Image = global::GeneradorMapa.Properties.Resources.blanco;
            ActualizarImagen();
        }
       public int get_index() { return indx_; }
       public void set_index(int i) { indx_ = i; ActualizarImagen(); }
       public void Celda_Click(object sender, EventArgs e){
           indx_ = parent_.get_selected();
           //this.Image = global::GeneradorMapa.Properties.Resources.rojo;
           ActualizarImagen();
       }
       public void ActualizarImagen()
       {
           switch (indx_)
           {
               case 0: this.Image = global::GeneradorMapa.Properties.Resources.rock3; break;
               case 1: this.Image = global::GeneradorMapa.Properties.Resources.Inicio; break;
               case 2: this.Image = global::GeneradorMapa.Properties.Resources.Fin; break;
               case 3: this.Image = global::GeneradorMapa.Properties.Resources.water; break;
               case 4: this.Image = global::GeneradorMapa.Properties.Resources.rock4; break;
           }
       }
    }
}
