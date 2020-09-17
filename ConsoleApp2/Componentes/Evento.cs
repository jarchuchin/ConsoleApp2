using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Componentes
{
    public  class Evento : DBObject
    {


        public Evento()
        {
        }


        public int EnviarEventos()
        {
            string sql = "select e.id, e.idpaciente, e.idmedico, e.fechainicio, e.fechacreacion, CONCAT(dp.nombre, ' ', dp.apellidopaterno, ' ',  dp.apellidomaterno) as nombreCompleto, e.telefono, e.celular, e.correoelectronico, nm.id as idnotamedica, hc.subjetivo, hc.objetivo, a.descripcion as analisis, p.folio, p.iddatospersonales, p.iddomicilio, dp.nombre, dp.apellidopaterno, dp.apellidomaterno, ex.id as idexpediente from evento e left outer join notamedica nm on nm.idevento=e.id left outer join historiaclinica hc on hc.idnotamedica=nm.id left outer join analisis a on a.idnotamedica=nm.id left outer join paciente p on p.id=e.idpaciente left outer join datospersonales dp on dp.id=p.iddatospersonales left outer join expediente ex on ex.id=p.id  where e.idestatus=7 and e.idtipoitinerario=1 ";

            //solo para pruebas
            sql = sql + " order by p.id LIMIT 100";

            DataSet ds = this.ExecuteDataSet(sql, null);
            DataTable dt = ds.Tables[0];


            ConsoleApp2.srEnviarDatos.wsRecibirDatosSoapClient srED = new ConsoleApp2.srEnviarDatos.wsRecibirDatosSoapClient();

            foreach (DataRow dr in dt.Rows)
            {

                string cadEvento = Componentes.Utils.Convert_DataRowToJson(dr);
                string cadenaDatosPersonales = string.Empty;
                string cadenaAD = string.Empty;
                string cadenaEO = string.Empty;
                string cadenaBM = string.Empty;
                string cadenaMO = string.Empty;
                string cadenaPI = string.Empty;
                DataTable cadenaDC = new DataTable();
                string cadenaPlanTx = string.Empty;
                DataTable cadenaMC = new DataTable();

                //datos personales;
                sql = "select dp.* from datospersonales dp where dp.id=" + Convert.ToString(dr["iddatospersonales"]);
                DataSet dsDatosPersonales = this.ExecuteDataSet(sql, null);
                if (dsDatosPersonales.Tables[0].Rows.Count > 0 && Convert.ToString(dr["idnotamedica"]).Length > 0 )
                {
                    DataTable dtDatosPesonales = dsDatosPersonales.Tables[0];
                    cadenaDatosPersonales = Componentes.Utils.Convert_DataRowToJson(dtDatosPesonales.Rows[0]);


                    //antecedentes oftalmologifos
                    sql = "select af.* from antecedentesoftalmicos af where af.idexpediente=" + Convert.ToString(dr["idexpediente"]);
                    DataSet dsAF = this.ExecuteDataSet(sql, null);
                    if (dsAF.Tables[0].Rows.Count > 0 )
                    {
                        DataTable dtAF = dsAF.Tables[0];
                        cadenaAD = Componentes.Utils.Convert_DataRowToJson(dtAF.Rows[0]);
                    }


                    //exploracionoptometrica
                    sql = "select eo.* from exploracionoptometrica eo where eo.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsEO = this.ExecuteDataSet(sql, null);
                    if (dsEO.Tables[0].Rows.Count > 0 )
                    {
                        DataTable dtEO = dsEO.Tables[0];
                        cadenaEO = Componentes.Utils.Convert_DataRowToJson(dtEO.Rows[0]);
                    }


                    //biomicroscopia
                    sql = "select bm.* from biomicroscopia bm where bm.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsBM = this.ExecuteDataSet(sql, null);
                    if (dsBM.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtBM = dsBM.Tables[0];
                        cadenaBM = Componentes.Utils.Convert_DataRowToJson(dtBM.Rows[0]);
                    }


                    //motilidad ocular
                    sql = "select mo.* from motilidadocular mo where mo.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsMO = this.ExecuteDataSet(sql, null);
                    if (dsMO.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtMO = dsMO.Tables[0];
                        cadenaMO = Componentes.Utils.Convert_DataRowToJson(dtMO.Rows[0]);
                    }

                    //presion intraocular
                    sql = "select fo.* from fondoocular fo where fo.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsPI = this.ExecuteDataSet(sql, null);
                    if (dsPI.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtPI = dsPI.Tables[0];
                        cadenaMO = Componentes.Utils.Convert_DataRowToJson(dtPI.Rows[0]);
                    }

                    //diagnosticos
                    sql = "select dc.*, d.clave, d.nombre from diagnosticocitas dc left outer join diagnostico d on d.id=dc.iddiagnostico where dc.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsDC = this.ExecuteDataSet(sql, null);
                    cadenaDC = dsDC.Tables[0];
                    //if (dsDC.Tables[0].Rows.Count > 0 )
                    //{
                    //    DataTable dtDC = dsDC.Tables[0];
                    //    cadenaDC = dtDC; //Componentes.Utils.Convert_DataTableToJson(dtDC);
                    //}

                    //tratamiento
                    sql = "select tc.*, t.nombre, t.clave, t.idproducto  from tratamientocitas tc left outer join tratamiento t on t.id=tc.idtratamiento where tc.idnotamedica=" + Convert.ToString(dr["idnotamedica"]); 
                    DataSet dsTC = this.ExecuteDataSet(sql, null);
                    if (dsTC.Tables[0].Rows.Count > 0 )
                    {
                        DataTable dtTC = dsTC.Tables[0];
                        string ctx = string.Empty;
                        foreach (DataRow drtx in dtTC.Rows)
                        {
                            if (Convert.ToString(drtx["nombre"]).Trim().Length > 0 ) 
                            {
                                if (ctx.Length > 0)
                                {
                                    ctx = Convert.ToString(drtx["nombre"]).Trim();
                                }
                                else
                                {
                                    ctx += System.Environment.NewLine + Convert.ToString(drtx["nombre"] ) ;
                                }
                            }

                            if (Convert.ToString(drtx["observacion"]).Trim().Length > 0)
                            {
                               
                                 ctx += System.Environment.NewLine + "Observación:" + Convert.ToString(drtx["observacion"]).Trim();

                            }


                        }
                        cadenaPlanTx = ctx;

                    }

                    //medicamento
                    sql = "select mc.*, m.nombre, m.clave from medicamentoscitas mc left outer join medicamentos m on m.id=mc.idmedicamento where mc.idnotamedica=" + Convert.ToString(dr["idnotamedica"]);
                    DataSet dsMC = this.ExecuteDataSet(sql, null);
                    cadenaMC = dsMC.Tables[0];
                    //if (dsMC.Tables[0].Rows.Count > 0 )
                    //{
                    //    DataTable dtMC = dsMC.Tables[0];
                    //    cadenaMC = dtMC; //Componentes.Utils.Convert_DataTableToJson(dtMC);
                    //}

                    Console.WriteLine("---->>>>" + srED.RecibirDatosConsulta(cadEvento, cadenaDatosPersonales, cadenaAD, cadenaEO, cadenaBM, cadenaMO, cadenaPI, cadenaDC, cadenaPlanTx, cadenaMC));



                }


                //enviar enventos a webdxapi


            }


            return 1;

        }

    }
}
