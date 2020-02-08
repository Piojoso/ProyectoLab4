﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MAB.Forms.CRUD.Clientes;
using MAB.Models;

namespace MAB.Forms.CRUD.Reparaciones
{
    public partial class frmAgregarReparaciones : Form
    {
        public frmAgregarReparaciones()
        {
            InitializeComponent();

            ucTop.Titulo = "Agregar nueva Reparacion";

            ucBottom.NumButtons = 2;

            ucBottom.Accion1 = "Agregar";
            ucBottom.Accion3 = "Cancelar";

            ucBottom.evAccion1 += agregarReparacion;
            ucBottom.evAccion3 += cancelar;
        }

        private void agregarReparacion(object sender, EventArgs e)
        {
            throw new Exception("No listo para su uso");

            using (MABEntities db = new MABEntities())
            {
                Models.Reparaciones reparacion = new Models.Reparaciones();

                reparacion.fechaIngreso = DateTime.Now;
                reparacion.fechaEgreso = null;
                reparacion.errorAReparar = cctbErrorAReparar.Text;
                reparacion.estadoReparacion = estadosReparacion.EnCurso;
                reparacion.mesesGarantia = null;
                reparacion.reparacionRealizada = "";
                reparacion.manoDeObra = 0;
                reparacion.totalRepuestos = 0;
                //SOSPECHO QUE ESTO VA A TIRAR ERROR, POSIBLE PROBLEMA: 
                // mi idea es que aparezca la marca y modelo del lavarropa del cliente.
                // si pudiera hacer algo como en html, que el value sea el id y el texto sea la marca y el modelo, seria FANTASTICO
                reparacion.LavarropasId = Convert.ToInt32(cboLavarropas.SelectedItem);

                db.Reparaciones.Add(reparacion);
            }
        }

        private void cancelar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("¿Desea crear un nuevo Cliente?", "¿Crear uno Nuevo?", MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if (resp == DialogResult.Yes)
            {
                frmAgregarCliente frm = new frmAgregarCliente();
                frm.ShowDialog();
            }
            else
            {
                frmBuscarCliente frm = new frmBuscarCliente();
                frm.ShowDialog();

                if(frm.DialogResult != DialogResult.Cancel)
                {
                    if (frm.idClientes.Count != 1)
                    {
                        MessageBox.Show("Hubo Multiples resultados en la ultima busqueda. Pruebe Brindando mas Informacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        using (MABEntities db = new MABEntities())
                        {
                            var cliente = db.Clientes.Find(frm.idClientes[0]);

                            cclblNombreCliente.Text = cliente.nombre + " " + cliente.apellido;

                            // REVISAR COMO SE VE ESTO, SI NO ME GUSTA (SOLO ID) TENDRE QUE VER COMO HACER PARA ENSEÑAR NOMBRE
                            cboLavarropas.DataSource = cliente.Lavarropas;
                        }
                    }
                }
            }
        }

        private void btnNuevoLavarropas_Click(object sender, EventArgs e)
        {
            /**
             * Aca mi idea es iniciar el formulario de agregar Lavarropas (Aun no existe)
             * Luego de que se cierre el formulario debo Refrescar el cbo.
             */
            throw new NotImplementedException();
        }

        private void cboLavarropas_SelectedIndexChanged(object sender, EventArgs e)
        {
            /**
             * Esto fue creado para hacer algo del estilo de...
             * Al seleccionar un item, tomo el id, y busco el lavarropas, y lo asigno al objeto que luego sera la reparacion.
             */
            throw new NotImplementedException();
        }
    }
}
