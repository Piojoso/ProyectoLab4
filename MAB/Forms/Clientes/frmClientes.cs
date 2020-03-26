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

namespace MAB.Forms.CRUD.Clientes
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();

            ucBG.Titulo = "Ultimos Clientes";

            ucBG.Accion1 = "Agregar";
            ucBG.Accion2 = "Modificar";
            ucBG.Accion3 = "Ver Todos";

            ucBG.evAccion1 += btnAgregar;
            ucBG.evAccion2 += btnModificar;
            ucBG.evAccion3 += btnVerTodos;

            cargarDGV();
        }

        void btnAgregar(object sender, EventArgs e)
        {
            frmAgregarCliente frm = new frmAgregarCliente();
            frm.ShowDialog();
        }

        void btnModificar(object sender, EventArgs e)
        {
            DataGridViewRow fila = ucBG.getSelectedItem();

            using (MABEntities db = new MABEntities())
            {
                /**
                 * TODO: Revisar el funcionamiento de esta funcion.
                 * Puede que al mandarle un object, la funcion find, realize otra cosa que no sea la que yo deseo.
                 * En tal caso, la posible solucion seria convetir el value a int. Para que asi busque por id.
                 * 
                 * -------------------------------------------------------------------------------------------------
                 * // TAMBIEN TENGO ESTE POSIBLE PROBLEMA EN LOS TELEFONOS
                 * Ese comentario lo puse hace un tiempo y no logro entender donde estaria el problema con el value. Luego debo revisarlo.
                 * 
                 */ 
                Models.Clientes cliente = db.Clientes.Find(fila.Cells[0].Value);

                frmModificarCliente frm = new frmModificarCliente(cliente.Id);
                frm.ShowDialog();

                cargarDGV();
            }
        }

        void btnVerTodos(object sender, EventArgs e)
        {
            frmVerTodosCliente frm = new frmVerTodosCliente();
            frm.ShowDialog();
        }

        private void cargarDGV()
        {
            using (MABEntities db = new MABEntities())
            {
                var clientes = db.Clientes;

                var ultimo = clientes.ToList().LastOrDefault();

                if(ultimo != null)
                {
                    var data = from cliente in db.Clientes
                           where cliente.Id < (ultimo.Id - 10)
                           select cliente;

                    ucBG.cargarDGV(data.ToList());
                }
                else
                {
                    ucBG.cargarDGV(clientes.ToList());
                }
            }
        }
    }
}