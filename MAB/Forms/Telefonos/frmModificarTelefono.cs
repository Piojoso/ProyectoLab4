﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MAB.Models;

namespace MAB.Forms.Telefonos
{
    public partial class frmModificarTelefono : Form
    {
        private Models.Telefonos Telefono;

        public frmModificarTelefono(int idCliente, long numTelefono)
        {
            /**
             * TODO: Probar correcto funcionamiento.
             */
            InitializeComponent();

            Text = "Modificar el Numero: " + Telefono.telefono;

            ucBottom.Accion1 = "Modificar";
            ucBottom.Accion2 = "Cerrar";

            ucBottom.evAccion1 += modificarNumero;
            ucBottom.evAccion2 += cerrarVentana;
        }

        private void cargarTelefono(int idCliente, long numTelefono)
        {
            using (MABEntities db = new MABEntities())
            {
                Telefono = db.Telefonos.Find(idCliente, numTelefono);

                cclblNombreCliente.Text = Telefono.Cliente.nombre + " " + Telefono.Cliente.apellido;

                cctbNumTelefono.Text = Telefono.telefono.ToString();
            }
        }

        private void modificarNumero(object sender, EventArgs e)
        {
            if(cctbNumTelefono.Text != string.Empty)
            {
                if(Convert.ToInt64(cctbNumTelefono.Text) != Telefono.telefono)
                {
                    DialogResult resp = MessageBox.Show("El numero de telefono cambiara de: " + Telefono.telefono + " a " + cctbNumTelefono.Text + "\n" +
                        "¿Desea Continuar?", "Atencion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if(resp == DialogResult.Yes)
                    {
                        using (MABEntities db = new MABEntities())
                        {
                            Telefono.telefono = Convert.ToInt64(cctbNumTelefono.Text);

                            db.Entry(Telefono).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El numero de telefono no ha cambiado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El campo Telefono es invalido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cerrarVentana(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}