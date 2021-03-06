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
using MAB.Forms.Clientes;

namespace MAB.Forms.Lavarropas
{
    public partial class frmSeleccionarLavarropas : Form
    {
        public frmSeleccionarLavarropas()
        {
            InitializeComponent();

            cargarLavarropas();

            ucBottom.Accion1 = "Seleccionar";
            ucBottom.Accion2 = "Buscar";
            ucBottom.Accion3 = "Nuevo";

            ucBottom.evAccion1 += seleccionarLavarropas;
            ucBottom.evAccion2 += buscarLavarropas;
            ucBottom.evAccion3 += agregarNuevo;
        }

        private int idLavarropas = -1;

        public int lavarropasSeleccionado
        {
            get { return idLavarropas; }
        }

        private void cargarLavarropas()
        {
            using (MABEntities db = new MABEntities())
            {
                var lavarropas = db.Lavarropas;

                ucDGVTabla.dataSource(lavarropas.ToList());
            }

            Text = "Seleccione un Lavarropas";

            ucDGVTabla.Columns["Id"].Visible = false;
            ucDGVTabla.Columns["ClienteId"].Visible = false;
            ucDGVTabla.Columns["Cliente"].Visible = false;
            ucDGVTabla.Columns["Reparacion"].Visible = false;
        }

        #region Eventos de los botones

        private void seleccionarLavarropas(object sender, EventArgs e)
        {
            if(ucDGVTabla.selectedRow() != null)
            {
                int idLavarropas = Convert.ToInt32(ucDGVTabla.selectedRow().Cells["Id"].Value);

                using (MABEntities db = new MABEntities())
                {
                    Models.Lavarropas lavarropas = db.Lavarropas.Find(idLavarropas);
                    
                    this.idLavarropas = lavarropas.Id;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("No hay ningun Lavarropas seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buscarLavarropas(object sender, EventArgs e)
        {
            frmBuscarLavarropas frm = new frmBuscarLavarropas();
            frm.ShowDialog();

            if (frm.getResultados.Count != 0)
            {
                using (MABEntities db = new MABEntities())
                {
                    List<Models.Lavarropas> lavarropas = new List<Models.Lavarropas>();

                    foreach (int id in frm.getResultados)
                    {
                        var lav = db.Lavarropas.Find(id);

                        if(!lavarropas.Contains(lav))
                        {
                            lavarropas.Add(lav);
                        }
                    }

                    ucDGVTabla.dataSource(lavarropas);
                }
            }
        }

        private void agregarNuevo(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Se necesita un cliente para poder agregarle un Lavarropas al mismo. \n" +
                "Es por este motivo que a continuacion se abrira una ventana para que pueda seleccionar uno.",
                "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);

            frmSeleccionarCliente frmSeleccionarCliente = new frmSeleccionarCliente();
            frmSeleccionarCliente.ShowDialog();

            int idCliente = frmSeleccionarCliente.ClienteSeleccionado;

            if (idCliente != -1)
            {
                frmAgregarLavarropas frm = new frmAgregarLavarropas(idCliente);
                frm.ShowDialog();
            }

            cargarLavarropas();
        }

        #endregion
    }
}
