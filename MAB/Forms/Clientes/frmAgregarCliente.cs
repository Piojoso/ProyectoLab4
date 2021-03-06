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
using MAB.Forms.CRUD.Telefonos;
using MAB.Forms.Lavarropas;

namespace MAB.Forms.CRUD.Clientes
{
    public partial class frmAgregarCliente : Form
    {
        public frmAgregarCliente()
        {
            /**
             * TODO: Revisar el correcto funcionamiento
             */

            InitializeComponent();
            
            ucBottom.Accion1 = "Crear";
            ucBottom.Accion2 = "Cerrar";

            ucBottom.evAccion1 += crearCliente;
            ucBottom.evAccion2 += cancelarCreacion;

            string messageError = "Solo se permiten Letras, no se permiten numeros";

            cctbNombre.CaracterIncorrectErrorMessage = messageError;
            cctbApellido.CaracterIncorrectErrorMessage = messageError;
        }

        private void crearCliente(object sender, EventArgs e)
        {
            int idCliente = guardarCliente();

            if(idCliente != -1)
            {
                DialogResult resp = MessageBox.Show(
                    "¿Desea agregarle telefonos a este cliente?", 
                    "¿Agregar Telefonos?", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning
                );

                if (resp == DialogResult.Yes)
                {
                    frmAgregarTelefono frm = new frmAgregarTelefono(idCliente);
                    frm.ShowDialog();
                }

                resp = MessageBox.Show(
                    "¿Desea registrar Lavarropas a este Cliente?", 
                    "¿Agregar Lavarropas?", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning
                );

                if(resp == DialogResult.Yes)
                {
                    frmAgregarLavarropas frm = new frmAgregarLavarropas(idCliente);
                    frm.ShowDialog();
                }

                resp = MessageBox.Show(
                    "Cliente agregador correctamente",
                    "Guardado Correctamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                
                cctbNombre.Text = "";
                cctbApellido.Text = "";
                cctbDireccion.Text = "";

                cctbNombre.Focus();
            }
        }

        private void cancelarCreacion(object sender, EventArgs e)
        {
            this.Close();
        }

        private int guardarCliente()
        {
            Models.Clientes cliente;

            if ((cctbApellido.Text != string.Empty) && (cctbDireccion.Text != string.Empty))
            {
                using (MABEntities db = new MABEntities())
                {
                    cliente = new Models.Clientes();

                    cliente.nombre = cctbNombre.Text;
                    cliente.apellido = cctbApellido.Text;
                    cliente.direccion = cctbDireccion.Text;

                    db.Clientes.Add(cliente);
                    db.SaveChanges();

                    return cliente.Id;
                }
            }
            else
            {
                MessageBox.Show("Faltan campos por completar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return -1;
        }
    }
}
