﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MAB.Models;

namespace MAB.Forms.Reparaciones
{
    public partial class frmEstadisticas : Form
    {
        public frmEstadisticas()
        {
            /**
             * TODO: Revisar el correcto funcionamiento
             * 
             */

            InitializeComponent();

            ucBottom.NumButtons = 2;

            ucBottom.Accion1 = "Reparaciones";
            ucBottom.Accion2 = "Repuestos";


            ucBottom.evAccion1 += verReparaciones;
            ucBottom.evAccion2 += verRepuestos;

            rbCantRepInMensual.Checked = true;
            rbCantRepOutMensual.Checked = true;

            cargarCBO();
        }

        private int? idRepuestos = null;

        #region eventos de ucBottom

        private void verReparaciones(object sender, EventArgs e)
        {
            pnlNumeroReparaciones.BringToFront();
        }

        private void verRepuestos(object sender, EventArgs e)
        {
            pnlRepuestosUsados.BringToFront();
        }

        #endregion

        #region eventos de RadioButtons CantRepIn

        private void rbCantRepInMensual_CheckedChanged(object sender, EventArgs e)
        {
            if(rbCantRepInMensual.Checked)
            {
                using (MABEntities db = new MABEntities())
                {
                    // Revisar lo que el negro hiso en su .java
                    chartCantRepIn.Series[0].Points.DataBind(db.reparacionesIngresadasMensuales().ToList(), "Reparaciones", "Mes", "");
                }
            }
        }

        private void rbCantRepInAnual_CheckedChanged(object sender, EventArgs e)
        {
            if(rbCantRepInAnual.Checked)
            {
                using (MABEntities db = new MABEntities())
                {
                    var data = db.reparacionesIngresadasAnuales().ToList();

                    chartCantRepIn.DataSource = data;
                }
            }
        }

        #endregion

        #region eventos de RadioButtons cantRepOut

        private void rbCantRepOutMensual_CheckedChanged(object sender, EventArgs e)
        {
            using (MABEntities db = new MABEntities())
            {
                /**
                 * select COUNT(r.Id) 
                 * from Reparaciones as r
                 * where r.fechaEgreso is not null
                 * group by MONTH(r.fechaIngreso);
                 * 
                 */

                var data = (from r in db.Reparaciones
                            where r.fechaEgreso != null
                            group r by new { month = r.fechaEgreso.Value.Month } into grouped
                            select new { count = grouped.Count() }).ToList();

                chartCantRepOut.DataSource = data;
            }
        }

        private void rbCantRepOutAnual_CheckedChanged(object sender, EventArgs e)
        {
            using (MABEntities db = new MABEntities())
            {
                /**
                 * select count(r.id)
                 * from Reparaciones as r
                 * where r.fechaEgreso is not null
                 * group by YEAR(r.fechaIngreso);
                 * 
                 */

                var data = (from r in db.Reparaciones
                            where r.fechaEgreso != null
                            group r by new { year = r.fechaEgreso.Value.Year } into grouped
                            select new { count = grouped.Count() }).ToList();

                chartCantRepOut.DataSource = data;
            }
        }

        #endregion

        #region eventos de RadioButtons RepuestosUsados

        private void rbRepuestoUsadoMensualmente_CheckedChanged(object sender, EventArgs e)
        {
            using (MABEntities db = new MABEntities())
            {
                /**
                 * select COUNT(r.ReparacionesId) 
                 * from ReparacionesRepuestos as r
                 * where r.RepuestoId = ** El Id del Repuesto que este seleccionado en el cbo **
                 * group by MONTH(r.Reparacion.fechaEgreso);
                 * 
                 */

                if (idRepuestos != null)
                {
                    var data = (from r in db.ReparacionesRepuestos
                                where r.RepuestosId == idRepuestos
                                group r by new { month = r.Reparaciones.fechaEgreso.Value.Month } into grouped
                                select new { count = grouped.Count() }).ToList();

                    chartCantRepOut.DataSource = data;
                }
                else
                {
                    MessageBox.Show("Debe seleccionar primero un repuesto", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void rbRepuestoUsadoAnualmente_CheckedChanged(object sender, EventArgs e)
        {
            using (MABEntities db = new MABEntities())
            {
                /**
                     * select COUNT(r.ReparacionesId) 
                     * from ReparacionesRepuestos as r
                     * where r.RepuestoId = ** El Id del Repuesto que este seleccionado en el cbo **
                     * group by MONTH(r.Reparacion.fechaEgreso);
                     * 
                     */

                if (idRepuestos != null)
                {
                    var data = (from r in db.ReparacionesRepuestos
                                where r.RepuestosId == idRepuestos
                                group r by new { month = r.Reparaciones.fechaEgreso.Value.Year } into grouped
                                select new { count = grouped.Count() }).ToList();

                    chartCantRepOut.DataSource = data;
                }
                else
                {
                    MessageBox.Show("Debe seleccionar primero un repuesto", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        private void cargarCBO()
        {
            using (MABEntities db = new MABEntities())
            {
                var repuestos = db.Repuestos.ToList();

                foreach (Models.Repuestos r in repuestos)
                {
                    cboRepuestoSeleccionado.Items.Add(r);
                }
                cboRepuestoSeleccionado.DisplayMember = "nombre";
            }
        }

        private void btnSeleccionarRepuesto_Click(object sender, EventArgs e)
        {
            idRepuestos = ((Models.Repuestos)cboRepuestoSeleccionado.SelectedItem).Id;

            using(MABEntities db = new MABEntities())
            {
                int cantReparacionesConRepuestoTal = (from r in db.ReparacionesRepuestos
                                        where r.RepuestosId == idRepuestos
                                        select r).Count();

                cclblCantReparacionesConRepuesto.Text = cantReparacionesConRepuestoTal.ToString();

                cclblPorcentajeReparacionesConRepuesto.Text = ((cantReparacionesConRepuestoTal / db.Reparaciones.Count()) * 100).ToString();
            }

            if (rbRepuestoUsadoMensualmente.Checked)
                rbRepuestoUsadoMensualmente.Checked = true;
            else
                rbRepuestoUsadoAnualmente.Checked = true;
        }
    }
}
